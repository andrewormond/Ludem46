using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public override void Supply(ref Dictionary<Resource.ResType, Resource> balance)
    {
        base.Supply(ref balance);
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
        balance[Resource.ResType.Power] += Mathf.RoundToInt(PowerAmount*modifier);
    }
}
