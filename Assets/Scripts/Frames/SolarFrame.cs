using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Resource;

public class SolarFrame : Frame
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static int PowerAmount = 100;

    public static int MaxHeight = 300;
    public static int MinHeight = 100;

    public static float MaxEff = 1f;
    public static float MinEff = 0.5f;

    public static float CalculateEfficiency(float altitude)
    {
        float modifier = 1f;
        if (altitude < MinHeight)
        {
            modifier = MinEff;
        }
        else if (altitude < MaxHeight)
        {
            modifier = MinEff + (MaxEff - MinEff) * ((altitude - MinHeight) / (MaxHeight - MinHeight));
        }
        else
        {
            modifier = 1f;
        }
        return modifier;
    }

    public override void SupplyIndependent(ref Dictionary<ResType, Resource> demands, ref Dictionary<ResType, Resource> balance)
    {
        base.SupplyIndependent(ref demands, ref balance);
        
        balance[ResType.Power] += Mathf.RoundToInt(PowerAmount* CalculateEfficiency(transform.position.y));
    }
}
