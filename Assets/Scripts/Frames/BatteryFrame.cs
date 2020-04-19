using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Resource;

public class BatteryFrame : Frame
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private bool started = false;
    public int BatterySize = 1000;

    public override void Supply(ref Dictionary<Resource.ResType, Resource> balance)
    {
        base.Supply(ref balance);
        balance[ResType.Power] = balance[ResType.Power].AddStorage(BatterySize);
        if (!started)
        {
            balance[ResType.Power] += BatterySize;
            started = true;
        }
    }
}
