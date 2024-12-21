using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��Ҵ��п�ʼ״̬
/// </summary>
public class PlayerBigSkillStartState : PlayerStateBase
{
    public override void Enter()
    {
        base.Enter();

        //�л���ͷ
        CameraManager.INSTANCE.cm_brain.m_DefaultBlend = new CinemachineBlendDefinition(CinemachineBlendDefinition.Style.Cut, 0f);
        CameraManager.INSTANCE.freeLookCanmera.SetActive(false);
        playerModel.bigSkillStartShot.SetActive(true);

       //���Ź�����ҡ����
       playerController.PlayAnimation("BigSkill_Start", 0.0f);
    }

    public override void Update()
    {
        base.Update();

        #region �����Ƿ񲥷Ž���
        if (IsAnimationEnd())
        {
            //�л�������״̬
            playerController.SwitchState(PlayerState.BigSkill);
            return;
        }
        #endregion
    }
}
