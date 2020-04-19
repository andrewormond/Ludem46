using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class Console : MonoBehaviour
{
    // Start is called before the first frame update
    public static Console Self;
    public static Dictionary<string, string> Entries = new Dictionary<string, string>();

    public static void Log(string key, object value)
    {
        Entries[key] = value.ToString();
        Self?.Reload();
    }

    private void Awake()
    {
        Self = this;
        Reload();
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void Reload()
    {
        Text txt = GetComponent<Text>();
        string s = "";
        foreach (string key in Entries.Keys)
        {
            s += string.Format("{0}: {1}", key, Entries[key]) + Environment.NewLine + Environment.NewLine;
        }
        txt.text = s;
    }
}
