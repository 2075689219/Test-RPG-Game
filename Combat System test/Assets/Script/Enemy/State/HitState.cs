using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitState : State<EnemyController>
{
    [SerializeField] float stunnTime = 0.5f;
    EnemyController enemy;
    public override void Enter(EnemyController owner)
    {
        StopAllCoroutines();
        enemy = owner;
        enemy.EnemyItSelf.OnHitComplete += () => StartCoroutine(GoToCombatMOvement());
    }

    IEnumerator GoToCombatMOvement()
    {
        yield return new WaitForSeconds(stunnTime);
        enemy.ChangeState(EnemyState.CombatMove);
    }
}
