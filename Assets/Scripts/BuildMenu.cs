using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(HorizontalLayoutGroup))]
public class BuildMenu : Tool
{
    public Frame[] BuildableFrames;
    public GameObject buttonPrefab;
    private const string KEY_PREVIEW = "Selected Frame";

    private Frame preview = null;
    private Ship ship = null;

    public override void Deselect()
    {
        gameObject.SetActive(false);
        if(ship != null)
        {
            ship.BoundaryObject.SetActive(false);
        }
        ship = null;
    }

    public override string GetName()
    {
        return "Build";
    }

    public override void Select(Ship ship)
    {
        this.ship = ship;
        ship.BoundaryObject.SetActive(true);
        gameObject.SetActive(true);
    }

    private void Awake()
    {
        if (buttonPrefab != null)
        {
            foreach (Frame frame in BuildableFrames)
            {
                GameObject obj = Instantiate(buttonPrefab);
                Text txt = obj.GetComponentInChildren<Text>();
                if (txt != null)
                {
                    txt.text = frame.prefixName;
                }
                obj.name = "Button ("+frame.prefixName+")";

                obj.GetComponent<Button>().onClick.AddListener(() =>
                {
                    if(ship != null)
                    {
                        SetPreview(frame);
                    }
                    else
                    {
                        Debug.LogError("ship is null");
                    }
                });
                obj.transform.SetParent(transform);
            }
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (preview != null && GetMousePositionOnXZPlane(ship, out Vector2 location))
        {
            location = new Vector2(Mathf.Round(location.x), Mathf.Round(location.y));
            preview.Location = location;
            if (ship.canPlaceFrameHere(location, preview))
            {
                preview.ShowError = false;
                bool placeMany = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);

                if (Input.GetMouseButtonUp(0))
                {
                    ship.AddFrame(preview.gameObject, location);
                    if (!placeMany)
                    {
                        RemovePreview();
                    }
                }
                else if (placeMany && Input.GetMouseButton(0))
                {
                    ship.AddFrame(preview.gameObject, location);
                }
            }
            else
            {
                preview.ShowError = true;
            }
        }
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
    private void SetPreview(Frame prefabFrame)
    {
        RemovePreview();

        GameObject obj = Instantiate(prefabFrame.gameObject);
        Frame frame = obj.GetComponent<Frame>();
        obj.transform.SetParent(ship.transform);
        Console.Log(KEY_PREVIEW, frame.prefixName);
        frame.prefixName = "Preview Frame";
        preview = frame;
        frame.ShowError = false;
    }
}
