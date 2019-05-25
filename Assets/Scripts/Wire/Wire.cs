using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wire
{
    public Node startNode;
    public Node endNode;

    public bool AssignInput(Node input)
    {
        startNode = input;
        bool available = startNode.CheckConnectOut();
        return available;
    }

    public bool AssignOutput(Node output)
    {
        endNode = output;
        bool available = endNode.ConnectIn(startNode);
        if (available && !endNode.Equals(startNode))
        {
            bool success = startNode.ConnectOut(endNode);
            if (!success)
            {
                endNode.DisconnectIn(startNode);
            }
            else
            {
                return true;
            }
        }
        else
        {
            endNode.DisconnectIn(startNode);
        }
        return false;
    }

    public void RemoveConnections()
    {
        if (startNode.connectionsOut.Contains(endNode))
        {
            startNode.Disconnect(endNode);
        }
    }
}
