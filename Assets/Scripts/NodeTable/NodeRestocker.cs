using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeRestocker : MonoBehaviour
{
    public GameObject ButtonTemplate;
    public StoreNode[] nodesAvailable;

    [System.Serializable]
    public class StoreNode
    {
        public GameObject node;
        public int amount;
    }

    void Awake()
    {
        foreach (StoreNode node in nodesAvailable)
        {
            GameObject b = Instantiate(ButtonTemplate, transform);
            StoreButton s = b.GetComponent<StoreButton>();
            s.nodeData = node;
        }
    }
}
