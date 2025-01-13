using System.Collections;
using UnityEngine;

public class DeadState : State<EnemyController>
{
    EnemyController enemy;

    public override void Enter(EnemyController owner)
    {
        enemy = owner;
        
        // 禁用敌人感知和控制
        enemy.visionSensor.gameObject.SetActive(false);
        EnemyManager.instance.RemoveEnemyInRange(enemy);
        enemy.CharacterController.enabled = false;
        enemy.NavMeshAgent.enabled = false;

        // 开始尸体下沉与销毁协程
        enemy.StartCoroutine(HandleCorpseFadeAndDestroy());
    }

    private IEnumerator HandleCorpseFadeAndDestroy()
    {
        float destroyDelay = 5f; // 多久后开始下沉
        float sinkDuration = 2f; // 下沉持续时间

        // 等待一段时间后开始下沉
        yield return new WaitForSeconds(destroyDelay);

        Vector3 sinkTarget = enemy.transform.position - new Vector3(0, 1f, 0); // 下沉目标位置

        float elapsedTime = 0f;
        Vector3 initialPosition = enemy.transform.position;

        // 平滑下沉
        while (elapsedTime < sinkDuration)
        {
            enemy.transform.position = Vector3.Lerp(initialPosition, sinkTarget, elapsedTime / sinkDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 确保下沉到目标位置
        enemy.transform.position = sinkTarget;

        // 销毁尸体
        Destroy(enemy.gameObject);
    }
}
