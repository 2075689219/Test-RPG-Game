using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerStateBase
{
    /// <summary>
    /// ����״̬
    /// </summary>
    public override void Enter()
    {
        base.Enter();

        switch (playerModel.currentState) 
        {
            case PlayerState.Idle:
                playerController.PlayAnimation("Idle", 0.25f);
                break;
            case PlayerState.Idle_AFK:
                playerController.PlayAnimation("Idle_AFK", 0.25f);
                break;
        }  
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

        switch (playerModel.currentState)
        {
            case PlayerState.Idle:
                #region ���һ�
                if (statePlayTime > 3) 
                {
                    //�л����һ�״̬
                    playerController.SwitchState(PlayerState.Idle_AFK);
                    return;
                }
                #endregion
                break;
            case PlayerState.Idle_AFK:
                #region ���һ���������
                if (IsAnimationEnd()) 
                {
                    //�л�������״̬
                    playerController.SwitchState(PlayerState.Idle);
                    return;
                }
                #endregion
                break;
        }

    }
}
