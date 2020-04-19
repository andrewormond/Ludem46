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

    public int PowerAmount = 100;

    public int MaxHeight = 300;
    public int MinHeight = 100;

    public float MaxEff = 1f;
    public float MinEff = 0.5f;

    public override void SupplyIndependent(ref Dictionary<ResType, Resource> demands, ref Dictionary<ResType, Resource> balance)
    {
        base.SupplyIndependent(ref demands, ref balance);
        float modifier = 1f;
        if (transform.position.y < MinHeight)
        {
            modifier = MinEff;
        }
        else if (transform.position.y < MaxHeight)
        {
            modifier = MinEff + (MaxEff - MinEff) * ((transform.position.y - MinHeight) / (MaxHeight - MinHeight));
        }
        else
        {
            modifier = 1f;
        }
        balance[ResType.Power] += Mathf.RoundToInt(PowerAmount*modifier);
    }
}
