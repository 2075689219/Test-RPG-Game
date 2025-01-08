using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 6f;
    [SerializeField] float rotateSpeed = 300f;

    Quaternion targetRotation;
    CameraController cameraController;
    private void Awake()
    {
        cameraController = Camera.main.GetComponent<CameraController>();
    }

    private void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        var moveInput = (new Vector3(h, 0, v)).normalized;      //归一化防止对角线方向移动速度更快

        var moveDir = cameraController.PlaneRotation * moveInput;

        //有速度才移动和转向
        float moveAmount = Mathf.Abs(h) + Mathf.Abs(v);
        if (moveAmount > 0f)
        {
            transform.position += moveDir * moveSpeed * Time.deltaTime; //朝相机方向移动
            targetRotation = Quaternion.LookRotation(moveDir);          //转向
        }
        //平滑旋转
        transform.rotation = Quaternion.RotateTowards(transform.rotation,
        targetRotation, rotateSpeed * Time.deltaTime);
    }
}
