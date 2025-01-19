using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Combat System/Create new attack")]
public class AttackData : ScriptableObject
{
    [field: SerializeField] public string AnimName { get; private set; }
    [field: SerializeField] public AttackHitBox HitBox { get; private set; }
    [field: SerializeField] public float StartTime { get; private set; }//实际是百分比
    [field: SerializeField] public float EndTime { get; private set; }
    [field: SerializeField] public bool moveToTarget { get; private set; }
    [field: SerializeField] public float distanceToStop { get; private set; } = 1.3f;
    [field: SerializeField] public float maxMovement { get; private set; } = 4f;
    [field: SerializeField] public float moveStartTime { get; private set; } = 0f;
    [field: SerializeField] public float moveEndTime { get; private set; } = 1f;
}
public enum AttackHitBox {weapon}//暂时只有weapon