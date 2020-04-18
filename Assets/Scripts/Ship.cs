using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    public GameObject FramePrefab;

    public Dictionary<Vector2, Frame> frames;

    public void AddFrame(GameObject prefab, Vector2 location)
    {
        GameObject obj = Instantiate(FramePrefab);
        Frame frame = obj.GetComponent<Frame>();
        frame.Location = location;
        frames.Add(frame.Location, frame);
    }

    private Frame preview = null;
    private void Awake()
    {
        frames = new Dictionary<Vector2, Frame>();
        for(int i = -3; i < 3; i++)
        {
            AddFrame(FramePrefab, new Vector2(i, 0));
        }
        GameObject obj = Instantiate(FramePrefab);
        Frame frame = obj.GetComponent<Frame>();
        preview = frame;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    static Plane XYPlane = new Plane(Vector3.forward, Vector3.zero);

    public static bool GetMousePositionOnXZPlane(out Vector2 location)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (XYPlane.Raycast(ray, out float distance))
        {
            Vector3 hitPoint = ray.GetPoint(distance);
            //Just double check to ensure the z position is exactly zero
            hitPoint.z = 0;
            location = new Vector2(hitPoint.x, hitPoint.y);
            return true;
        }
        location = Vector2.zero;
        return false;
    }

    // Update is called once per frame
    void Update()
    {
        if(preview != null && GetMousePositionOnXZPlane(out Vector2 location))
        {
            location = new Vector2(Mathf.Round(location.x), Mathf.Round(location.y));
            if (!frames.ContainsKey(location))
            {
                preview.Location = location;
                preview.gameObject.SetActive(true);
                if (Input.GetMouseButtonUp(0))
                {
                    AddFrame(preview.gameObject, location);
                    if(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                    {

                    }
                    else
                    {
                        preview = null;
                    }
                }
            }
            else
            {
                preview.gameObject.SetActive(false);
            }
        }
    }
}
