using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BattleMove
{
    [Header("Properties")]
    [Space]
    public string moveName;
    public int movePower;
    public int moveCost;
    public AttackEffect fx;
}
