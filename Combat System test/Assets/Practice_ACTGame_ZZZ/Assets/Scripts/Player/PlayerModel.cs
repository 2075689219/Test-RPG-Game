using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ModelFoot
{
    Left, Right
}
public class PlayerModel : MonoBehaviour
{
    //����������
    [HideInInspector] public Animator animator;
    //��ҵ�ǰ״̬
    public PlayerState currentState;
    //��ɫ������
    [HideInInspector] public CharacterController characterController;
    //������Ϣ
    private AnimatorStateInfo stateInfo;
    //����
    public float gravity = -9.8f;
    //���������ļ�
    public SkiilConfig skiilConfig;
    //����Start��ͷ
    public GameObject bigSkillStartShot;
    //���о�ͷ
    public GameObject bigSkillShot;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
    }

    #region ����״̬
    public ModelFoot foot = ModelFoot.Left;

    /// <summary>
    /// ģ���볡
    /// </summary>
    public void Enter(Vector3 pos, Quaternion rot)
    {
        //ǿ���Ƴ��˳��߼�
        MonoManager.INSTANCE.RemoveUpdateAction(OnExit);

        #region ���ý�ɫ����λ��
        //�������ҵ�����
        Vector3 rightDirection = rot * Vector3.right;
        //����ƫ��0.8����λ
        pos += rightDirection * 0.8f;
        //������������
        Vector3 backDirection = rot * Vector3.back;
        //���ƫ��3����λ
        pos += backDirection * 4f;

        characterController.Move(pos - transform.position);
        transform.rotation = rot;
        #endregion
    }

    /// <summary>
    /// ģ���˳�
    /// </summary>
    public void Exit() 
    {
        animator.CrossFade("SwitchOut_Normal", 0.1f);
        MonoManager.INSTANCE.AddUpdateAction(OnExit);
    }

    /// <summary>
    /// �˳��߼�
    /// </summary>
    public void OnExit()
    {
        #region ��⶯���Ƿ񲥷Ž���
        if (IsAnimationEnd())
        {
            gameObject.SetActive(false);
            MonoManager.INSTANCE.RemoveUpdateAction(OnExit);
        }
        #endregion
    }

    /// <summary>
    /// �ж϶����Ƿ����
    /// </summary>
    public bool IsAnimationEnd()
    {
        //ˢ�¶���״̬
        stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        return stateInfo.normalizedTime >= 1.0f && !animator.IsInTransition(0);
    }

    /// <summary>
    /// �������
    /// </summary>
    public void SetOutLeftFoot()
    {
        foot = ModelFoot.Left;
    }

    /// <summary>
    /// �����ҽ�
    /// </summary>
    public void SetOutRightFoot()
    {
        foot = ModelFoot.Right;
    }

    private void OnDisable()
    {
        //������ͨ��������
        skiilConfig.currentNormalAttackIndex = 1;
    }
    #endregion
}
