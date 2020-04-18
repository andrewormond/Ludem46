﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frame : MonoBehaviour
{

    private Vector2 location = new Vector2(0, 0);

    public string prefixName = "Frame";

    public GameObject[] ValidObjects;
    public GameObject[] InvalidObjects;

    private Ship ship = null;

    public virtual Vector2 Location
    {
        get => location; set
        {
            location = value;
            transform.position = value;
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
    public Ship Ship { get => ship; set => ship = value; }
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
