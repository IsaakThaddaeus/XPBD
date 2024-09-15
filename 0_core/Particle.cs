using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle
{
    public Vector3 x;
    public Vector3 p;
    public Vector3 v;
    public float w;

    public Particle(Vector3 x, float m)
    {
        this.x = x;
        this.v = Vector3.zero;
        this.w = m > 0 ? 1 / m : 0;
    }
}
