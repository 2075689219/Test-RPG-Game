using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �����ͨ����״̬
/// </summary>
public class PlayerNormalAttackState : PlayerStateBase
{
    private bool enterNextAttack;

    public override void Enter()
    {
        base.Enter();

        enterNextAttack = false;
        //���Ź�������
        playerController.PlayAnimation("Attack_Normal_" + playerModel.skiilConfig.currentNormalAttackIndex, 0.1f) ;
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

        // �ж��Ƿ�����
        if (NormalizedTime() >= 0.5f && playerController.inputSystem.Player.Fire.triggered) 
        {
            enterNextAttack = true;
        }

        #region �����Ƿ񲥷Ž���
        if (IsAnimationEnd())
        {
            if (enterNextAttack)
            {
                // �л�����һ���չ�
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
            else
            {
                //�л�����ͨ������ҡ״̬
                playerController.SwitchState(PlayerState.NormalAttackEnd);
                return;
            }
        }
        #endregion
    }
}
