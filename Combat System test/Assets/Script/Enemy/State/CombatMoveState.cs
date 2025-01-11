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
    [SerializeField] float StopChaseDistance = 4f;
    [SerializeField] float 调整距离阈值 = 1f;//确保敌人对于玩家的移动不会太敏感
    [SerializeField] Vector2 idleTimeRange = new Vector2(2, 5);//idle状态持续时间
    [SerializeField] Vector2 circlingTimeRange = new Vector2(1, 3);//idle状态持续时间
    AICombatState State;//当前状态
    EnemyController enemy;

    float timer = 0;
    int circlingDirection = 1;//1为左，-1为右
    public override void Enter(EnemyController owner)
    {
        enemy = owner;
        enemy.NavMeshAgent.stoppingDistance = StopChaseDistance;
    }

    public override void Execute()
    {
        //enemy和player之间的距离
        var distance = Vector3.Distance(enemy.Target.transform.position, enemy.transform.position);

        if (distance > StopChaseDistance + 调整距离阈值)
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
            if (distance <= StopChaseDistance + 0.05f)
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
            transform.RotateAround(enemy.Target.transform.position, Vector3.up, circlingSpeed * circlingDirection * Time.deltaTime);
        }

        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
    }

    public override void Exit()
    {

    }

    void StartIdle()//切换到Combat_Idle状态,而非Idle状态
    {
        enemy.Animator.SetBool("combatMode", true);
         enemy.Animator.SetBool("circling", false);
        timer = Random.Range(idleTimeRange.x, idleTimeRange.y);
        State = AICombatState.Idle;
    }

    void StartChase()
    {
        enemy.Animator.SetBool("combatMode", false);
        enemy.Animator.SetBool("circling", false);
        State = AICombatState.Chase;
    }

    void StartCircling()
    {
        timer = Random.Range(circlingTimeRange.x, circlingTimeRange.y);
        State = AICombatState.Circling;

        circlingDirection = (Random.Range(0, 2) == 0) ? -1 : 1;

        enemy.Animator.SetBool("circling", true);
        enemy.Animator.SetFloat("circlingDirection", circlingDirection);
    }
    void Chase()
    {
        enemy.NavMeshAgent.SetDestination(enemy.Target.transform.position);
    }
}
