using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Stats", menuName = "Stats")]
public class Stats : ScriptableObject
{
    public float capacity = 0;
    public float requiredPower = 0;
    public float maxCurrent = 0;

    public int maxConnectionsIn = 1;
    public int maxConnectionsOut = 1;
}
