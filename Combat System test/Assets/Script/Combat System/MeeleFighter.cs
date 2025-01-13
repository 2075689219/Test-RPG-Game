using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum AttackStates { none, start, impact, end }
public class MeeleFighter : MonoBehaviour
{
    [SerializeField] List<AttackData> attackList;
    [SerializeField] GameObject weapon;

    BoxCollider weaponCollider;
    Animator animator;
    public bool InAction { get; private set; } = false;
    public bool InCounter { get; private set; } = false;
    public AttackStates AttackState { get; private set; }
    bool doCombo;
    int comboCount = 0;

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
    public void TryToAttack()
    {
        if (!InAction)
        {
            StartCoroutine(Attack());
        }
        else if (AttackState == AttackStates.impact || AttackState == AttackStates.end)
        {
            doCombo = true;
        }
    }

    IEnumerator Attack()
    {
        InAction = true;
        AttackState = AttackStates.start;

        animator.CrossFade(attackList[comboCount].AnimName, 0.2f);//占用当前动画的20%时间过渡，必须保证前一个动画的时间不能太长
        yield return null;//等待一帧，保证接下来的动画处于过渡阶段
        var animState = animator.GetNextAnimatorStateInfo(1);//获取下一个动画的信息(也就是攻击动画)

        float timer = 0f;

        while (timer <= animState.length)
        {
            timer += Time.deltaTime;
            float normalizedTime = timer / animState.length;

            if (InCounter) break;//TODO:这里有问题
            
            if (AttackState == AttackStates.start && normalizedTime >= attackList[comboCount].StartTime)
            {
                if (InCounter) break;//TODO:这里有问题
                AttackState = AttackStates.impact;
                //攻击生效
                EnableHitBox(attackList[comboCount]);
                //播放音效
                AudioManager.Instance.Play(attackList[comboCount].name);
            }
            if (AttackState == AttackStates.impact && normalizedTime >= attackList[comboCount].EndTime)
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
        if (other.tag == "HitBox" && !InAction)//攻击状态不可以被打断（播放受击动画）
        {
            StartCoroutine(PlayHitReaction());
        }
    }

    IEnumerator PlayHitReaction()
    {
        InAction = true;

        animator.CrossFade("Impact", 0.2f);//占用当前动画的20%时间过渡，必须保证前一个动画的时间不能太长
        yield return null;//等待一帧，保证接下来的动画处于过渡阶段
        var animState = animator.GetNextAnimatorStateInfo(1);//获取下一个动画的信息(也就是受击动画)
        yield return new WaitForSeconds(animState.length * 0.6f);

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

        //占用当前动画的20%时间过渡，必须保证前一个动画的时间不能太长
        animator.CrossFade("CounterAttack", 0.2f);
        enemy.Animator.CrossFade("Death", 0.2f);

        yield return null;//等待一帧，保证接下来的动画处于过渡阶段
        //获取下一个动画的信息(也就是受击动画)
        var animState = animator.GetNextAnimatorStateInfo(1);
        yield return new WaitForSeconds(animState.length * 0.6f);


        enemy.EnemyItSelf.InCounter = false;
        InCounter = false;
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
}
