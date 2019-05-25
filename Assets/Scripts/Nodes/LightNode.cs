using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Material))]
public class LightNode : PowerNode
{
    bool active;
    bool broken = false;

    SpriteRenderer sr;

    protected override void Start()
    {
        base.Start();
        sr = GetComponent<SpriteRenderer>();
        sr.color = Color.gray;
    }

    public override void UsePower()
    {
        if (canUsePower && !broken)
        {
            if (powerOnHold <= stats.capacity)
            {
                if (powerOnHold >= stats.requiredPower)
                {
                    On();
                }
                else
                {
                    Off();
                }
            }
            else
            {
                Break();
            }
        }
    }

    void On()
    {
        if (!active)
        {
            sr.color = Color.yellow;
        }
        action.Do(this, powerOnHold);
        powerOnHold -= stats.requiredPower;
        active = true;
    }

    void Off()
    {
        if (active)
        {
            sr.color = Color.gray;
        }
        active = false;
    }

    void Break()
    {
        broken = true;
        active = false;

        sr.color = Color.black;
    }
}
