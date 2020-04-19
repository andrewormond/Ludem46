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
    public int Storage;
    private void Awake()
    {
        Storage = BatterySize;
    }

    public override void SupplyFromStorage(ref Dictionary<ResType, Resource> demands, ref Dictionary<ResType, Resource> balance)
    {
        base.SupplyFromStorage(ref demands, ref balance);

        if(balance[ResType.Power] < demands[ResType.Power])
        {
            int demand = (demands[ResType.Power] - balance[ResType.Power]).amount;

            if(demand > Storage)
            {
                demand = Storage;
            }

            //Console.Log("Battery", string.Format("Demand: {0}, Supplying: {1}", demands[ResType.Power] - balance[ResType.Power], demand));

            balance[ResType.Power] += demand;
            Storage -= demand;
        }
        else
        {
            //Console.Log("Battery", string.Format("Balance: {0} > {1}", balance[ResType.Power], demands[ResType.Power]));
        }
    }

    public override void Store(ref Dictionary<ResType, Resource> balance)
    {
        base.Store(ref balance);
        if(balance[ResType.Power].amount > 0 && Storage < BatterySize)
        {
            int sup = balance[ResType.Power].amount;
            if(sup > BatterySize - Storage)
            {
                sup = BatterySize - Storage;
            }
            Storage += sup;
            balance[ResType.Power] -= sup;
        }
    }
}
