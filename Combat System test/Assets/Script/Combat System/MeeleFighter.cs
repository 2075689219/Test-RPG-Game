using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttackState { none, start, impact, end }
public class MeeleFighter : MonoBehaviour
{
    [SerializeField] GameObject weapon;
    BoxCollider weaponCollider;
    Animator animator;
    public bool InAction { get; private set; } = false;

    private AttackState attackState;

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
    }

    IEnumerator Attack()
    {
        InAction = true;
        attackState = AttackState.start;

        float impactStartTime = 0.39f;
        float impactEndTime = 0.45f;

        animator.CrossFade("Attack01", 0.2f);//占用当前动画的20%时间过渡，必须保证前一个动画的时间不能太长
        yield return null;//等待一帧，保证接下来的动画处于过渡阶段
        var animState = animator.GetNextAnimatorStateInfo(1);//获取下一个动画的信息(也就是攻击动画)

        float timer = 0f;
        
                while (timer <= animState.length)
                {
                    timer += Time.deltaTime;
                    float normalizedTime = timer / animState.length;

                    if (attackState == AttackState.start && normalizedTime >= impactStartTime)
                    {
                        attackState = AttackState.impact;
                        //攻击生效
                        weaponCollider.enabled = true;
                        Debug.Log("Impact");
                    }
                    if (attackState == AttackState.impact && normalizedTime >= impactEndTime)
                    {
                        attackState = AttackState.end;
                        //结束攻击
                        weaponCollider.enabled = false;
                        Debug.Log("End");
                    }
                    else if (attackState == AttackState.end)
                    {
                        //TODO:连击，反击
                    }
        
                    yield return null;
                }
        
        attackState = AttackState.none;//重置攻击状态
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
        yield return new WaitForSeconds(animState.length);

        InAction = false;
    }
}
