using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    Idle, Idle_AFK,
    Walk, Run, RunEnd, TurnBack,
    Evade_Front, Evade_Back, Evade_Front_End, Evade_Back_End,
    NormalAttack, NormalAttackEnd,
    BigSkillStart, BigSkill, BigSkillEnd,
    SwitchInNormal
}
public class PlayerStateBase : StateBase
{
    //��ҿ�����
    protected PlayerController playerController;
    //���ģ��
    protected PlayerModel playerModel;
    //������Ϣ
    private AnimatorStateInfo stateInfo;
    //��¼��ǰ״̬�����ʱ��
    protected float statePlayTime = 0;

    public override void Init(IStateMachineOwner owner)
    {
        playerController = (PlayerController)owner;
        playerModel = playerController.playerModel;
    }

    public override void Enter()
    {
        statePlayTime = 0;
    }

    public override void Exit()
    {
    }

    public override void FixedUpdate()
    {
        //ʩ������Ӱ��
        playerModel.characterController.Move(new Vector3(0, playerModel.gravity * Time.deltaTime, 0));
        //ˢ�¶���״̬
        stateInfo = playerModel.animator.GetCurrentAnimatorStateInfo(0);
    }

    public override void LateUpdate()
    {
    }

    public override void UnInit()
    {

    }

    public override void Update()
    {
        //ʩ������Ӱ��
        playerModel.characterController.Move(new Vector3(0, playerModel.gravity * Time.deltaTime, 0));
        //״̬����ʱ���ʱ
        statePlayTime += Time.deltaTime;

        #region ����ɫ�л�
        if (playerController.inputSystem.Player.SwitchDown.triggered &&
            playerModel.currentState!= PlayerState.BigSkillStart&&
            playerModel.currentState != PlayerState.BigSkill)
        {
            //�л���ɫ
            playerController.SwitchNextModel();
        }
        if (playerController.inputSystem.Player.SwitchUp.triggered &&
        playerModel.currentState != PlayerState.BigSkillStart &&
        playerModel.currentState != PlayerState.BigSkill)
        {
            //�л���ɫ
            playerController.SwitchLastModel();
        }
        #endregion
    }

    /// <summary>
    /// �ж϶����Ƿ����
    /// </summary>
    public bool IsAnimationEnd()
    {
        //ˢ�¶���״̬
        stateInfo = playerModel.animator.GetCurrentAnimatorStateInfo(0);

        return stateInfo.normalizedTime >= 1.0f && !playerModel.animator.IsInTransition(0);
    }

    /// <summary>
    /// ��ȡ�������Ž���
    /// </summary>
    /// <returns>�������Ž���</returns>
    public float NormalizedTime()
    {
        //ˢ�¶���״̬
        stateInfo = playerModel.animator.GetCurrentAnimatorStateInfo(0);

        return stateInfo.normalizedTime;
    }
}
