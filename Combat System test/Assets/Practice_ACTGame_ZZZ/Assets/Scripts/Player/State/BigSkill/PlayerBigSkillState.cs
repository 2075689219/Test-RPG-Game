using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��Ҵ�����β״̬
/// </summary>
public class PlayerBigSkillState : PlayerStateBase
{
    public override void Enter()
    {
        base.Enter();

        //�л���ͷ
        playerModel.bigSkillStartShot.SetActive(false);
        playerModel.bigSkillShot.SetActive(true);

        //���Ź�����ҡ����
        playerController.PlayAnimation("BigSkill", 0.0f);
    }

    public override void Update()
    {
        base.Update();

        #region �����Ƿ񲥷Ž���
        if (IsAnimationEnd())
        {
            //�л�������״̬
            playerController.SwitchState(PlayerState.BigSkillEnd);
            return;
        }
        #endregion
    }
}
