using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;

public class Cloth : MonoBehaviour
{

    [Header("Mesh")]
    public int xVertices = 10;
    public int yVertices = 10;
    public float width = 10f;
    public float height = 10f;
    MeshFilter mf;
    Mesh mesh;

    [Header("Parameter")]
    public float stiffness;
    public float damping;
    public Vector3 g;
    public int n = 2;


    [Header("Particles & Constraints")]
    List<Particle> particles = new List<Particle>();
    List<DistanceConstraint> constraints = new List<DistanceConstraint>();

    XPBD xpbd;

    void Start()
    {
        xpbd = new XPBD(n);


        createMesh();
        createParticelsAndConstraints();

        xpbd.g = g;
        xpbd.particles = particles;
        xpbd.constraints = constraints;


    }

    private void FixedUpdate()
    {
        xpbd.g = g;

        xpbd.simulate();
        updateMesh();

    }

    private void OnDrawGizmos()
    {

        Gizmos.color = Color.white;
        for (int i = 0; i < particles.Count; i++)
        {
            Gizmos.DrawSphere(particles[i].p, 0.05f);
        }

        foreach (var c in constraints)
        {
            Gizmos.DrawLine(c.p1.p, c.p2.p);
        }
    }


    private void createMesh()
    {
        mf = GetComponent<MeshFilter>();
        mesh = new Mesh();
        mf.mesh = mesh;

        Vector3[] vertices = new Vector3[xVertices * yVertices];
        int[] triangles = new int[(xVertices - 1) * (yVertices - 1) * 6];

        float xStep = width / (xVertices - 1);
        float yStep = height / (yVertices - 1);

        for (int y = 0; y < yVertices; y++)
        {
            for (int x = 0; x < xVertices; x++)
            {
                vertices[y * xVertices + x] = new Vector3(x * xStep, y * yStep, 0);
            }
        }

        int triIndex = 0;
        for (int y = 0; y < yVertices - 1; y++)
        {
            for (int x = 0; x < xVertices - 1; x++)
            {
                int baseIndex = y * xVertices + x;
                triangles[triIndex++] = baseIndex;
                triangles[triIndex++] = baseIndex + xVertices;
                triangles[triIndex++] = baseIndex + 1;

                triangles[triIndex++] = baseIndex + 1;
                triangles[triIndex++] = baseIndex + xVertices;
                triangles[triIndex++] = baseIndex + xVertices + 1;
            }
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }
    private void createParticelsAndConstraints()
    {
        for (int y = 0; y < yVertices; y++)
        {
            for (int x = 0; x < xVertices; x++)
            {
                Vector3 pos = mesh.vertices[y * xVertices + x];
                Particle p;

                if ((y == xVertices - 1 || x == 0) && y == yVertices - 1)
                   p = new Particle(pos, 0);

                else
                   p = new Particle(pos, 1);

                particles.Add(p);
            }
        }


        for (int x = 0; x < xVertices; x++)
        {
            for (int y = 0; y < yVertices - 1; y++)
            {
                int index0 = y * xVertices + x;
                int index1 = (y + 1) * xVertices + x;

                DistanceConstraint dc = new DistanceConstraint(particles[index0], particles[index1], stiffness, damping, xpbd.dts2);
                constraints.Add(dc);
            }
        }

        for (int x = 0; x < xVertices - 1; x++)
        {
            for (int y = 0; y < yVertices; y++)
            {
                int index0 = y * xVertices + x;
                int index1 = y * xVertices + x + 1;

                DistanceConstraint dc = new DistanceConstraint(particles[index0], particles[index1], stiffness, damping, xpbd.dts2);
                constraints.Add(dc);
            }
        }

        for (int x = 0; x < xVertices - 1; x++)
        {
            for (int y = 0; y < yVertices - 1; y++)
            {
                int index0 = y * xVertices + x;
                int index1 = (y + 1) * xVertices + x + 1;

                int index2 = (y + 1) * xVertices + x;
                int index3 = y * xVertices + x + 1;

                DistanceConstraint dc0 = new DistanceConstraint(particles[index0], particles[index1], stiffness, damping, xpbd.dts2);
                constraints.Add(dc0);

                DistanceConstraint dc1 = new DistanceConstraint(particles[index2], particles[index3], stiffness, damping, xpbd.dts2);
                constraints.Add(dc1);
            }
        }


    }
    private void updateMesh()
    {
        Vector3[] vertices = new Vector3[mesh.vertexCount];

        for (int y = 0; y < yVertices; y++)
        {
            for (int x = 0; x < xVertices; x++)
            {
                vertices[y * xVertices + x] = particles[y * xVertices + x].p;
            }
        }

        mesh.vertices = vertices;
        mesh.RecalculateNormals();

    }

}







