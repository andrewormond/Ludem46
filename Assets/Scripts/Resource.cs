using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Resource;

public struct Resource : IEquatable<Resource>
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

    public Resource(ResType type, int amount)
    {
        this.type = type;
        this.amount = amount;
    }

    public Resource SetAmount(int nvalue)
    {
        return new Resource(type, nvalue);
    }
    public Resource AddAmount(int noffset)
    {
        return new Resource(type, amount + noffset);
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
        return amount.ToString();//string.Format("{0}: {1}", type.ToString(), amount);
    }

    public override bool Equals(object obj)
    {
        return obj is Resource resource && Equals(resource);
    }

    public bool Equals(Resource other)
    {
        return type == other.type &&
               amount == other.amount;
    }

    public override int GetHashCode()
    {
        int hashCode = -107281250;
        hashCode = hashCode * -1521134295 + type.GetHashCode();
        hashCode = hashCode * -1521134295 + amount.GetHashCode();
        return hashCode;
    }

    public static readonly Resource Invalid = new Resource(ResType.INVALID, 0);

    public static Resource operator +(Resource a) => a;
    public static Resource operator -(Resource a) => new Resource(a.type, -a.amount);

    public static Resource operator +(Resource a, Resource b)
    {
        if (a.type != b.type) throw new System.Exception("Resources must be the same type. Not " + a.type + " and " + b.type);

        return new Resource(a.type, a.amount + b.amount);
    }

    public static Resource operator +(Resource a, int amount)
    {
        return new Resource(a.type, a.amount + amount);
    }
    public static Resource operator -(Resource a, int amount)
    {
        return new Resource(a.type, a.amount - amount);
    }

    public static Resource operator -(Resource a, Resource b)
        => a + (-b);

    public static bool operator <(Resource a, Resource b)
    {
        if (a.type != b.type) throw new System.Exception("Resources must be the same type. Not " + a.type + " and " + b.type);

        return (a.amount < b.amount);
    }
    public static bool operator >(Resource a, Resource b)
    {
        if (a.type != b.type) throw new System.Exception("Resources must be the same type. Not " + a.type + " and " + b.type);

        return (a.amount > b.amount);
    }

    public static bool operator ==(Resource a, Resource b)
    {
        if (a.type != b.type) throw new System.Exception("Resources must be the same type. Not " + a.type + " and " + b.type);

        return (a.amount == b.amount);
    }

    public static bool operator !=(Resource a, Resource b)
    {
        if (a.type != b.type) throw new System.Exception("Resources must be the same type. Not " + a.type + " and " + b.type);

        return (a.amount != b.amount);
    }

    
}