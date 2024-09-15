using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XPBDMeshless
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
    public List<ShapeMatchingConstraint> shapeMatchingConstraints = new List<ShapeMatchingConstraint>();
    public List<CollisionConstraint> collisionConstraints = new List<CollisionConstraint>();

    //Shape Matching
    public Vector3[] verticesInitial;
    public Vector3[] verticesTarget;
    public Vector3[] verticesSimulated;

    public Vector3 centerOfMassInitial;
    public Vector3 centerOfMassSimulated;
    Quaternion quat = Quaternion.identity;

    public XPBDMeshless(int substeps)
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



            //Shape Matching ------------------------------------------------------------------
            for (int j = 0; j < particles.Count; j++)
                verticesSimulated[j] = particles[j].p;
            
            centerOfMassSimulated = StablePolarDecomposition.getCenterOfMass(verticesSimulated);
            verticesTarget = StablePolarDecomposition.matchShape(verticesInitial, verticesSimulated, centerOfMassInitial, quat, 50);
            for(int j = 0; j < particles.Count; j++)
                shapeMatchingConstraints[j].target = verticesTarget[j];
            //---------------------------------------------------------------------------------



            //Solve Constraints shape matching
            for (int j = 0; j < shapeMatchingConstraints.Count; j++)
            {
                shapeMatchingConstraints[j].solve();
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

    public void generateCollisionConstraints()
    {
        collisionConstraints.Clear();

        for (int j = 0; j < particles.Count; j++)
        {
            Vector3 direction = (particles[j].p - particles[j].x).normalized;
            Vector3 offsetX = particles[j].x - (direction * 0.00001f);
            float dist = (particles[j].p - offsetX).magnitude;

            Ray ray = new Ray(offsetX, direction);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, dist))
            {
                collisionConstraints.Add(new CollisionConstraint(particles[j], hit.point, hit.normal, muS, muK));
            }
        }
    }

    /* 
    
    Alternative

    for (int j = 0; j < particles.Count; j++)
    {
        if (particles[j].p.y < 0)
            particles[j].p.y = 0;            
    }
    */
}



