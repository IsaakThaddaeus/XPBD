using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionConstraint
{
    public Particle p1;
    public float stiffness;
    public Vector3 qc;
    public Vector3 nc;

    private float muS;
    private float muK;

    public CollisionConstraint(Particle p1, Vector3 qc, Vector3 nc, float muS, float muK)
    {
        this.p1 = p1;
        this.qc = qc;
        this.nc = nc;
        this.muS = muS;
        this.muK = muK;
    }

    public void solve()
    {
        

        float c = Vector3.Dot(p1.p - qc, nc);
        p1.p += nc * -c;
   
        Vector3 deltaX = p1.p - p1.x;
        Vector3 deltaXt = deltaX - Vector3.Dot(deltaX, nc) * nc; // tangential component of deltaX
        
        float deltaL = deltaXt.magnitude;
        float depth = -c;

        if (deltaL < depth * muS){
            p1.p -= deltaXt;
        }

        else{
            float frictionAdjustment = (depth * muK) / deltaL;
            p1.p -= deltaXt * Mathf.Min(frictionAdjustment, 1);
        }

       

        /*
        float c = Vector3.Dot(p1.p - qc, nc);
        float lamda = -c / p1.w;
        p1.p +=  p1.w * nc * lamda;
        */

    }
}
