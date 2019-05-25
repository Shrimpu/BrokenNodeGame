using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WireMove : MonoBehaviour, IPointerClickHandler
{
    public delegate void RemoveThis(Wire wire);
    public RemoveThis remove;
    public GameObject directionIndicatorPrefab;
    private GameObject directionIndicator;

    public Wire wire;
    public LineRenderer lr;
    private PolygonCollider2D col;

    Node startNode;
    Node endNode;

    Vector2[] points = new Vector2[4];

    public void StartNodeIsSet(Node node)
    {
        col = GetComponent<PolygonCollider2D>();
        startNode = node;
        startNode.isMoving += MoveStart;
        startNode.disconnectInfo += CheckDisconnection;
    }

    public void EndNodeIsSet(Node node)
    {
        col.SetPath(0, points);
        endNode = node;
        endNode.isMoving += MoveEnd;
        endNode.disconnectInfo += CheckDisconnection;
        directionIndicator = Instantiate(directionIndicatorPrefab);

        //safety setup because it fails ones every 10 times
        MoveStart(startNode.transform.position);
        MoveEnd(endNode.transform.position);
    }

    void MoveStart(Vector3 pos)
    {
        float yOffset = lr.startWidth / 2f + WorldData.wireSkin;
        points[0] = new Vector2(pos.x, pos.y - yOffset);
        points[1] = new Vector2(pos.x, pos.y + yOffset);
        // change the collider vertexes to match the line
        col.SetPath(0, points);
        lr.SetPosition(0, pos);
    }

    void MoveEnd(Vector3 pos)
    {
        float yOffset = lr.startWidth / 2f + WorldData.wireSkin;
        points[2] = new Vector2(pos.x, pos.y + yOffset);
        points[3] = new Vector2(pos.x, pos.y - yOffset);
        // change the collider vertexes to match the line
        col.SetPath(0, points);
        lr.SetPosition(1, pos);
    }

    void CheckDisconnection(Node node)
    {
        if (node == startNode || node == endNode)
        {
            WireHandler.RemoveWire(wire);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (WireHandler.drawingWire == WireHandler.Drawing.OnHold)
            StartCoroutine(CheckClick());
    }

    IEnumerator CheckClick()
    {

        while (true)
        {
            if (WireHandler.drawingWire == WireHandler.Drawing.Drawing)
            {
                break;
            }
            if (Input.GetKey(KeyCode.Backspace) || Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Delete))
            {
                WireHandler.RemoveWire(wire);
            }
            else if (Input.anyKeyDown)
            {
                break;
            }
            yield return null;
        }
    }

    public void Remove()
    {
        if (directionIndicator != null)
            Destroy(directionIndicator);
        // remove the delegate connections to prevent null references
        if (startNode != null)
        {
            startNode.isMoving -= MoveStart;
            startNode.disconnectInfo -= CheckDisconnection;
        }
        if (endNode != null)
        {
            endNode.isMoving -= MoveEnd;
            startNode.disconnectInfo -= CheckDisconnection;
        }
    }

    private void FixedUpdate()
    {
        if (endNode != null && startNode != null && directionIndicator != null)
        {
            directionIndicator.transform.position = Vector3.Lerp(startNode.transform.position, endNode.transform.position, (Time.time * 0.8f) % 1);
        }
    }
}
