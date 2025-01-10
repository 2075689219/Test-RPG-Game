using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Combat System/Create new attack")]
public class AttackData : ScriptableObject
{
    [field: SerializeField] public string AnimName { get; private set; }
    [field: SerializeField] public float StartTime { get; private set; }//实际是百分比
    [field: SerializeField] public float EndTime { get; private set; }
}
