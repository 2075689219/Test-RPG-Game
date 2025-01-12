using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State<EnemyController>
{
    [SerializeField] float attackDistance = 2f;
    EnemyController enemy;
    bool isAttacking = false;
    public override void Enter(EnemyController owner)
    {
        enemy = owner;

        enemy.NavMeshAgent.stoppingDistance = attackDistance;
    }
    public override void Execute()
    {
        //如果没有目标，返回
        if (enemy.Target == null) return;
        //如果正在攻击，返回
        if (isAttacking) return;
        //开始攻击
        enemy.NavMeshAgent.SetDestination(enemy.Target.transform.position);

        //enemy和player之间的距离
        var distance = Vector3.Distance(enemy.Target.transform.position,
        enemy.transform.position);

        //当player在attackDistance范围内时，敌人攻击
        if (distance <= attackDistance + 0.05f)
        {
            StartCoroutine(Attack());
        }
    }
    public override void Exit()
    {
        enemy.NavMeshAgent.ResetPath();
    }

    IEnumerator Attack()
    {
        isAttacking = true;
        enemy.Animator.applyRootMotion = true;//攻击时启用root motion
        enemy.EnemyItSelf.TryToAttack();

        yield return new WaitUntil(() => enemy.EnemyItSelf.AttackState == AttackStates.none);

        enemy.Animator.applyRootMotion = false;//攻击结束后关闭root motion
        isAttacking = false;
    }
}
