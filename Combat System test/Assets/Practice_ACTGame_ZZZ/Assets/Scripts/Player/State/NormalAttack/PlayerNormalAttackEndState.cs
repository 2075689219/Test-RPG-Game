using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �����ͨ������ҡ״̬
/// </summary>
public class PlayerNormalAttackEndState : PlayerStateBase
{
    public override void Enter()
    {
        base.Enter();

        //���Ź�����ҡ����
        playerController.PlayAnimation($"Attack_Normal_{ playerModel.skiilConfig.currentNormalAttackIndex}_End", 0.1f) ;
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
            //���������ۼ�
            playerModel.skiilConfig.currentNormalAttackIndex++;
            if (playerModel.skiilConfig.currentNormalAttackIndex > playerModel.skiilConfig.normalAttackDamageMultiple.Length)
            {
                // ��ǰ������������
                playerModel.skiilConfig.currentNormalAttackIndex = 1;
            }

            //�л�����ͨ����״̬
            playerController.SwitchState(PlayerState.NormalAttack);
            return;
        }
        #endregion

        #region ����ƶ�
        if (playerController.inputMoveVec2 != Vector2.zero && statePlayTime > 0.5f)
        {
            //�л����ƶ�״̬
            playerController.SwitchState(PlayerState.Walk);
            // ��ǰ������������
            playerModel.skiilConfig.currentNormalAttackIndex = 1;
            return;
        }
        #endregion

        #region �������
        if (playerController.inputSystem.Player.Evade.triggered)
        {
            //�л�������״̬
            playerController.SwitchState(PlayerState.Evade_Back);
            // ��ǰ������������
            playerModel.skiilConfig.currentNormalAttackIndex = 1;
            return;
        }
        #endregion

        #region �����Ƿ񲥷Ž���
        if (IsAnimationEnd())
        {
            //�л�������״̬
            playerController.SwitchState(PlayerState.Idle);
            // ��ǰ������������
            playerModel.skiilConfig.currentNormalAttackIndex = 1;
            return;
        }
        #endregion
    }
}
