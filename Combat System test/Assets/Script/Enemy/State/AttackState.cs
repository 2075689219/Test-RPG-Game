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

        // 停止Circling相关逻辑
        enemy.NavMeshAgent.isStopped = true;
        enemy.NavMeshAgent.ResetPath();

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
            StartCoroutine(Attack(Random.Range(0, enemy.EnemyItSelf.AttackList.Count + 1)));
        }
    }

    public override void Exit()
    {
        //调用 ResetPath() 是一种清理操作，让敌人不再受之前路径的干扰。
        enemy.NavMeshAgent.ResetPath();
    }

    IEnumerator Attack(int comboCount = 1)
    {
        isAttacking = true;
        enemy.Animator.applyRootMotion = true;//攻击时启用root motion
        enemy.EnemyItSelf.TryToAttack();

        for (int i = 1; i <= comboCount; i++)
        {
            yield return new WaitUntil(() => enemy.EnemyItSelf.AttackState == AttackStates.end);
            enemy.EnemyItSelf.TryToAttack();

        }

        //等待攻击动画播放完毕
        yield return new WaitUntil(() => enemy.EnemyItSelf.AttackState == AttackStates.none);

        enemy.Animator.applyRootMotion = false;//攻击结束后关闭root motion
        isAttacking = false;

        //只有处于攻击状态时才能切换到后退状态，确保在其他状态下不会切换到后退状态
        if(enemy.IsInState(EnemyState.Attack))
        enemy.ChangeState(EnemyState.RetreatAfterAttackState);//攻击完成后切换到后退状态
    }
}
