using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Resource
{

    public enum ResType
    {
        Mass=0,
        Power,
        Thrust,
        NumberResources
    }

    public ResType type;
    public int amount;

    public Resource(ResType type, int amount)
    {
        this.type = type;
        this.amount = amount;
    }

    public static Dictionary<ResType, int> GetInitializedDictionary()
    {
        Dictionary<ResType, int> resources = new Dictionary<ResType, int>();
        for (int i = 0; i < (int)ResType.NumberResources; i++)
        {
            resources[(ResType)i] = 0;
        }
        return resources;
    }
}
