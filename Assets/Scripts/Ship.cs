using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Ship : MonoBehaviour
{
    public GameObject FramePrefab;

    public Dictionary<Vector2, Frame> frames;

    private const string KEY_PREVIEW = "Selected Frame";

    public Vector2 MaxSize= new Vector2(20, 5);
    public int BoundTop = 2;
    public int BoundBot = -2;
    public int BoundLeft = -3;
    public int BoundRight = 3;
    public GameObject BoundaryObject;

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
        frame.Location = location;
        foreach(Vector2 loc in frame.GetBlockedLocations())
        {
            frames.Add(loc, frame);
        }
        frame.ShowError = false;
        frame.transform.SetParent(transform);
    }

    public void RemoveFrame(Frame frame)
    {
        foreach(Vector2 key in frames.Keys)
        {
            if(frames[key] == frame)
            {
                frames.Remove(key);
            }
        }
        Destroy(frame.gameObject);
    }

    private void RemovePreview()
    {
        if (preview != null)
        {
            Destroy(preview.gameObject);
            preview = null;
        }
        Console.Log(KEY_PREVIEW, "None");
    }

    private Frame preview = null;
    private void SetPreview(Frame prefabFrame)
    {
        RemovePreview();

        GameObject obj = Instantiate(prefabFrame.gameObject);
        Frame frame = obj.GetComponent<Frame>();
        Console.Log(KEY_PREVIEW, frame.prefixName);
        frame.prefixName = "Preview Frame";
        preview = frame;
        frame.ShowError = false;
    }

    private void UpdateBoundaries()
    {
        BoundaryObject.transform.localScale = new Vector3(BoundWidth, BoundHeight, 0.25f);
        BoundaryObject.transform.position = new Vector3(BoundLeft + (BoundWidth - 1) /2f, BoundBot + (BoundHeight - 1)/2f);
    }

    private void Awake()
    {
        frames = new Dictionary<Vector2, Frame>();
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

    static Plane XYPlane = new Plane(Vector3.forward, Vector3.zero);

    public static bool GetMousePositionOnXZPlane(out Vector2 location)
    {
        location = Vector2.zero;
        if (!IsMouseAvailable()) return false;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (XYPlane.Raycast(ray, out float distance))
        {
            Vector3 hitPoint = ray.GetPoint(distance);
            //Just double check to ensure the z position is exactly zero
            hitPoint.z = 0;
            location = new Vector2(hitPoint.x, hitPoint.y);
            return true;
        }
        return false;
    }

    public static bool IsMouseAvailable()
    {
        return !MouseInputUIBlocker.BlockedByUI;
    }

    bool canPlaceFrameHere(Vector2 here, Frame frame)
    {
        foreach (Vector2 offset in frame.BlockedOffsets)
        {
            if(frames.ContainsKey(offset + here))
            {
                return false;
            }
        }

        return true;
    }

    // Update is called once per frame
    void Update()
    {
        if(preview != null && GetMousePositionOnXZPlane(out Vector2 location))
        {
            location = new Vector2(Mathf.Round(location.x), Mathf.Round(location.y));
            preview.Location = location;
            if (canPlaceFrameHere(location, preview))
            {
                preview.ShowError = false;
                bool placeMany = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);

                if (Input.GetMouseButtonUp(0))
                {
                    AddFrame(preview.gameObject, location);
                    if (!placeMany)
                    { 
                        RemovePreview();
                    }
                }else if(placeMany && Input.GetMouseButton(0))
                {
                    AddFrame(preview.gameObject, location);
                }
            }
            else
            {
                preview.ShowError = true;
            }
        }
    }

    public void OnBuildClick(Frame prefab)
    {
        SetPreview(prefab);
        Debug.Log("On build: " + prefab);
    }
}
