using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PowerAction : Action
{
    public abstract void Do(MonoBehaviour mono, float power);
}
