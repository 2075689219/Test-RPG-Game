using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �������״̬
/// </summary>
public class PlayerEvadeState : PlayerStateBase
{
    public override void Enter()
    {
        base.Enter();

        #region �ж�ǰ������
        switch (playerModel.currentState)
        {
            case PlayerState.Evade_Front:
                playerController.PlayAnimation("Evade_Front", 0.1f);
                break;
            case PlayerState.Evade_Back:
                playerController.PlayAnimation("Evade_Back", 0.1f);
                break;
        }
        #endregion
    }
    public override void Update()
    {
        base.Update();

        #region ������
        if (playerController.inputSystem.Player.BigSkill.triggered)
        {
            //�л����������״̬
            playerController.SwitchState(PlayerState.BigSkillStart);
            return;
        }
        #endregion

        #region �����Ƿ񲥷Ž���
        if (IsAnimationEnd())
        {
            switch (playerModel.currentState)
            {
                case PlayerState.Evade_Front:
                    playerController.SwitchState(PlayerState.Evade_Front_End);
                    break;
                case PlayerState.Evade_Back:
                    playerController.SwitchState(PlayerState.Evade_Back_End);
                    break;
            }
            return;
        }
        #endregion
    }
}
