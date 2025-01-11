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
    [SerializeField] float StopChaseDistance = 4f;
    [SerializeField] float 调整距离阈值 = 1f;//确保敌人对于玩家的移动不会太敏感

    AICombatState State;//当前状态
    EnemyController enemy;
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

        }
        else if (State == AICombatState.Chase)
        {
            if (distance <= StopChaseDistance + 0.05f)
            {
                StartIdel();
                return;
            }
            Chase();
            //enemy.NavMeshAgent.SetDestination(enemy.Target.transform.position);
        }
        else if (State == AICombatState.Circling)
        {

        }
        
    }

    public override void Exit()
    {

    }

    void StartIdel()//切换到Combat_Idle状态,而非Idle状态
    {
        enemy.Animator.SetBool("combatMode", true);
        State = AICombatState.Idle;
    }
    void StartChase()
    {
        enemy.Animator.SetBool("combatMode", false);
        State = AICombatState.Chase;
    }
    void Chase()
    {
        enemy.NavMeshAgent.SetDestination(enemy.Target.transform.position);
    }
}
