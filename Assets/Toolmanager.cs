using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Toolmanager : MonoBehaviour
{
    public Tool[] Tools;
    public GameObject buttonPrefab;
    public Ship ship;

    private Tool selectedTool = null;
    private const string KEY_TOOL = "Tool";


    void DeselectAny()
    {
        if(selectedTool != null)
        {
            Console.Log(KEY_TOOL, "None");
            selectedTool.Deselect();
        }
    }

    void Select(Tool tool)
    {
        DeselectAny();
        Console.Log(KEY_TOOL, tool.GetName());
        selectedTool = tool;
        selectedTool.Select(ship);
    }

    void Awake()
    {
        if (buttonPrefab != null)
        {
            foreach (Tool tool in Tools)
            {
                GameObject obj = Instantiate(buttonPrefab);
                Text txt = obj.GetComponentInChildren<Text>();
                if (txt != null)
                {
                    txt.text = tool.GetName();
                }
                obj.name = "Button (" + tool.GetName() + ")";

                obj.GetComponent<Button>().onClick.AddListener(() =>
                {
                    Select(tool);
                });
                obj.transform.SetParent(transform);
                tool.Deselect();
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
