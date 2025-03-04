using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Video;

public enum AICombatState
{
    Idle,
    Chase,
    Circling
}
public class CombatMoveState : State<EnemyController>
{
    [SerializeField] float circlingSpeed = 20f;
    [SerializeField] float stopChaseDistance = 6f;
    [SerializeField] float 调整距离阈值 = 1f;//确保敌人对于玩家的移动不会太敏感
    [SerializeField] Vector2 idleTimeRange = new Vector2(2, 5);//idle状态持续时间
    [SerializeField] Vector2 circlingTimeRange = new Vector2(1, 3);//idle状态持续时间
    AICombatState State;//当前状态
    EnemyController enemy;

    public float timer = 0;
    int circlingDirection = 1;//1为左，-1为右
    public override void Enter(EnemyController owner)
    {
        enemy = owner;
        enemy.NavMeshAgent.stoppingDistance = stopChaseDistance;
        timer = Random.Range(idleTimeRange.x, idleTimeRange.y);
        enemy.stayingCombatTime = 0;
        enemy.Animator.SetBool("combatMode", true);
    }

    public override void Execute()
    {
        if (enemy.Target == null)
        {
            enemy.Target = enemy.FindTarget();
            if(enemy.Target == null)
            {
                enemy.ChangeState(EnemyState.Idle);
                return;
            }
        } 

        if(enemy.Target.Health <= 0)
        {
            enemy.Target = null;
            enemy.ChangeState(EnemyState.Idle);
            return;
        }
        timer -= Time.deltaTime;
        //enemy和player之间的距离
        var distance = Vector3.Distance(enemy.Target.transform.position, enemy.transform.position);

        if (distance > stopChaseDistance + 调整距离阈值)
        {
            StartChase();
        }

        if (State == AICombatState.Idle)
        {
            if (timer <= 0)
            {
                if (Random.Range(0, 2) == 0)
                {
                    StartIdle();
                }
                else
                {
                    StartCircling();
                }
            }

        }
        else if (State == AICombatState.Chase)
        {
            if (distance <= stopChaseDistance + 0.05f)
            {
                StartIdle();
                return;
            }
            Chase();
            //enemy.NavMeshAgent.SetDestination(enemy.Target.transform.position);
        }
        else if (State == AICombatState.Circling)
        {
            if (timer <= 0)
            {
                StartIdle();
                return;
            }

            var vecTotarget = enemy.Target.transform.position - transform.position;

            var rotatePsoition = Quaternion.Euler(0, circlingSpeed * circlingDirection * Time.deltaTime, 0) * vecTotarget;
            enemy.NavMeshAgent.Move(rotatePsoition - vecTotarget);

            enemy.transform.rotation = Quaternion.LookRotation(rotatePsoition);
        }

        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }

        enemy.stayingCombatTime += Time.deltaTime;
    }

    public override void Exit()
    {
        enemy.stayingCombatTime = 0;
    }

    void StartIdle()//切换到Combat_Idle状态,而非Idle状态
    {
        timer = Random.Range(idleTimeRange.x, idleTimeRange.y);
        State = AICombatState.Idle;
    }

    void StartChase()
    {
        State = AICombatState.Chase;
    }

    void StartCircling()
    {
        timer = Random.Range(circlingTimeRange.x, circlingTimeRange.y);
        State = AICombatState.Circling;
        enemy.NavMeshAgent.ResetPath();//TODO:这里有问题,关于敌人处于circling状态时，如果玩家移动，敌人的旋转中心不会变
        circlingDirection = (Random.Range(0, 2) == 0) ? -1 : 1;
    }
    void Chase()
    {
        enemy.NavMeshAgent.SetDestination(enemy.Target.transform.position);
    }
}
