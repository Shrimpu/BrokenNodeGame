using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New ClearAfterTime", menuName = "ClearActions/ClearAfterTime")]
public class ClearAfterTimePowered : PowerAction
{
    public float timeRequired = 1f;
    float timeElapsed = 0;

    GameObject button = null;
    Coroutine timer;

    public override void Do(MonoBehaviour mono)
    {
        if (button == null)
            Count();
        else if (button.transform.position != new Vector3(0, 360, 0))
            Count();
        else
            timeElapsed = 0;
    }

    public override void Do(MonoBehaviour mono, float power)
    {
        if (button == null)
            Count();
        else if (button.transform.position != new Vector3(0, 360, 0))
            Count();
        else
            timeElapsed = 0;
    }

    void Count()
    {
        timeElapsed += Time.deltaTime;
        if (timeElapsed >= timeRequired)
        {
            if (button == null)
            {
                button = GameObject.FindGameObjectWithTag("NextLevelButton");
                button.transform.localPosition = new Vector3(0, 360, 0);
                timeElapsed = 0;
            }
            else if (button.transform.position != new Vector3(0, 360, 0))
            {
                button.transform.localPosition = new Vector3(0, 360, 0);
                timeElapsed = 0;
            }
        }
    }
}
