using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static Resource;

public class Ship : MonoBehaviour
{
    public GameObject FramePrefab;

    public Dictionary<Vector2, Frame> frameGrid = new Dictionary<Vector2, Frame>();
    public List<Frame> allFrames = new List<Frame>();

    public int BoundTop = 2;
    public int BoundBot = -2;
    public int BoundLeft = -3;
    public int BoundRight = 3;
    public GameObject BoundaryObject;
    public Slider AltitudeSlider;
    public Slider ThrottleSlider;


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
        for(int i = BoundLeft; i <= BoundRight; i++)
        {
            AddFrame(FramePrefab, new Vector2(i, 0));
        }
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

    Dictionary<ResType, int> Balance = GetInitializedDictionary();

    public Dictionary<ResType, int> GetSupply()
    {
        Dictionary<ResType, int> resources = new Dictionary<ResType, int>();
        for (int i = 0; i < (int)ResType.NumberResources; i++)
        {
            resources[(ResType)i] = 0;
        }

        foreach (Frame frame in allFrames)
        {
            foreach (Resource res in frame.GetResources())
            {
                if (resources.ContainsKey(res.type) && res.amount > 0)
                {
                    resources[res.type] += res.amount;
                }
            }
        }
        return resources;
    }
    public Dictionary<ResType, int> GetDemand()
    {
        Dictionary<ResType, int> resources = new Dictionary<ResType, int>();
        for (int i = 0; i < (int)ResType.NumberResources; i++)
        {
            resources[(ResType)i] = 0;
        }

        foreach (Frame frame in allFrames)
        {
            foreach (Resource res in frame.GetResources())
            {
                if (resources.ContainsKey(res.type) && res.amount < 0)
                {
                    resources[res.type] += res.amount;
                }
                else
                {
                    resources[res.type] = res.amount;
                }
            }
        }
        return resources;
    }


    public Dictionary<ResType, int> GetTotalResourceBalance()
    {
        Dictionary<ResType, int> resources = new Dictionary<ResType, int>();
        for (int i = 0; i < (int)ResType.NumberResources; i++)
        {
            resources[(ResType)i] = 0;
        }

        foreach (Frame frame in allFrames)
        {
            foreach (Resource res in frame.GetResources())
            {
                if (resources.ContainsKey(res.type))
                {
                    resources[res.type] += res.amount;
                }
                else
                {
                    resources[res.type] = res.amount;
                }
            }
        }
        return resources;
    }



    // Update is called once per frame
    void Update()
    {
        if(AltitudeSlider != null)
        {
            AltitudeSlider.value = transform.position.y;
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

    private void FixedUpdate()
    {
        resources = GetTotalResourceBalance();
        foreach (ResType resType in resources.Keys)
        {
            Console.Log(resType.ToString(), resources[resType]);
        }

        Console.Log("Supported Mass", resources[ResType.Thrust]);

        Rigidbody rigidbody = GetComponent<Rigidbody>();
        rigidbody.mass = resources[ResType.Mass];
        rigidbody.AddForce(new Vector3(0, -9.8f * rigidbody.mass, 0));

        throttle = AltitudeController(TargetAltitude, transform.position.y);
        if (throttle > 1f) throttle = 1f;
        if (throttle < 0f) throttle = 0f;
        ThrottleSlider.value = throttle;
        Console.Log("Throttle", throttle);
        float thrust =  throttle * resources[ResType.Thrust];
        Console.Log("Thrust", thrust);
        rigidbody.AddForce(new Vector3(0, thrust*9.8f, 0));

        Console.Log("Altitude", transform.position.y + " m");
    }


    public void SetAltitude(float nalt)
    {
        TargetAltitude = nalt;
    }

}
