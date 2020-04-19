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

    public override List<Resource> GetResources()
    {
        List < Resource > list =  base.GetResources();
        list.Add(new Resource(Resource.ResType.Power, PowerAmount));
        return list;
    }
}
