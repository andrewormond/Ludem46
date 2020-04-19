using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrusterFrame : Frame
{
    //thrust/power
    public float ConversionEfficiency = 0.8f;
    public float MaxThrust = 25;

    public override void Supply(ref Dictionary<Resource.ResType, int> balance)
    {
        base.Supply(ref balance);
        int powerCost = Mathf.RoundToInt(MaxThrust / ConversionEfficiency);
        if(balance[Resource.ResType.Power] >= powerCost)
        {
            balance[Resource.ResType.Power] -= powerCost;
            balance[Resource.ResType.Thrust] += powerCost;
        }
    }
}
