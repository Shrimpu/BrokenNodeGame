using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldTimer : MonoBehaviour
{
    public delegate void Tick();
    public static Tick tick;

    private void Awake()
    {
        StartCoroutine(CountTicks());
    }

    IEnumerator CountTicks()
    {
        while (true)
        {
            yield return new WaitForSeconds(WorldData.TickSpeed);
            tick?.Invoke();
        }
    }
}
