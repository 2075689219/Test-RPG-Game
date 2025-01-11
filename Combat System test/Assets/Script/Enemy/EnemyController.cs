using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState//敌人状态
{
    Idle,
    CombatMove,
    Attack,
    Dead
}

public class EnemyController : MonoBehaviour
{
    [SerializeField] public float fov = 170;//敌人视野
    //敌人周围的目标
    public List<MeeleFighter> TargetsInRange { get; set; } = new List<MeeleFighter>();
    //敌人选中的目标(目前只能是player)
    public MeeleFighter Target { get; set; }
    //创建enemy状态机
    public StateMachine<EnemyController> stateMachine { get; private set; }

    //创建enemy状态字典
    Dictionary<EnemyState, State<EnemyController>> stateDict;

    public NavMeshAgent NavMeshAgent { get; private set; }
    public Animator Animator { get; private set; }
    private void Start()
    {   
        //获取Animator组件
        Animator = GetComponent<Animator>();
        //获取NavMeshAgent组件
        NavMeshAgent = GetComponent<NavMeshAgent>();

        //创建enemy状态字典并添加状态
        stateDict = new Dictionary<EnemyState, State<EnemyController>>();
        stateDict[EnemyState.Idle] = GetComponent<IdleState>();
        stateDict[EnemyState.CombatMove] = GetComponent<CombatMoveState>();

       //创建enemy状态机并切换到Idle状态
        stateMachine = new StateMachine<EnemyController>(this);
        stateMachine.ChangeState(stateDict[EnemyState.Idle]);
    }

    public void ChangeState(EnemyState state)
    {
        stateMachine.ChangeState(stateDict[state]);
    }
    private void Update()
    {
        //更新动画
        float moveAmount = NavMeshAgent.velocity.magnitude / NavMeshAgent.speed;
        Animator.SetFloat("moveAmount", moveAmount);
        
        //执行状态机
        stateMachine.Execute();
    }






}

