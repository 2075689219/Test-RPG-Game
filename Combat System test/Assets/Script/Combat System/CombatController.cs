using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatController : MonoBehaviour
{
    EnemyController targetEnemy;
    public EnemyController TargetEnemy
    {
        get => targetEnemy;
        set
        {
            targetEnemy = value;
            // 如果目标为空，强制退出 CombatMode
            CombatMode = targetEnemy != null;
        }
    }
    private bool forceExitCombatMode = false; // 强制退出战斗模式标志
    /// <summary>
    /// 强制退出战斗模式
    /// </summary>
    public void ForceExitCombatMode()
    {
        forceExitCombatMode = true; // 标志设置为 true
        CombatMode = false;         // 强制关闭战斗模式
    }
    bool combatMode;
    public bool CombatMode
    {
        get => combatMode;
        set
        {
            if (forceExitCombatMode && value)
            {
                // 如果处于强制退出状态，禁止再次开启
                return;
            }

            combatMode = value && targetEnemy != null;  // 确保只有在有目标时才开启
            animator.SetBool("combatMode", combatMode);

            if (!combatMode)
            {
                forceExitCombatMode = false; // 重置强制退出标志
            }
        }
    }
    EnemyManager enemyManager;
    MeeleFighter meeleFighter;
    Animator animator;
    CameraController cam;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        meeleFighter = GetComponent<MeeleFighter>();
        cam = Camera.main.GetComponent<CameraController>();
    }

    private void Update()
    {
        // 切换 CombatMode 的时候，要检查目标是否存在
        if (Input.GetButtonDown("LockOn"))
        {
            CombatMode = !CombatMode;
            // 如果没有目标，强制退出 CombatMode
            if (TargetEnemy == null)
            {
                CombatMode = false;
            }
        }

        if (targetEnemy != null && targetEnemy.IsInState(EnemyState.Dead)) // 检查是否被销毁
        {
            TargetEnemy = null; // 目标被销毁，清除引用
        }

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

                CombatMode = true;
            }
        }

        // if (Input.GetButtonDown("LockOn"))
        // {
        //     CombatMode = !CombatMode;
        // }
    }

    private void OnAnimatorMove()
    {
        //在执行counterAttack时，只应用rootMotion
        if (meeleFighter.InCounter)

            transform.rotation = animator.rootRotation;
    }


    public Vector3 GetTargetingDirection()
    {
        var vectFromCam = transform.position - cam.transform.position;
        vectFromCam.y = 0;
        return vectFromCam.normalized;
    }

}
