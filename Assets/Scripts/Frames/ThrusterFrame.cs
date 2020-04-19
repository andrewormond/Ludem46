using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Resource;

public class ThrusterFrame : Frame
{
    //thrust/power
    public float ConversionEfficiency = 0.8f;
    public int MaxThrust = 100;


    private int powerBalance = 0;
    private int powerStorage = 0;



    private void Awake()
    {
        powerStorage = Mathf.RoundToInt(MaxThrust / ConversionEfficiency);
        powerBalance = powerStorage;
    }

    //Ask to refill internal power storage
    public override void Demand(ref Dictionary<Resource.ResType, Resource> demands)
    {
        base.Demand(ref demands);
        demands[ResType.Power] += powerStorage - powerBalance;
    }

    //refill internal power storage
    public override void Consume(ref Dictionary<ResType, Resource> balance)
    {
        base.Consume(ref balance);

        if (balance[ResType.Power].amount > 0 && powerBalance < powerStorage)
        {
            int sup = balance[ResType.Power].amount;
            if (sup > powerStorage - powerBalance)
            {
                sup = powerStorage - powerBalance;
            }
            powerBalance += sup;
            balance[ResType.Power] -= sup;
        }
    }

    //convert power to thrust
    public override void SupplyProduce(ref Dictionary<ResType, Resource> demands, ref Dictionary<ResType, Resource> balance)
    {
        base.SupplyProduce(ref demands, ref balance);
        if (balance[ResType.Thrust] < demands[ResType.Thrust])
        {
            //The amount of thrust the ship is asking for
            int demand = (demands[ResType.Thrust] - balance[ResType.Thrust]).amount;

            //The amount of thrust this frame can create
            int capacity = Mathf.RoundToInt(powerBalance * ConversionEfficiency);
            if (demand > capacity)
            {
                demand = capacity;
            }

            balance[ResType.Thrust] += demand;
            powerBalance -= Mathf.RoundToInt(demand / ConversionEfficiency);
        }
    }
}
