using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrusterFrame : Frame
{
    //thrust/power
    public float ConversionEfficiency = 0.8f;
    public float MaxThrust = 25;


    private int thrustBalance = 0;
    private int powerBalance = 0;

    public override void Consume(ref Dictionary<Resource.ResType, Resource> balance)
    {
        base.Consume(ref balance);
        int powerCost = Mathf.RoundToInt(MaxThrust / ConversionEfficiency);
        if (balance[Resource.ResType.Power].amount > powerCost)
        {
            powerBalance += powerCost;
            balance[Resource.ResType.Power] -= powerCost;
        }
    }

    public override void Produce(ref Dictionary<Resource.ResType, Resource> balance)
    {
        base.Produce(ref balance);
        thrustBalance += Mathf.RoundToInt(powerBalance * ConversionEfficiency);
        powerBalance = 0;
    }

    public override void Supply(ref Dictionary<Resource.ResType, Resource> balance)
    {
        base.Supply(ref balance);
        balance[Resource.ResType.Thrust] += thrustBalance;
        thrustBalance = 0;
    }
}
