using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;

public enum AttackStates { none, start, impact, end }
public class MeeleFighter : MonoBehaviour
{
    [field: SerializeField] public float Health { get; private set; } = 100;
    [SerializeField] List<AttackData> attackList;
    [SerializeField] List<AttackData> longRangeAttackList;
    [SerializeField] GameObject weapon;
    [SerializeField] Vector2 LongRangeAttackRange = new Vector2(2.4f, 6);

    public event Action OnGotHit;
    public event Action OnHitComplete;

    BoxCollider weaponCollider;
    Animator animator;
    public bool InAction { get; private set; } = false;
    public bool InCounter { get; private set; } = false;
    public AttackStates AttackState { get; private set; }
    bool doCombo;
    int comboCount = 0;
    public bool IsTakingHit { get; private set; }

    //////////////////Awake()////////////////////////////////////////////////////////////////////
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    //////////////////Start()////////////////////////////////////////////////////////////////////
    private void Start()
    {
        if (weapon != null)
        {
            weaponCollider = weapon.GetComponent<BoxCollider>();
            weaponCollider.enabled = false;
        }

    }
    //////////////////Attack()////////////////////////////////////////////////////////////////////
    public void TryToAttack(MeeleFighter target = null)
    {
        if (!InAction)
        {
            StartCoroutine(Attack(target));
        }
        else if (AttackState == AttackStates.impact || AttackState == AttackStates.end)
        {
            doCombo = true;
        }
    }

    IEnumerator Attack(MeeleFighter target = null)
    {
        var attack = attackList[comboCount];//攻击列表的具体第几个攻击
        InAction = true;
        AttackState = AttackStates.start;

        Vector3 startPos = transform.position;
        Vector3 targetPos = Vector3.zero;

        var attackDir = transform.forward;//攻击方向
        if (target != null)
        {
            var vecTotarget = target.transform.position - transform.position;
            vecTotarget.y = 0;
            attackDir = vecTotarget.normalized;

            float distance = vecTotarget.magnitude - attack.distanceToStop;
            if (distance > LongRangeAttackRange.x && longRangeAttackList.Count > 0)
            {
                if (UnityEngine.Random.Range(0, 2) == 0)
                {
                    attack = longRangeAttackList[0];
                }
                else
                {
                    attack = longRangeAttackList[1];
                }

            }
            if (attack.moveToTarget)
            {
                if (distance <= attack.maxMovement)
                    targetPos = target.transform.position - attackDir * attack.distanceToStop;
                else
                    targetPos = startPos + attackDir * attack.maxMovement;
            }

        }

        animator.CrossFade(attack.AnimName, 0.2f);//占用当前动画的20%时间过渡，必须保证前一个动画的时间不能太长
        yield return null;//等待一帧，保证接下来的动画处于过渡阶段
        var animState = animator.GetNextAnimatorStateInfo(1);//获取下一个动画的信息(也就是攻击动画)

        float timer = 0f;

        while (timer <= animState.length)
        {
            if (IsTakingHit) break;
            timer += Time.deltaTime;
            float normalizedTime = timer / animState.length;

            //if (InCounter) break;//TODO:这里有问题

            if (target != null && attack.moveToTarget)
            {
                float percTime = (normalizedTime - attack.StartTime) / (attack.EndTime - attack.StartTime);
                transform.position = Vector3.Lerp(startPos, targetPos, percTime);
            }

            if (AttackState == AttackStates.start && normalizedTime >= attack.StartTime)
            {
                if (InCounter) break;//TODO:这里有问题
                AttackState = AttackStates.impact;
                //攻击生效
                EnableHitBox(attack);
                //播放音效
                AudioManager.Instance.Play(attack.name);
            }
            if (AttackState == AttackStates.impact && normalizedTime >= attack.EndTime)
            {
                AttackState = AttackStates.end;
                //结束攻击
                DisableAllHitBox();
            }
            else if (AttackState == AttackStates.end)
            {
                if (doCombo)
                {
                    doCombo = false;
                    comboCount = (comboCount + 1) % attackList.Count;
                    StartCoroutine(Attack());
                    yield break;
                }
            }

            yield return null;
        }

        AttackState = AttackStates.none;//重置攻击状态
        comboCount = 0;
        InAction = false;
    }
    //////////////////HitReaction()////////////////////////////////////////////////////////////////////
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "HitBox" && !IsTakingHit && !InCounter)//攻击状态不可以被打断（播放受击动画）
        {
            OnGotHit?.Invoke();

            TakeDamage(20f);
            if (Health > 0)
                StartCoroutine(PlayHitReaction(other.GetComponentInParent<MeeleFighter>().transform));
            else
                PlayDeathAnim();
        }
    }

    IEnumerator PlayHitReaction(Transform attacker)
    {
        InAction = true;
        IsTakingHit = true;

        var dispVec = attacker.position - transform.position;
        dispVec.y = 0;
        transform.rotation = Quaternion.LookRotation(dispVec);

        animator.CrossFade("Impact", 0.2f);//占用当前动画的20%时间过渡，必须保证前一个动画的时间不能太长
        yield return null;//等待一帧，保证接下来的动画处于过渡阶段
        var animState = animator.GetNextAnimatorStateInfo(1);//获取下一个动画的信息(也就是受击动画)
        yield return new WaitForSeconds(animState.length * 0.6f);

        IsTakingHit = false;
        InAction = false;
    }

    public IEnumerator CounterAttack(EnemyController enemy)
    {
        InAction = true;
        InCounter = true;
        enemy.EnemyItSelf.InCounter = true;
        enemy.ChangeState(EnemyState.Dead);

        var dispVec = enemy.transform.position - transform.position;
        dispVec.y = 0;
        transform.rotation = Quaternion.LookRotation(dispVec);//面向敌人方向
        enemy.transform.rotation = Quaternion.LookRotation(-dispVec);//敌人面向玩家方向


        // 计算目标位置，稍微靠近敌人
        var targetPos = enemy.transform.position - dispVec.normalized * 1.5f;


        //占用当前动画的20%时间过渡，必须保证前一个动画的时间不能太长
        animator.CrossFade("CounterAttack", 0.2f);
        AudioManager.Instance.Play("kick");
        enemy.Animator.CrossFade("Death", 0.2f);

        yield return null;//等待一帧，保证接下来的动画处于过渡阶段
        //获取下一个动画的信息(也就是受击动画)
        var animState = animator.GetNextAnimatorStateInfo(1);

        float timer = 0f;
        while (timer <= animState.length)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, 4f * Time.deltaTime);
            yield return null;
            timer += Time.deltaTime;

        }

        enemy.EnemyItSelf.InCounter = false;
        InCounter = false;

        OnHitComplete?.Invoke();
        InAction = false;
    }

    void EnableHitBox(AttackData attack)
    {
        switch (attack.HitBox)
        {
            case AttackHitBox.weapon:
                weaponCollider.enabled = true;
                break;
        }

    }

    void DisableAllHitBox()
    {
        weaponCollider.enabled = false;
    }

    public List<AttackData> AttackList => attackList;
    public bool IsCounterable => AttackState == AttackStates.start && comboCount == 0;

    void TakeDamage(float damage)
    {
        Health = Mathf.Clamp(Health - damage, 0, Health);
    }
    void PlayDeathAnim()
    {
        animator.CrossFade("Death",0.2f);
    }
}
