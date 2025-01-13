using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DeadState : State<EnemyController>
{
    EnemyController enemy;
    public override void Enter(EnemyController owner)
    {
        enemy = owner; 
        enemy.visionSensor.gameObject.SetActive(false);
        EnemyManager.instance.RemoveEnemyInRange(enemy);
    }
}
