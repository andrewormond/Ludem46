using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(HorizontalLayoutGroup))]
public class BuildMenu : Tool
{
    public Frame[] BuildableFrames;
    public GameObject buttonPrefab;
    public Ship ship;

    public override void Deselect()
    {
        gameObject.SetActive(false);
    }

    public override string GetName()
    {
        return "Build";
    }

    public override void Select(Ship ship)
    {
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
                        ship.OnBuildClick(frame);
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
        
    }
}
