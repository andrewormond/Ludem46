using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Resource;

public struct Resource
{

    public enum ResType
    {
        Mass=0,
        Power,
        Thrust,
        NumberResources,
        INVALID
    }

    public ResType type;
    public int amount;
    public int storage;

    public Resource(ResType type, int amount, int storage)
    {
        this.type = type;
        this.amount = amount;
        this.storage = storage;
    }

    public Resource(ResType type, int amount) : this(type, amount, amount) { }

    public Resource SetStorage(int nvalue)
    {
        return new Resource(type, amount, nvalue);
    }
    public Resource AddStorage(int noffset)
    {
        return new Resource(type, amount, storage + noffset);
    }
    public Resource SetAmount(int nvalue)
    {
        return new Resource(type, nvalue, storage);
    }
    public Resource AddAmount(int noffset)
    {
        return new Resource(type, amount + noffset, storage);
    }

    public static Dictionary<ResType, Resource> GetInitializedDictionary()
    {
        Dictionary<ResType, Resource> resources = new Dictionary<ResType, Resource>();
        for (int i = 0; i < (int)ResType.NumberResources; i++)
        {
            resources[(ResType)i] = new Resource((ResType) i, 0);
        }
        return resources;
    }

    public override string ToString()
    {
        return string.Format("{0}/{1}", amount, storage);
    }


    public static readonly Resource Invalid = new Resource(ResType.INVALID, 0);

    public static Resource operator +(Resource a) => a;
    public static Resource operator -(Resource a) => new Resource(a.type, -a.amount, a.storage);

    public static Resource operator +(Resource a, Resource b)
    {
        if (a.type != b.type) throw new System.Exception("Resources must be the same type. Not " + a.type + " and " + b.type);

        return new Resource(a.type, a.amount + b.amount, a.storage + b.storage);
    }

    public static Resource operator +(Resource a, int amount)
    {
        return new Resource(a.type, a.amount + amount, a.storage);
    }
    public static Resource operator -(Resource a, int amount)
    {
        return new Resource(a.type, a.amount - amount, a.storage);
    }

    public static Resource operator -(Resource a, Resource b)
        => a + (-b);

}