using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��Ҵ��н���״̬
/// </summary>
public class PlayerBigSkillEndState : PlayerStateBase
{
    public override void Enter()
    {
        base.Enter();

        //�л���ͷ
        playerModel.bigSkillShot.SetActive(false);
        CameraManager.INSTANCE.cm_brain.m_DefaultBlend = new CinemachineBlendDefinition(CinemachineBlendDefinition.Style.EaseInOut, 1f);
        CameraManager.INSTANCE.freeLookCanmera.SetActive(true);
        CameraManager.INSTANCE.ResetFreeLookCamera();

        //���Ź�����ҡ����
        playerController.PlayAnimation("BigSkill_End", 0.0f);
    }

    public override void Update()
    {
        base.Update();

        #region ��⹥��
        if (playerController.inputSystem.Player.Fire.triggered)
        {
            //�л�����ͨ����״̬
            playerController.SwitchState(PlayerState.NormalAttack);
            return;
        }
        #endregion

        #region ����ƶ�
        if (playerController.inputMoveVec2 != Vector2.zero)
        {
            //�л�������״̬
            playerController.SwitchState(PlayerState.Walk);
            return;
        }
        #endregion

        #region �������
        if (playerController.inputSystem.Player.Evade.triggered)
        {
            //�л�������״̬
            playerController.SwitchState(PlayerState.Evade_Back);
            return;
        }
        #endregion

        #region �����Ƿ񲥷Ž���
        if (IsAnimationEnd())
        {
            //�л�������״̬
            playerController.SwitchState(PlayerState.Idle);
            return;
        }
        #endregion
    }
}
