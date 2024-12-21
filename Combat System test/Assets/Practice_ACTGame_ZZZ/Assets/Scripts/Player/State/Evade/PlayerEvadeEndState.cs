using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ������ܽ���״̬
/// </summary>
public class PlayerEvadeEndState : PlayerStateBase
{
    public override void Enter()
    {
        base.Enter();

        #region �ж�ǰ������
        switch (playerModel.currentState)
        {
            case PlayerState.Evade_Front_End:
                playerController.PlayAnimation("Evade_Front_End",0.1f);
                break;
            case PlayerState.Evade_Back_End:
                playerController.PlayAnimation("Evade_Back_End", 0.1f);
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
            switch (playerModel.currentState)
            {
                case PlayerState.Evade_Front_End:
                    playerController.SwitchState(PlayerState.Run);
                    break;
                case PlayerState.Evade_Back_End:
                    playerController.SwitchState(PlayerState.Walk);
                    break;
            }
            return;
        }
        #endregion

        #region �����Ƿ񲥷Ž���
        if (IsAnimationEnd())
        {
            playerController.SwitchState(PlayerState.Idle);
            return;
        }
        #endregion
    }
}
