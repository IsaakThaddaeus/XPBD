using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionConstraint
{
    public Particle p1;
    public float stiffness;
    public Vector3 qc;
    public Vector3 nc;

    public CollisionConstraint(Particle p1, float stiffness, float dts2, Vector3 qc, Vector3 nc)
    {
        this.p1 = p1;
        this.stiffness = stiffness / dts2;
        this.qc = qc;
        this.nc = nc;
    }

    public void solve()
    {
        float c = Vector3.Dot(p1.p - qc, nc);
        float lamda = -c / (stiffness + p1.w);

        p1.p +=  p1.w * nc * lamda;

       
    }
}
