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
        enemy.Target = enemy.FindTarget();
        if(enemy.Target != null)
            enemy.ChangeState(EnemyState.CombatMove);//切换到追击状态
    }

    public override void Exit( )
    {

    }
}
