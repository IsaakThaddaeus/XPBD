using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Friction : MonoBehaviour
{
    public List<Vector3> positions = new List<Vector3>();

    XPBD xpbd;
    public int n = 2;
    public Vector3 g = new Vector3(0, -9.81f, 0);
    public float muS;
    public float muK;
    void Start()
    {
        xpbd = new XPBD(n);
        xpbd.muS = muS;
        xpbd.muK = muK;
        xpbd.g = g;

        for (int i = 0; i < positions.Count; i++)
        {
            Particle p = new Particle(positions[i], 1);
            p.v = new Vector3(5,0,0);
            xpbd.particles.Add((p));
        }
   

    }

    private void FixedUpdate()
    {
        xpbd.g = g;

        xpbd.simulate();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        for (int i = 0; i < xpbd.particles.Count; i++)
        {
            Gizmos.DrawSphere(xpbd.particles[i].p, 0.1f);
        }
    }
}
