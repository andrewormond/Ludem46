using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PID
{
    public float P = 0;
    public float I = 0;
    public float D = 0;

    public int maxValuesStored = 100;

    private float LE = 0;
    private float LTme = 0;
    private float time = 0;
    float ei = 0;

    private float CalculatePI(float e)
    {
        float tElap = time - LTme;
        if(tElap > 0)
        {
            float sml = Mathf.Min(e, LE);
            float lrg = Mathf.Max(e, LE);
            ei += (sml + (lrg - sml) / 2f) * tElap;
        }
        return I * ei;
    }

    public float Process(float setPoint, float processVariable, float timeStep)
    {
        time += timeStep;

        float e = setPoint - processVariable;
        float ep = e * P;
        if (timeStep <= 0)
        {
            return ep;
        }

        float ed = D * (e - LE) / timeStep;
        float ei = CalculatePI(e);

        Console.Log("PID", string.Format("(eP:{0:0.000}, ed:{1:0.000}, ei:{2:0.000})", ep, ed, ei));




        LE = e;
        LTme = time;
        return ep + ed + ei;

    }
}
