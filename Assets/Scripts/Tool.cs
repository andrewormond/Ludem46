using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tool : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public abstract void Select(Ship ship);
    public abstract void Deselect();

    public abstract string GetName();




    static Plane XYPlane = new Plane(Vector3.forward, Vector3.zero);

    public static bool GetMousePositionOnXZPlane(Ship ship, out Vector2 location)
    {
        location = Vector2.zero;
        if (!IsMouseAvailable()) return false;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (XYPlane.Raycast(ray, out float distance))
        {
            Vector3 hitPoint = ray.GetPoint(distance);
            //Just double check to ensure the z position is exactly zero
            hitPoint.z = 0;
            hitPoint = ship.transform.InverseTransformPoint(hitPoint);
            location = new Vector2(hitPoint.x, hitPoint.y);
            Console.Log("Mouse", location);
            return true;
        }
        return false;
    }
    public static bool IsMouseAvailable()
    {
        return !MouseInputUIBlocker.BlockedByUI;
    }
}
