using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class IdleState : State<EnemyController>
{
    EnemyController enemy;
    public override void Enter(EnemyController owner)
    {
        enemy = owner;
        enemy.Animator.SetBool("combatMode",false);
    }

    public override void Execute( )
    {
        foreach (var target in enemy.TargetsInRange)
        {
            var vecTotarget = target.transform.position - transform.position;
            float angle = Vector3.Angle(transform.forward, vecTotarget);

            if( angle <= enemy.fov /2)
            {
                enemy.Target = target;//选中目标
                enemy.ChangeState(EnemyState.CombatMove);//切换到追击状态
                break;
            }
        }

    }

    public override void Exit( )
    {

    }
}
