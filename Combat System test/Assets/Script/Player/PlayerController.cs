using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 6f;
    [SerializeField] float rotateSpeed = 300f;
    [Header("Ground Check")]
    [SerializeField] float groundCheckRidous = 0.2f;
    [SerializeField] Vector3 groundCheckOffset;
    [SerializeField] LayerMask groundLayer;
    [Header("盔甲音效设置")]
    [SerializeField] private float minMoveThreshold = 0.2f; // 播放音效的最小移动量
    [SerializeField] private float soundInterval = 22f; // 音效播放间隔
    private float soundTimer = 0f; // 音效计时器
    Quaternion targetRotation;
    [SerializeField] CameraController cameraController;
    Animator animator;
    CharacterController characterController;
    MeeleFighter meeleFighter;
    CombatController combatController;
    [Header("BGM设置")]
    [SerializeField] private float fadeDuration = 2f; // 渐入渐出的时间
    private float combatBGMMinTime = 30f; // combatBGM 最小播放时间
    private float combatBGMTimer = 0f; // combatBGM 计时器
    private bool isCombatBGMPlaying = false; // 当前是否播放 combatBGM

    bool isGrounded = false;
    float ySpeed = 0f;

    private void Awake()
    {
        cameraController = Camera.main.GetComponent<CameraController>();
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        meeleFighter = GetComponent<MeeleFighter>();
        combatController = GetComponent<CombatController>();


        // 初始化 CombatMode 为 false
        if (combatController != null)
        {
            combatController.CombatMode = false;
        }
        else
        {
            Debug.LogError("CombatController is not assigned or missing.");
        }
    }

    private void Update()
    {

        if (meeleFighter.InAction)
        {
            targetRotation = transform.rotation;
            animator.SetFloat("forwardSpeed", 0);
            return;
        }

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        var moveInput = new Vector3(h, 0, v).normalized;//归一化防止对角线方向移动速度更快

        var moveDir = cameraController.PlaneRotation * moveInput;
        float moveAmount = Mathf.Clamp01(Mathf.Abs(h) + Mathf.Abs(v));

        // 如果处于 CombatMode，则将移动速度降低为原来的 40%
        float adjustedMoveSpeed = combatController.CombatMode ? moveSpeed * 0.4f : moveSpeed;

        //检测是否处于地面，设置y轴速度
        GroundCheck();
        if (isGrounded)
        {
            ySpeed = -0.3f;
        }
        else
        {
            ySpeed += Physics.gravity.y * Time.deltaTime;
        }

        var velocity = moveDir * moveSpeed;

        if (combatController.CombatMode)
        {
            var targetVec = combatController.TargetEnemy.transform.position - transform.position;
            targetVec.y = 0;

            if (moveAmount > 0f)
            {
                targetRotation = Quaternion.LookRotation(targetVec);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
            }

            velocity /= 3;
            animator.SetFloat("forwardSpeed", Vector3.Dot(transform.forward, velocity) / moveSpeed, 0.2f, Time.deltaTime);
            animator.SetFloat("strafeSpeed", Mathf.Sin(Vector3.SignedAngle(transform.forward, velocity, Vector3.up) * Mathf.Deg2Rad), 0.2f, Time.deltaTime);
        }
        else
        {
            if (moveAmount > 0f)
            {
                targetRotation = Quaternion.LookRotation(moveDir);//转向
            }
            //平滑旋转
            transform.rotation = Quaternion.RotateTowards(transform.rotation,
            targetRotation, rotateSpeed * Time.deltaTime);

            animator.SetFloat("forwardSpeed", moveAmount, 0.25f, Time.deltaTime);

        }

        velocity.y = ySpeed;
        //实现移动和转向
        characterController.Move(velocity * Time.deltaTime);

        //BGM设置
        AudioManager.Instance.SetVolume("combatBGM", 0.1f);
        AudioManager.Instance.SetVolume("normalBGM", 0.1f);
        if (combatController.CombatMode)
        {
            combatBGMTimer += Time.deltaTime;

            if (!isCombatBGMPlaying)
            {
                AudioManager.Instance.SetVolume("combatBGM", 0.1f);
                AudioManager.Instance.SetVolume("normalBGM", 0.1f);
                StartCoroutine(SwitchBGM("combatBGM", "normalBGM", true));
                isCombatBGMPlaying = true;
            }
        }
        else
        {
            combatBGMTimer += Time.deltaTime;

            if (isCombatBGMPlaying && combatBGMTimer >= combatBGMMinTime)
            {
                AudioManager.Instance.SetVolume("combatBGM", 0.1f);
                AudioManager.Instance.SetVolume("normalBGM", 0.1f);
                StartCoroutine(SwitchBGM("normalBGM", "combatBGM", false));
                isCombatBGMPlaying = false;
                combatBGMTimer = 0f; // 重置计时器
            }
        }
    }

    private IEnumerator SwitchBGM(string targetBGM, string currentBGM, bool isEnteringCombat)
    {
        // 获取当前播放的 AudioSource
        AudioSource currentSource = AudioManager.Instance.GetAudioSource(currentBGM);
        AudioSource targetSource = AudioManager.Instance.GetAudioSource(targetBGM);

        // 如果目标音乐已经在播放，直接退出
        if (targetSource != null && targetSource.isPlaying)
        {
            yield break;
        }

        // 渐出当前音乐
        if (currentSource != null && currentSource.isPlaying)
        {
            while (currentSource.volume > 0)
            {
                currentSource.volume -= Time.deltaTime / fadeDuration;
                yield return null;
            }
            currentSource.Stop();
        }

        // 渐入目标音乐
        if (targetSource != null)
        {
            targetSource.volume = 0;
            targetSource.Play();

            while (targetSource.volume < 1)
            {
                targetSource.volume += Time.deltaTime / fadeDuration;
                yield return null;
            }
        }
    }

    //地面检测
    void GroundCheck()
    {
        isGrounded = Physics.CheckSphere(transform.TransformPoint(groundCheckOffset)
        , groundCheckRidous, groundLayer);
    }




    //unity编辑器中绘制Gizmos，方便观察GroundCheck
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawSphere(transform.TransformPoint(groundCheckOffset), groundCheckRidous);
    }
}
