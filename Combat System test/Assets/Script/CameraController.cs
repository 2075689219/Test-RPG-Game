using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform followTarget;
    [SerializeField] float rotationSpeed = 2f;
    [SerializeField] float distanceFromTarget = 2.8f;
    [SerializeField] float maxVerticleAngle = 60f;
    [SerializeField] float minVerticleAngle = -10f;
    [SerializeField] Vector2 framingOffset;
    float rotationY = 0f;   //绕Y轴旋转的角度
    float rotationX = 0f;   //绕X轴旋转的角度

    private void Start()
    {
        Cursor.visible = false;                     //隐藏鼠标
        Cursor.lockState = CursorLockMode.Locked;   //锁定鼠标
    }

    private void Update()
    {
        rotationY += Input.GetAxis("Mouse X") * rotationSpeed;
        rotationX += Input.GetAxis("Mouse Y") * rotationSpeed * (-1);
        rotationX = Mathf.Clamp(rotationX, minVerticleAngle, maxVerticleAngle);//限制X轴旋转角度
        var targetRotation = Quaternion.Euler(rotationX, rotationY, 0);

        var focusPsition = followTarget.position + new Vector3(framingOffset.x, framingOffset.y, 0);//设置焦点位置

        transform.position = followTarget.position - targetRotation * new Vector3(0, 0, distanceFromTarget);

        transform.rotation = targetRotation;
    }

    public Quaternion PlaneRotation => Quaternion.Euler(0, rotationY, 0);
}
