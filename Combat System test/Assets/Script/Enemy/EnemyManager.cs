using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] Vector2 attackIntervalRange = new Vector2(2,5 );
    public static EnemyManager instance { get; private set; }
    private void Awake()
    {
        instance = this;
        enemiesInRange = new List<EnemyController>();
    }

    List<EnemyController> enemiesInRange;
    float AttackTimeInterval = 2f;//敌人群体攻击间隔

    public void AddEnemyInRange(EnemyController enemy)
    {
        if (!enemiesInRange.Contains(enemy))
        {
            enemiesInRange.Add(enemy);
        }
    }

    public void RemoveEnemyInRange(EnemyController enemy)
    {
        if (enemiesInRange.Contains(enemy))
        {
            enemiesInRange.Remove(enemy);
        }
    }

    private void Update()
    {
        if (enemiesInRange.Count == 0) return;

        if (!enemiesInRange.Any(e => e.IsInState(EnemyState.Attack)))
        {
            if (AttackTimeInterval > 0)
            {
                AttackTimeInterval -= Time.deltaTime;
            }
            else
            {
                //如果player周围没有敌人处于攻击状态，就随机选一个敌人进行攻击
                var attackingEnemy = SelectEnemyToAttack();
                if (attackingEnemy != null)
                {
                    attackingEnemy.ChangeState(EnemyState.Attack);
                }

                //重置攻击间隔
                AttackTimeInterval = Random.Range(attackIntervalRange.x, attackIntervalRange.y);
            }
        }
    }
    EnemyController SelectEnemyToAttack()
    {
        return enemiesInRange.OrderByDescending(e => e.stayingCombatTime).FirstOrDefault();
    }

}
