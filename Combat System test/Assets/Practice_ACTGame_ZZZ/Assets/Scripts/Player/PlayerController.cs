using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��ҿ�����
/// </summary>
public class PlayerController : SingleMomoBase<PlayerController>,IStateMachineOwner
{
    //����ϵͳ
    [HideInInspector]public InputSystem inputSystem;
    //����ƶ�����
    public Vector2 inputMoveVec2;

    //���ģ��
    public PlayerModel playerModel;

    //ת���ٶ�
    public float rotationSpeed = 8f;

    // ���ܼ�ʱ��
    private float evadeTimer = 1;

    //״̬��
    private StateMachine stateMachine;

    //���������Ϣ
    public PlayerConfig playerConfig;

    //���
    private List<PlayerModel> controllableModels;

    //��ǰ���ƵĽ�ɫ�±�
    private int currentModelIndex;

    protected override void Awake()
    {
        base.Awake();

        stateMachine = new StateMachine(this);
        inputSystem= new InputSystem();

        controllableModels = new List<PlayerModel>();

        #region ������ɫģ��
        for (int i = 0; i < playerConfig.models.Length; i++) 
        {
            GameObject modle = Instantiate(playerConfig.models[i], transform);
            controllableModels.Add(modle.GetComponent<PlayerModel>());
            controllableModels[i].gameObject.SetActive(false);
        }
        #endregion

        #region �ٿص�һ����ɫ
        currentModelIndex = 0;
        controllableModels[currentModelIndex].gameObject.SetActive(true);
        playerModel = controllableModels[currentModelIndex];
        #endregion
    }

    private void Start()
    {
        //�������
        LockMouse();
        //�л�������״̬
        SwitchState(PlayerState.Idle);
    }

    /// <summary>
    /// �л�״̬
    /// </summary>
    /// <param name="playerState">״̬</param>
    public void SwitchState(PlayerState playerState)
    {
        playerModel.currentState= playerState;
        switch (playerState)
        {
            case PlayerState.Idle:
            case PlayerState.Idle_AFK:
                stateMachine.EnterState<PlayerIdleState>(true);
                break;
            case PlayerState.Walk:
            case PlayerState.Run:
                stateMachine.EnterState<PlayerRunState>(true);
                break;
            case PlayerState.RunEnd:
                stateMachine.EnterState<PlayerRunEndState>();
                break;
            case PlayerState.TurnBack:
                stateMachine.EnterState<PlayerTurnBackState>();
                break;
            case PlayerState.Evade_Front:
            case PlayerState.Evade_Back:
                if (evadeTimer !=1)
                {
                    return;
                }
                stateMachine.EnterState<PlayerEvadeState>();
                evadeTimer = 0f;
                break;
            case PlayerState.Evade_Front_End:
            case PlayerState.Evade_Back_End:
                stateMachine.EnterState<PlayerEvadeEndState>();
                break;
            case PlayerState.NormalAttack:
                stateMachine.EnterState<PlayerNormalAttackState>(true);
                break;
            case PlayerState.NormalAttackEnd:
                stateMachine.EnterState<PlayerNormalAttackEndState>();
                break;
            case PlayerState.BigSkillStart:
                stateMachine.EnterState<PlayerBigSkillStartState>();
                break;
            case PlayerState.BigSkill:
                stateMachine.EnterState<PlayerBigSkillState>();
                break;
            case PlayerState.BigSkillEnd:
                stateMachine.EnterState<PlayerBigSkillEndState>();
                break;
            case PlayerState.SwitchInNormal:
                stateMachine.EnterState<PlayerSwitchInNormalState>();
                break;
        }
    }

    /// <summary>
    /// �л�����һ��ģ��
    /// </summary>
    public void SwitchNextModel()
    {
        //ˢ��״̬��
        stateMachine.Clear();
        //�˳���ǰģ��
        playerModel.Exit();
        #region ������һ��ģ��
        currentModelIndex++;
        if (currentModelIndex >= controllableModels.Count) 
        {
            currentModelIndex = 0;
        }
        PlayerModel nextModel = controllableModels[currentModelIndex];
        nextModel.gameObject.SetActive(true);

        Vector3 prevPos = playerModel.transform.position;
        Quaternion prevRot = playerModel.transform.rotation;

        playerModel = nextModel;
        #endregion
        //������һ��ģ��
        playerModel.Enter(prevPos, prevRot);
        //�л����볡״̬
        SwitchState(PlayerState.SwitchInNormal);
    }

    /// <summary>
    /// �л�����һ��ģ��
    /// </summary>
    public void SwitchLastModel()
    {
        //ˢ��״̬��
        stateMachine.Clear();
        //�˳���ǰģ��
        playerModel.Exit();
        #region ������һ��ģ��
        currentModelIndex--;
        if (currentModelIndex < 0)
        {
            currentModelIndex = controllableModels.Count - 1;
        }
        PlayerModel nextModel = controllableModels[currentModelIndex];
        nextModel.gameObject.SetActive(true);

        Vector3 prevPos = playerModel.transform.position;
        Quaternion prevRot = playerModel.transform.rotation;

        playerModel = nextModel;
        #endregion
        //������һ��ģ��
        playerModel.Enter(prevPos, prevRot);
        //�л����볡״̬
        SwitchState(PlayerState.SwitchInNormal);
    }
    

    /// <summary>
    /// ���Ŷ���
    /// </summary>
    /// <param name="animationName">��������</param>
    /// <param name="fixedTransitionDurarion">����ʱ��</param>
    public void PlayAnimation(string animationName,float fixedTransitionDurarion = 0.25f)
    {
        playerModel.animator.CrossFadeInFixedTime(animationName, fixedTransitionDurarion);
    }
    /// <summary>
    /// ���Ŷ���
    /// </summary>
    /// <param name="animationName">��������</param>
    /// <param name="fixedTransitionDurarion">����ʱ��</param>
    /// <param name="fixedTimeOffset">������ʼ����ƫ��</param>
    public void PlayAnimation(string animationName, float fixedTransitionDurarion = 0.25f, float fixedTimeOffset = 0f)
    {
        playerModel.animator.CrossFadeInFixedTime(animationName, fixedTransitionDurarion, 0, fixedTimeOffset);
    }

    private void FixedUpdate()
    {
        //������ҵ��ƶ�����
        inputMoveVec2 = inputSystem.Player.Move.ReadValue<Vector2>().normalized;
        //�ָ����ܼ�ʱ��
        if (evadeTimer < 1f) 
        {
            evadeTimer += Time.deltaTime;
            if (evadeTimer > 1f) 
            {
                evadeTimer = 1f;
            }
        }
    }

    private void LockMouse()
    {
        //�������������Ļ�м�
        Cursor.lockState = CursorLockMode.Locked;
        //���ع��
        Cursor.visible = false;
    }

    private void OnEnable()
    {
        inputSystem.Enable();
    }
    private void OnDisable()
    {
        inputSystem.Disable();
    }
}
