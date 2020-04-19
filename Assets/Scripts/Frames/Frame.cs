using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Resource;

public class Frame : MonoBehaviour
{

    private Vector2 location = new Vector2(0, 0);

    public string prefixName = "Frame";

    public GameObject[] ValidObjects;
    public GameObject[] InvalidObjects;
    public int Mass = 10;

    //Each tick:
    /*
     * 1) Calculate Demand for all resources
     * 2) Supply resources with no dependency
     * 3) Supply resources from storage
     * 4) All frames take as many resources as they can
     * 5) Perform conversion up to the demand
     * 6) Store excess
     */

    public virtual void Demand(ref Dictionary<ResType, Resource> demands)
    {

    }

    public virtual void SupplyIndependent(ref Dictionary<ResType, Resource> demands, ref Dictionary<ResType, Resource> balance)
    {
        balance[ResType.Mass] += new Resource(ResType.Mass, Mass);
    }
    public virtual void SupplyFromStorage(ref Dictionary<ResType, Resource> demands, ref Dictionary<ResType, Resource> balance)
    {

    }

    public virtual void SupplyProduce(ref Dictionary<ResType, Resource> demands, ref Dictionary<ResType, Resource> balance)
    {

    }

    public virtual void Consume(ref Dictionary<ResType, Resource> balance)
    {

    }


    //public virtual void Produce(ref Dictionary<ResType, Resource> balance)
    //{

    //}

    public virtual void Store(ref Dictionary<ResType, Resource> balance)
    {

    }

    public virtual Vector2 Location
    {
        get => location; set
        {
            location = value;
            transform.localPosition = value;
            name = prefixName + ": " + location.ToString();
        }
    }

    private bool showError = false;

    public List<Vector2> GetBlockedLocations()
    {
        List<Vector2> locs = new List<Vector2>();
        foreach(Vector2 offset in BlockedOffsets)
        {
            locs.Add(Location + offset);
        }

        return locs;
    }

    public virtual List<Vector2> BlockedOffsets { get => blockedOffsets; }
    public Ship Ship { get; set; } = null;
    public bool ShowError
    {
        get => showError; set
        {
            showError = value;
            foreach (GameObject vld in ValidObjects)
            {
                vld.SetActive(!value);
            }
            foreach (GameObject vld in InvalidObjects)
            {
                vld.SetActive(value);
            }
        }
    }

    private List<Vector2> blockedOffsets = new List<Vector2>()
    {
        Vector2.zero,
    };

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
