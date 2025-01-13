using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatController : MonoBehaviour
{
    MeeleFighter meeleFighter;

    private void Awake()
    {
        meeleFighter = GetComponent<MeeleFighter>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Attack"))
        {
            //如果player周围有敌人处于攻击的第一下，并且player不处于攻击状态，就尝试反击
            var attakingEnemy = EnemyManager.instance.GetAttackingEnemy();
            if (attakingEnemy != null 
            && attakingEnemy.EnemyItSelf.IsCounterable == true
            && !meeleFighter.InAction)
            {
                StartCoroutine(meeleFighter.CounterAttack(attakingEnemy));//打断攻击
            }
            else
            {
                meeleFighter.TryToAttack();
            }
            
        }
    }
}
