using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XPBD
{
    //Parameters
    public Vector3 g;
    public int substeps;
    public float dtf = 0.02f;
    public float dts;
    public float dts2;

    public float muS;
    public float muK;

    public List<Particle> particles = new List<Particle>();
    public List<DistanceConstraint> constraints = new List<DistanceConstraint>();
    public List<CollisionConstraint> collisionConstraints = new List<CollisionConstraint>();

    public XPBD(int substeps)
    {
        this.substeps = substeps;
        dts = dtf / substeps;
        dts2 = dts * dts;
    }

    public void simulate()
    {
        for (int i = 0; i < substeps; i++)
        {
            //Predict Positions
            for (int j = 0; j < particles.Count; j++)
            {
                particles[j].p = particles[j].x + dts * particles[j].v + dts2 * particles[j].w * g;
            }

            //Solve Constraints
            for (int j = 0; j < constraints.Count; j++)
            {
                constraints[j].solve();
            }

            //Solve Collision Constraints
            generateCollisionConstraints();
            for (int j = 0; j < collisionConstraints.Count; j++)
            {
                collisionConstraints[j].solve();
            }

            //Update Positions
            for (int j = 0; j < particles.Count; j++)
            {
                particles[j].v = (particles[j].p - particles[j].x) / dts;
                particles[j].x = particles[j].p;
            }

        }
    }

    private void generateCollisionConstraints()
    {
        collisionConstraints.Clear();

        for (int j = 0; j < particles.Count; j++)
        {
            Vector3 direction = (particles[j].p - particles[j].x).normalized;
            Vector3 offsetX = particles[j].x - (direction * 0.01f);
            float dist = (particles[j].p - offsetX).magnitude;

            Ray ray = new Ray(offsetX, direction);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, dist))
            {
                collisionConstraints.Add(new CollisionConstraint(particles[j], hit.point, hit.normal, muS, muK));
            }
        }
    }

}



