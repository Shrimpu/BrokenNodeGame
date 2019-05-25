using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class DisplayWires : MonoBehaviour
{
    public AudioClip electricitySound;
    public Material wireMat;
    public GameObject directionIndicator;

    List<WireComponents> wires = new List<WireComponents>();
    Camera cam;
    AudioSource au;

    void Awake()
    {
        WireHandler.wireAdded += CreateWire;
        WireHandler.wireRemoved += RemoveWire;
        WireHandler.wireCompleted += CompleteWire;

        cam = Camera.main;
        au = GetComponent<AudioSource>();
    }

    void CreateWire(Wire wire)
    {
        if (!au.isPlaying)
        {
            au.clip = electricitySound;
            au.loop = true;
            au.Play(0);
        }

        wires.Add(new WireComponents()
        {
            gObject = new GameObject(),
            wireData = wire
        });

        // setting up the wire
        wires[wires.Count - 1].line = wires[wires.Count - 1].gObject.AddComponent<LineRenderer>();
        wires[wires.Count - 1].gObject.AddComponent<WireMove>().lr = wires[wires.Count - 1].line;
        WireMove wireMove = wires[wires.Count - 1].gObject.GetComponent<WireMove>();
        wireMove.wire = wire;

        PolygonCollider2D col = wires[wires.Count - 1].gObject.AddComponent<PolygonCollider2D>();

        wires[wires.Count - 1].line.SetPosition(0, wire.startNode.transform.position);
        wires[wires.Count - 1].line.positionCount = 2;
        wires[wires.Count - 1].line.startWidth = 0.1f;
        wires[wires.Count - 1].line.alignment = LineAlignment.TransformZ;
        wires[wires.Count - 1].line.materials[0] = wireMat;
        wires[wires.Count - 1].gObject.name = "Wire " + (wires.Count - 1);
        wireMove.StartNodeIsSet(wires[wires.Count - 1].wireData.startNode);
        wireMove.remove += RemoveWire;
        wireMove.directionIndicatorPrefab = directionIndicator;
        // setup complet
        if (gameObject != null)
            StartCoroutine(DrawWire(wires[wires.Count - 1].line));
    }

    void CompleteWire(Wire wire)
    {
        // The wire will always be last or so i think. better safe than sorry
        for (int i = wires.Count - 1; i > -1; i--)
        {
            if (wires[i].wireData == wire)
            {
                wires[i].line.SetPosition(1, wires[i].wireData.endNode.transform.position);
                WireMove wireMove = wires[wires.Count - 1].gObject.GetComponent<WireMove>();
                wireMove.EndNodeIsSet(wires[wires.Count - 1].wireData.endNode);
                break;
            }
        }
    }

    void RemoveWire(Wire wire)
    {
        for (int i = 0; i < wires.Count; i++)
        {
            if (wires[i].wireData.Equals(wire))
            {
                wires[i].gObject.GetComponent<WireMove>().Remove();
                Destroy(wires[i].gObject);
                // I think this takes less processing than the ordinary remove
                wires.RemoveAt(i);

                if (wires.Count <= 0)
                {
                    au.Stop();
                }

                break;
            }
        }
    }

    IEnumerator DrawWire(LineRenderer lr)
    {
        while (WireHandler.drawingWire == WireHandler.Drawing.Drawing)
        {
            lr.SetPosition(1, cam.ScreenToWorldPoint(Input.mousePosition));
            yield return null;
            if (Input.GetKeyDown(KeyCode.Backspace) || Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Delete))
            {
                RemoveWire(wires[wires.Count - 1].wireData);
                WireHandler.CancelWire();
            }
        }
    }

    class WireComponents
    {
        public GameObject gObject;
        public LineRenderer line;
        public Wire wireData;
    }
}
