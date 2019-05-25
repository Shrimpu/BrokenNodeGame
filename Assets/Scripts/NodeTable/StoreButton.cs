using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StoreButton : MonoBehaviour
{
    [HideInInspector]
    public NodeRestocker.StoreNode nodeData;
    public TMP_Text text;

    private void Start()
    {
        UpdateText();
    }

    public void SpawnNode()
    {
        if (nodeData.amount > 0)
        {
            GameObject n = Instantiate(nodeData.node, Vector3.zero, Quaternion.identity);
            nodeData.amount--;
            Node ns = n.GetComponent<Node>();
            ns.nodeDestroyed += RestockNode;
            UpdateText();
        }
    }

    public void RestockNode()
    {
        nodeData.amount++;
        UpdateText();
    }

    void UpdateText()
    {
        text.text = nodeData.node.name + "\n" + nodeData.amount;
    }
}
