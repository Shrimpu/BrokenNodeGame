using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WireHandler
{
    public delegate void AllwiresRemoved();
    public static AllwiresRemoved allwiresRemoved;
    public delegate void WireEdited(Wire wire);
    public static WireEdited wireRemoved;
    public static WireEdited wireAdded;
    public static WireEdited wireCompleted;

    public enum Drawing { OnHold, Drawing }
    public static Drawing drawingWire;

    static List<Wire> wires = new List<Wire>();

    public static bool StartWire(Node node)
    {
        wires.Add(new Wire());
        bool canAdd = wires[wires.Count - 1].AssignInput(node);
        if (canAdd)
        {
            drawingWire = Drawing.Drawing;
            wireAdded?.Invoke(wires[wires.Count - 1]);
        }
        else
        {
            wires.RemoveAt(wires.Count - 1);
        }
        return canAdd;
    }

    public static bool EndWire(Node node)
    {
        bool canAdd = wires[wires.Count - 1].AssignOutput(node);
        if (canAdd)
        {
            drawingWire = Drawing.OnHold;
            wireCompleted?.Invoke(wires[wires.Count - 1]);
        }
        return canAdd;
    }

    public static void CancelWire()
    {
        if (drawingWire == Drawing.Drawing)
        {
            drawingWire = Drawing.OnHold;
            wireRemoved?.Invoke(wires[wires.Count - 1]);
            wires[wires.Count - 1].RemoveConnections();
            wires.RemoveAt(wires.Count - 1);
        }
    }

    public static void RemoveWire(Wire wire)
    {
        if (wires.Contains(wire))
        {
            wireRemoved?.Invoke(wire);
            wire.RemoveConnections();
            wires.Remove(wire);
        }
    }

    static void DisconnectWire(Wire wire)
    {
        if (wires.Contains(wire))
        {
            wireRemoved?.Invoke(wire);
            wire.RemoveConnections();
        }
    }

    public static void Reset()
    {
        foreach (Wire wire in wires)
        {
            DisconnectWire(wire);
        }

        wires.Clear();
        allwiresRemoved?.Invoke();
        wireAdded = null; wireRemoved = null; wireCompleted = null;
    }
}
