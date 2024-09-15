using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceConstraint
{
    public Particle p1;
    public Particle p2;
    public float restLength;
    public float stiffness;

    public DistanceConstraint(Particle p1, Particle p2, float stiffness, float dts2)
    {
        this.p1 = p1;
        this.p2 = p2;
        this.restLength = (p1.x - p2.x).magnitude;
        this.stiffness = stiffness / dts2;
    }

    public void solve()
    {
        float c = (p1.p - p2.p).magnitude - restLength;
        Vector3 n = (p1.p - p2.p).normalized;
        float lamda = -c / (p1.w + p2.w + stiffness);

        if(p1.w > 0 || p2.w > 0)
        {
            p1.p += p1.w * n * lamda;
            p2.p -= p2.w * n * lamda;
        }
     
    }
}
