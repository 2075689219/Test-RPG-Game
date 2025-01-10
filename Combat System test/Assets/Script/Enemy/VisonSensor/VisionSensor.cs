using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionSensor : MonoBehaviour
{
    [SerializeField] EnemyController enemy;
    private SphereCollider sphereCollider;

    private void Awake()
    {
        sphereCollider = GetComponent<SphereCollider>();
        if (sphereCollider == null)
        {
            Debug.LogError("VisionSensor requires a SphereCollider component!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var fighter = other.GetComponent<MeeleFighter>();
        if (fighter != null)
        {
            enemy.TargetsInRange.Add(fighter);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var fighter = other.GetComponent<MeeleFighter>();
        if (fighter != null)
        {
            enemy.TargetsInRange.Remove(fighter);
        }
    }




//////////////////OnDrawGizmos()////////////////////////////////////////////////////////////////////

    private void OnDrawGizmos()
{
    if (enemy == null || sphereCollider == null) return;

    // 绘制目标连线
    Gizmos.color = Color.red;
    if (enemy.TargetsInRange != null)
    {
        foreach (var target in enemy.TargetsInRange)
        {
            if (target != null)
            {
                Gizmos.DrawLine(transform.position, target.transform.position);
            }
        }
    }

    // 绘制 SphereCollider 范围
    Gizmos.color = Color.cyan;
    Gizmos.DrawWireSphere(transform.position, sphereCollider.radius); // 碰撞体实际范围

    // 绘制视野扇形主线
    Gizmos.color = Color.green;
    Vector3 forward = transform.forward;
    Vector3 rightBoundary = Quaternion.Euler(0, enemy.fov / 2, 0) * forward;
    Vector3 leftBoundary = Quaternion.Euler(0, -enemy.fov / 2, 0) * forward;

    Gizmos.DrawRay(transform.position, rightBoundary * sphereCollider.radius);
    Gizmos.DrawRay(transform.position, leftBoundary * sphereCollider.radius);

    // 加粗视野线条（通过多条相邻线模拟）
    Gizmos.color = new Color(0f, 1f, 0f, 0.8f); // 调高透明度
    float thickness = 0.1f; // 线条厚度
    for (float offset = -thickness; offset <= thickness; offset += thickness / 2)
    {
        Vector3 rightOffset = Quaternion.Euler(0, offset, 0) * rightBoundary;
        Vector3 leftOffset = Quaternion.Euler(0, offset, 0) * leftBoundary;

        Gizmos.DrawRay(transform.position, rightOffset * sphereCollider.radius);
        Gizmos.DrawRay(transform.position, leftOffset * sphereCollider.radius);
    }

    // 在扇形内增加径向辅助线
    Gizmos.color = new Color(0f, 0.8f, 0f, 0.6f); // 辅助线颜色稍暗
    int lineCount = 10; // 辅助线数量
    float stepAngle = enemy.fov / (lineCount + 1);
    for (int i = 1; i <= lineCount; i++)
    {
        float angle = -enemy.fov / 2 + i * stepAngle;
        Vector3 direction = Quaternion.Euler(0, angle, 0) * forward;
        Gizmos.DrawRay(transform.position, direction * sphereCollider.radius);
    }
}

}
