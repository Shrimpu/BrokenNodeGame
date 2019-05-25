using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PowerMeter : PowerNode
{
    public TMP_Text text;

    protected override void DistributePower()
    {
        powerOnHold = Mathf.Clamp(powerOnHold, 0, stats.capacity);
        text.text = powerOnHold.ToString();
        base.DistributePower();
    }
}
