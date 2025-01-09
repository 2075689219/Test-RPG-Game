using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeeleFighter : MonoBehaviour
{
    Animator animator;
    public bool InAction { get; private set; } = false;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
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

        animator.CrossFade("Attack01", 0.2f);//占用当前动画的20%时间过渡，必须保证前一个动画的时间不能太长
        yield return null;//等待一帧，保证接下来的动画处于过渡阶段
        var animState = animator.GetNextAnimatorStateInfo(1);//获取下一个动画的信息(也就是攻击动画)
        yield return new WaitForSeconds(animState.length);

        InAction = false;
    }
}
