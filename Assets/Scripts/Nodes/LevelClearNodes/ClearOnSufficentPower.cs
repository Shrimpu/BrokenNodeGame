using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New ClearOnPowerReached", menuName = "ClearActions/ClearOnPowerReached")]
public class ClearOnSufficentPower : PowerAction
{
    GameObject button = null;
    public float requiredPower;

    public override void Do(MonoBehaviour mono, float power)
    {
        if (requiredPower == power)
        {
            if (button == null || button.transform.position != new Vector3(0, 360, 0))
            {
                button = GameObject.FindGameObjectWithTag("NextLevelButton");
                button.transform.localPosition = new Vector3(0, 360, 0);
            }
        }
    }

    public override void Do(MonoBehaviour mono)
    {
        if (button == null)
        {

            button = GameObject.FindGameObjectWithTag("NextLevelButton");
            button.transform.localPosition = new Vector3(0, 360, 0);
        }
    }
}
