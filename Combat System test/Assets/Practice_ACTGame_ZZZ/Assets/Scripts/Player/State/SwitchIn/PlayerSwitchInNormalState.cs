using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��ɫ��ͨ�볡״̬
/// </summary>
public class PlayerSwitchInNormalState : PlayerStateBase
{
    public override void Enter()
    {
        base.Enter();

        playerController.PlayAnimation("SwitchIn_Normal", 0f);
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
            //�л����ƶ�״̬
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

        #region ��⶯���Ƿ����
        if (IsAnimationEnd())
        {
            //�л�������״̬
            playerController.SwitchState(PlayerState.Idle);
            return;
        }
        #endregion
    }
}
