using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static Resource;

public class Ship : MonoBehaviour
{
    public GameObject EmptyFramePrefab;
    public GameObject GeneratorFramePrefab;
    public GameObject PowerFramePrefab;
    public GameObject LiftFramePrefab;
    public GameObject BatteryFramePrefab;

    public Dictionary<Vector2, Frame> frameGrid = new Dictionary<Vector2, Frame>();
    public List<Frame> allFrames = new List<Frame>();

    public int BoundTop = 2;
    public int BoundBot = -2;
    public int BoundLeft = -3;
    public int BoundRight = 3;
    public GameObject BoundaryObject;
    public Slider AltitudeSlider;
    public Slider ThrottleSlider;
    public Slider PowerSlider;
    public Slider SolarSlider;


    public int BoundWidth
    {
        get
        {
            return 1 + BoundRight - BoundLeft;
        }
    }
    public int BoundHeight
    {
        get
        {
            return 1 + BoundTop - BoundBot;
        }
    }

    public void AddFrame(GameObject prefab, Vector2 location)
    {
        GameObject obj = Instantiate(prefab);
        Frame frame = obj.GetComponent<Frame>();
        frame.preview = false;
        frame.transform.SetParent(transform);
        frame.Location = location;
        foreach(Vector2 loc in frame.GetBlockedLocations())
        {
            frameGrid.Add(loc, frame);
        }
        allFrames.Add(frame);
        frame.ShowError = false;
    }

    public void RemoveFrame(Frame frame)
    {
        foreach(Vector2 key in frameGrid.Keys)
        {
            if(frameGrid[key] == frame)
            {
                frameGrid.Remove(key);
            }
        }
        allFrames.Remove(frame);
        Destroy(frame.gameObject);
    }


    private void UpdateBoundaries()
    {
        BoundaryObject.transform.localScale = new Vector3(BoundWidth, BoundHeight, 0.25f);
        BoundaryObject.transform.localPosition = new Vector3(BoundLeft + (BoundWidth - 1) /2f, BoundBot + (BoundHeight - 1)/2f);
    }

    private void Awake()
    {
        AddFrame(GeneratorFramePrefab, new Vector2(0, 0));
        AddFrame(PowerFramePrefab, new Vector2(1, 0));
        AddFrame(BatteryFramePrefab, new Vector2(2, 0));
        AddFrame(LiftFramePrefab, new Vector2(-1, 0));
        AddFrame(PowerFramePrefab, new Vector2(-2, 0));
        UpdateBoundaries();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public bool isOutOfBounds(Vector2 location)
    {
        return (location.x < BoundLeft) || (location.x > BoundRight) || (location.y < BoundBot) || (location.y > BoundTop);
    }

    public bool canPlaceFrameHere(Vector2 here, Frame frame)
    {
        foreach (Vector2 offset in frame.BlockedOffsets)
        {
            if(frameGrid.ContainsKey(offset + here) || isOutOfBounds(offset + here))
            {
                return false;
            }
        }

        return true;
    }

    //public Dictionary<ResType, Resource> resources = GetInitializedDictionary();

    //public void LimitStorage()
    //{
    //    for (int i = 0; i < (int)ResType.NumberResources; i++)
    //    {
    //        ResType type = (ResType)i;
    //        int namount = resources[type].amount;
    //        if (namount > resources[type].storage) {
    //            namount = resources[type].storage;
    //            Debug.Log(string.Format("{0} limited from {1} to {2}", type, resources[type].amount, namount));
    //        }
    //        resources[type] = new Resource(type, namount, 0);
    //    }
    //}

    //public Dictionary<ResType, Resource> CalculateTotalResourceBalance()
    //{
    //    LimitStorage();
    //    foreach (Frame frame in allFrames)
    //    {
    //        frame.Supply(ref resources);
    //    }
    //    foreach (Frame frame in allFrames)
    //    {
    //        frame.Consume(ref resources);
    //    }
    //    foreach (Frame frame in allFrames)
    //    {
    //        frame.Produce(ref resources);
    //    }
    //    return resources;
    //}



    // Update is called once per frame
    void Update()
    {
        if(AltitudeSlider != null)
        {
            AltitudeSlider.value = transform.position.y;
        }

        if(SolarSlider != null)
        {
            SolarSlider.maxValue = 1;
            SolarSlider.minValue = 0;
            SolarSlider.value = SolarFrame.CalculateEfficiency(transform.position.y);
        }
    }

    public float throttle = 0.5f;
    public float TargetAltitude = 35;

    public float P = 0.05f;
    public float I = 0.00005f;
    public float D = 0.005f;

    public PID AltitudeCntrl = new PID();
    private float AltitudeController(float setPoint, float processVariable)
    {
        AltitudeCntrl.P = P;
        AltitudeCntrl.I = I;
        AltitudeCntrl.D = D;
        return AltitudeCntrl.Process(setPoint, processVariable, Time.fixedDeltaTime);
    }

    private int GetMaxThrust()
    {
        int sum = 0;
        foreach (Frame frame in allFrames)
        {
            if (frame is ThrusterFrame thruster)
            {
                sum += thruster.MaxThrust;
            }
        }
        return sum;
    }

    private Tuple<int, int> GetPowerStorage()
    {
        int balance = 0;
        int max = 0;
        foreach (Frame frame in allFrames)
        {
            if (frame is BatteryFrame battery)
            {
                balance += battery.Storage;
                max += battery.BatterySize;
            }
        }
        PowerSlider.maxValue = max;
        PowerSlider.minValue = 0;
        PowerSlider.value = balance;
        return new Tuple<int, int>(balance, max);
    }

    private int GetThrustDemand()
    {
        throttle = AltitudeController(TargetAltitude, transform.position.y);
        if (throttle > 1f) throttle = 1f;
        if (throttle < 0f) throttle = 0f;
        ThrottleSlider.value = throttle;
        Console.Log("Throttle", throttle);
        int maxThrust = GetMaxThrust();
        Console.Log("Max Thrust", maxThrust);
        int thrust = Mathf.RoundToInt( throttle * maxThrust);
        Console.Log("Thrust Demand", thrust);
        return thrust;
    }

    private void FixedUpdate()
    {

        //1) Calculate Demand for all resources
        Dictionary<ResType, Resource> demands = GetInitializedDictionary();
        demands[ResType.Thrust] += GetThrustDemand();
        foreach (Frame frame in allFrames)
        {
            frame.Demand(ref demands);
        }

        // 2) Supply resources with no dependency
        Dictionary<ResType, Resource> balance = GetInitializedDictionary();
        foreach (Frame frame in allFrames)
        {
            frame.SupplyIndependent(ref demands, ref balance);
        }

        // 3) Supply resources from storage
        foreach (Frame frame in allFrames)
        {
            frame.SupplyFromStorage(ref demands, ref balance);
        }

        // 4) All frames take as many resources as they can
        foreach (Frame frame in allFrames)
        {
            frame.Consume(ref balance);
        }

        // 5) Perform conversion up to the demand
        foreach (Frame frame in allFrames)
        {
            frame.SupplyProduce(ref demands, ref balance);
        }

        // 6) Store excess
        foreach (Frame frame in allFrames)
        {
            frame.Store(ref balance);
        }

        foreach (ResType resType in balance.Keys)
        {
            Console.Log("Delta-"+resType.ToString(), (balance[resType] - demands[resType]).ToString()) ;
        }


        Rigidbody rigidbody = GetComponent<Rigidbody>();
        rigidbody.mass = balance[ResType.Mass].amount;

        Console.Log("Mass", balance[ResType.Mass]);
        rigidbody.AddForce(new Vector3(0, -9.8f * rigidbody.mass, 0));


        Console.Log("Produced Thrust", balance[ResType.Thrust]);
        rigidbody.AddForce(new Vector3(0, balance[ResType.Thrust].amount * 9.8f, 0));

        Console.Log("Altitude", transform.position.y + " m");

        (int powerBalance, int powerMax) = GetPowerStorage();
        Console.Log(ResType.Power.ToString() + " storage", string.Format("{0}/{1}", powerBalance, powerMax));
    }


    public void SetAltitude(float nalt)
    {
        TargetAltitude = nalt;
    }

}
