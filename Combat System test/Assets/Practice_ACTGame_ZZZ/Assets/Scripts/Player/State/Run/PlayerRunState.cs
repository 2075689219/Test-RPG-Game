using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// ����ƶ�״̬
/// </summary>
public class PlayerRunState : PlayerStateBase
{
    private Camera mainCamera;

    public override void Enter()
    {
        base.Enter();

        mainCamera = Camera.main;

        //�ж��ƶ�״̬
        switch (playerModel.currentState)
        {
            case PlayerState.Walk:
                #region �������ҽŵ��ж�
                switch (playerModel.foot)
                {
                    case ModelFoot.Left:
                        playerController.PlayAnimation("Walk", 0.125f, 0.5f);
                        playerModel.foot = ModelFoot.Right;
                        break;
                    case ModelFoot.Right:
                        playerController.PlayAnimation("Walk", 0.125f, 0.0f);
                        playerModel.foot = ModelFoot.Left;
                        break;
                }
                #endregion
                break;
            case PlayerState.Run:
                #region �������ҽŵ��ж�
                switch (playerModel.foot)
                {
                    case ModelFoot.Left:
                        playerController.PlayAnimation("Run", 0.125f, 0.5f);
                        playerModel.foot = ModelFoot.Right;
                        break;
                    case ModelFoot.Right:
                        playerController.PlayAnimation("Run", 0.125f, 0.0f);
                        playerModel.foot = ModelFoot.Left;
                        break;
                }
                #endregion
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

        #region �������
        if (playerController.inputSystem.Player.Evade.triggered)
        {
            //�л�������״̬
            playerController.SwitchState(PlayerState.Evade_Front);
            return;
        }
        #endregion

        #region ������
        if (playerController.inputMoveVec2 == Vector2.zero)
        {
            playerController.SwitchState(PlayerState.RunEnd);
            return;
        }
        #endregion
        else
        {
            #region �����ƶ�����
            Vector3 inputMoveVec3 = new Vector3(playerController.inputMoveVec2.x, 0, playerController.inputMoveVec2.y);
            //��ȡ�������ת��Y
            float cameraAxisY = mainCamera.transform.rotation.eulerAngles.y;
            //��Ԫ�� �� ����
            Vector3 targetDic = Quaternion.Euler(0, cameraAxisY, 0) * inputMoveVec3;
            Quaternion targetQua = Quaternion.LookRotation(targetDic);
            //������ת�Ƕ�
            float angles = Mathf.Abs(targetQua.eulerAngles.y - playerModel.transform.eulerAngles.y);
            if (angles > 145f && angles < 215f && playerModel.currentState == PlayerState.Run)  
            {
                //�л�ת��״̬
                playerController.SwitchState(PlayerState.TurnBack);
            }
            else
            {
                playerModel.transform.rotation = Quaternion.Slerp(playerModel.transform.rotation, targetQua, Time.deltaTime * playerController.rotationSpeed);
            }
            #endregion
        }

        #region �����������
        if (playerModel.currentState == PlayerState.Walk && statePlayTime > 3f) 
        {
            //�л�������״̬
            playerController.SwitchState(PlayerState.Run);
            return;
        }
        #endregion
    }
}
