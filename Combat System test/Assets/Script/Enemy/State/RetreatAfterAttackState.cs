using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RetreatAfterAttackState : State<EnemyController>
{
    [SerializeField] float retreatSpeed = 1.5f;
    [SerializeField] float stopRetreatDistance = 6f;//应该与CombatMoveState的stopChaseDistance相同
    EnemyController enemy;
    public override void Enter(EnemyController owner)
    {
        enemy = owner;
    }

    override public void Execute()
    {
        if (Vector3.Distance(enemy.Target.transform.position, enemy.transform.position) > stopRetreatDistance)
        {
            enemy.ChangeState(EnemyState.CombatMove);
        }

        //面向目标的方向
        var vetToTarget = enemy.Target.transform.position - enemy.transform.position;
        //后退
        enemy.NavMeshAgent.Move(-vetToTarget.normalized * retreatSpeed * Time.deltaTime);

        //面向玩家(任何情况下都要面向对方),逐渐转向而不是直接面朝对方
        vetToTarget.y = 0;//保持在水平面上
        transform.rotation = Quaternion.RotateTowards(enemy.transform.rotation, Quaternion.LookRotation(vetToTarget), 360 * Time.deltaTime);
    }

}
