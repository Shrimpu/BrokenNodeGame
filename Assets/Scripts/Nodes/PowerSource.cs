using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerSource : PowerNode
{
    public float powerProduction = 1f;

    protected override void DistributePower()
    {
        powerOnHold += powerProduction;
        base.DistributePower();
    }
}
