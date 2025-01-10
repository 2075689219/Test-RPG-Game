using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class ChaseState : State<EnemyController>
{
    [SerializeField] float StopChaseDistance = 4f;
    EnemyController enemy;
    public override void Enter(EnemyController owner)
    {
        enemy = owner;
        enemy.NavMeshAgent.stoppingDistance = StopChaseDistance;
    }

    public override void Execute()
    {
        float moveAmount = enemy.NavMeshAgent.velocity.magnitude / enemy.NavMeshAgent.speed;
        enemy.NavMeshAgent.SetDestination(enemy.Target.transform.position); 
        enemy.Animator.SetFloat("moveAmount", moveAmount);
    }

    public override void Exit()
    {

    }
}
