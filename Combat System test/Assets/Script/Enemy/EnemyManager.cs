using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] Vector2 attackIntervalRange = new Vector2(2, 5);
    [SerializeField] CombatController player;
    EnemyController lastHighlightedEnemy; // 记录上一次发光的敌人
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

            if(enemy == player.TargetEnemy)
            enemy.DisableHighlight();
        }
    }

    float timer = 0;
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

        if (timer >= 0.1f)
        {
            var closestEnemy = GetClosestEnemyToPlayerDir();

            if (closestEnemy != lastHighlightedEnemy)
            {
                // 禁用上一个敌人的发光效果
                if (lastHighlightedEnemy != null)
                {
                    lastHighlightedEnemy.DisableHighlight();
                }

                // 启用当前选中敌人的发光效果
                if (closestEnemy != null)
                {
                    closestEnemy.EnableHighlight();
                }

                // 更新记录
                lastHighlightedEnemy = closestEnemy;
            }

            player.TargetEnemy = closestEnemy;
            timer = 0;
        }
        else
        {
            timer += Time.deltaTime;
        }

    }
    EnemyController SelectEnemyToAttack()
    {
        return enemiesInRange.OrderByDescending(e => e.stayingCombatTime).FirstOrDefault(e => e.Target != null);
    }

    public EnemyController GetAttackingEnemy()
    {
        return enemiesInRange.FirstOrDefault(e => e.IsInState(EnemyState.Attack));
    }

    public EnemyController GetClosestEnemyToPlayerDir()
    {
        var targetingDir = player.GetTargetingDirection();
        float minDistance = float.MaxValue;
        EnemyController closestEnemy = null;

        foreach (var enemy in enemiesInRange)
        {
            var vectToEnemy = enemy.transform.position - player.transform.position;
            vectToEnemy.y = 0;
            float angle = Vector3.Angle(targetingDir, vectToEnemy.normalized);
            float distance = vectToEnemy.magnitude * Mathf.Sin(angle * Mathf.Deg2Rad);

            if (distance < minDistance)
            {
                minDistance = distance;
                closestEnemy = enemy;
            }

        }
        return closestEnemy;
    }
}
