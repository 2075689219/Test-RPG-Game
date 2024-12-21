using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Íæ¼Ò´óÕÐ½áÊø×´Ì¬
/// </summary>
public class PlayerBigSkillEndState : PlayerStateBase
{
    public override void Enter()
    {
        base.Enter();

        //ÇÐ»»¾µÍ·
        playerModel.bigSkillShot.SetActive(false);
        CameraManager.INSTANCE.cm_brain.m_DefaultBlend = new CinemachineBlendDefinition(CinemachineBlendDefinition.Style.EaseInOut, 1f);
        CameraManager.INSTANCE.freeLookCanmera.SetActive(true);
        CameraManager.INSTANCE.ResetFreeLookCamera();

        //²¥·Å¹¥»÷ºóÒ¡¶¯»­
        playerController.PlayAnimation("BigSkill_End", 0.0f);
    }

    public override void Update()
    {
        base.Update();

        #region ¼ì²â¹¥»÷
        if (playerController.inputSystem.Player.Fire.triggered)
        {
            //ÇÐ»»µ½ÆÕÍ¨¹¥»÷×´Ì¬
            playerController.SwitchState(PlayerState.NormalAttack);
            return;
        }
        #endregion

        #region ¼ì²âÒÆ¶¯
        if (playerController.inputMoveVec2 != Vector2.zero)
        {
            //ÇÐ»»µ½±¼ÅÜ×´Ì¬
            playerController.SwitchState(PlayerState.Walk);
            return;
        }
        #endregion

        #region ¼ì²âÉÁ±Ü
        if (playerController.inputSystem.Player.Evade.triggered)
        {
            //ÇÐ»»µ½ÉÁ±Ü×´Ì¬
            playerController.SwitchState(PlayerState.Evade_Back);
            return;
        }
        #endregion

        #region ¶¯»­ÊÇ·ñ²¥·Å½áÊø
        if (IsAnimationEnd())
        {
            //ÇÐ»»µ½´ý»ú×´Ì¬
            playerController.SwitchState(PlayerState.Idle);
            return;
        }
        #endregion
    }
}
