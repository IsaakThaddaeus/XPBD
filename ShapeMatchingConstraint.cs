using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeMatchingConstraint
{
    public Particle p;
    public Vector3 target;
    public float stiffness;

    public ShapeMatchingConstraint(Particle p, Vector3 target, float stiffness, float dts2)
    {
        this.p = p;
        this.target = target;
        this.stiffness = stiffness / dts2;


    }

    public void solve()
    {
        float c = (p.p - target).magnitude;
        Vector3 n = (p.p - target).normalized;
        float lamda = -c / (p.w + stiffness);

        p.p += p.w * n * lamda;
           
    }
}
