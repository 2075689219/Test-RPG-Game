using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EnemyController))]
public class EnemyControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // 获取 EnemyController 组件
        EnemyController enemyController = (EnemyController)target;

        // 检查 stateMachine 是否为 null
        if (enemyController.stateMachine != null)
        {
            // 显示当前的 EnemyState
            EditorGUILayout.LabelField("Current State", enemyController.stateMachine.CurrentState.GetType().Name);
        }
        else
        {
            EditorGUILayout.HelpBox("StateMachine is not initialized!", MessageType.Warning);
        }

        // 保持默认 Inspector 显示其他字段
        DrawDefaultInspector();
    }
}
