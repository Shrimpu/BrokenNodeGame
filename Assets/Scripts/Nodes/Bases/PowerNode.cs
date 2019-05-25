using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerNode : Node, IUsePower
{
    protected float powerOnHold = 0;
    public bool canUsePower = false;
    public PowerAction action;

    protected Dictionary<Node, IUsePower> nodeDictionary = new Dictionary<Node, IUsePower>(); // emergency fix. (changed a thing and need this)

    protected virtual void Start()
    {
        WorldTimer.tick += DistributePower;
        ChangeScenes.loadingAnyScene += RemoveNode;
    }

    public virtual void GetPower(float power)
    {
        powerOnHold += power;
    }

    public virtual void UsePower()
    {
        if (canUsePower && powerOnHold >= stats.requiredPower)
        {
            if (action != null)
                action.Do(this, powerOnHold);
            powerOnHold -= stats.requiredPower;
        }
    }

    public override bool ConnectOut(Node item)
    {
        if (CanConnectOut && !connectionsOut.Contains(item) && item.GetComponent<IUsePower>() != null)
        {
            item.ConnectIn(this);
            nodeDictionary.Add(item, item.GetComponent<IUsePower>());
            connectionsOut.Add(item);
            return true;
        }
        return false;
    }

    public override void Disconnect(Node item)
    {
        if (connectionsOut.Contains(item))
        {
            IUsePower iUsePower;
            nodeDictionary.TryGetValue(item, out iUsePower);
            if (iUsePower != null)
                nodeDictionary.Remove(item);
            connectionsOut.Remove(item);
            item.DisconnectIn(this);
            disconnectInfo?.Invoke(item);
        }
    }

    protected override void RemoveNode()
    {
        WorldTimer.tick -= DistributePower;
        ChangeScenes.loadingAnyScene -= RemoveNode;
        base.RemoveNode();
    }

    protected virtual void DistributePower()
    {
        UsePower();
        float powerOutput = 0;
        if (powerOnHold > stats.capacity)
        {
            powerOnHold = stats.capacity;
        }

        if (powerOnHold > stats.maxCurrent)
        {
            powerOutput = stats.maxCurrent;
        }
        else
        {
            powerOutput = powerOnHold;
        }

        foreach (var link in connectionsOut)
        {
            IUsePower linkInterface;
            nodeDictionary.TryGetValue(link, out linkInterface);
            linkInterface.GetPower(powerOutput / connectionsOut.Count);
        }
        powerOnHold -= powerOutput;
    }
}
