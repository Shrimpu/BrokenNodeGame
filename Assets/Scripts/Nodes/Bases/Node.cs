using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Node : MonoBehaviour, IConnect, IPointerDownHandler, IPointerUpHandler
{
    public delegate void VectorDelegate(Vector3 vector);
    public VectorDelegate isMoving;
    public delegate void NodeChanged(Node node);
    public NodeChanged disconnectInfo;
    public delegate void NodeDestroyed();
    public NodeDestroyed nodeDestroyed;

    public bool permanent = false;

    public Stats stats;

    public List<Node> connectionsOut = new List<Node>();
    public List<Node> connectionsIn = new List<Node>();

    public virtual bool CanConnectIn { get { return connectionsIn.Count <= stats.maxConnectionsIn; } set => CanConnectIn = value; }
    public virtual bool CanConnectOut { get { return connectionsOut.Count <= stats.maxConnectionsOut; } set => CanConnectOut = value; }

    Coroutine coroutineMove;

    public virtual bool ConnectIn(Node node)
    {
        if (CanConnectIn && !connectionsIn.Contains(node))
        {
            connectionsIn.Add(node);
            return true;
        }
        return false;
    }

    public virtual bool ConnectOut(Node node)
    {
        if (CanConnectOut && !connectionsIn.Contains(node))
        {
            node.ConnectIn(this);
            connectionsOut.Add(node);
            return true;
        }
        return false;
    }

    public virtual bool CheckConnectOut()
    {
        if (CanConnectOut)
        {
            return true;
        }
        return false;
    }

    public virtual void Disconnect(Node node)
    {
        if (connectionsIn.Contains(node))
        {
            node.DisconnectIn(this);
            connectionsOut.Remove(node);
            disconnectInfo?.Invoke(node);
        }
    }

    public virtual void DisconnectIn(Node node)
    {
        if (connectionsIn.Count > 0 && connectionsIn.Contains(node))
        {
            connectionsIn.Remove(node);
        }
    }

    protected virtual void RemoveNode()
    {
        if (!permanent)
        {
            for (int i = 0; i < connectionsOut.Count; i++)
            {
                Disconnect(connectionsOut[i]);
            }
            for (int i = 0; i < connectionsIn.Count; i++)
            {
                connectionsIn[i].Disconnect(this);
            }
            nodeDestroyed?.Invoke();
            Destroy(gameObject);
        }
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right && WireHandler.drawingWire == WireHandler.Drawing.OnHold)
        {
            WireHandler.StartWire(this);
        }
        else if (WireHandler.drawingWire == WireHandler.Drawing.Drawing)
        {
            bool s = WireHandler.EndWire(this);
        }
        coroutineMove = StartCoroutine(Move());
    }

    public virtual void OnPointerUp(PointerEventData eventData)
    {
        if (coroutineMove != null)
            StopCoroutine(coroutineMove);
        if (eventData.button == PointerEventData.InputButton.Left && WireHandler.drawingWire == WireHandler.Drawing.OnHold)
        {
            StartCoroutine(CheckClick());
        }
    }

    IEnumerator Move()
    {
        Camera cam = Camera.main;
        Vector3 offset = transform.position - cam.ScreenToWorldPoint(Input.mousePosition); // the offset is used so it doesn't teleport to the enter of your mouse
        while (true)
        {
            yield return null;
            transform.position = cam.ScreenToWorldPoint(Input.mousePosition) + offset;
            isMoving?.Invoke(transform.position);
        }
    }

    IEnumerator CheckClick()
    {
        while (true)
        {
            if (Input.GetKey(KeyCode.Backspace) || Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Delete))
            {
                RemoveNode();
                break;
            }
            else if (Input.anyKeyDown)
            {
                break;
            }
            yield return null;
        }
    }
}
