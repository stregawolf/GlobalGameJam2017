using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Vector3Extension
{
    public static Vector3 InvertVector(this Vector3 vector)
    {
        return new Vector3(1.0f / vector.x, 1.0f / vector.y, 1.0f / vector.z);
    }
};

public class Wave : MonoBehaviour
{
    public float damping = 0.99f;
    public float spring = 0.3f;
    public float waveStrength = 20.0f;

    public int width = 200;
    public int height = 25;

    private struct Int2
    {
        public int x;
        public int y;
    };

    private Mesh mesh;
    private float[,,] buffer;
    private float[,] velocityBuffer;
    private Vector3[] vertices;

    private int currentBuffer = 0;

    private Collider m_collider;

    // Use this for initialization
    void Start()
    {
        buffer = new float[width, height, 2];
        velocityBuffer = new float[width, height];
        vertices = new Vector3[width * height];

        for (int i = 0; i < width; i++)
            for (int j = 0; j < height; j++)
                for (int k = 0; k < 2; k++)
                {
                    buffer[i, j, k] = 0.0f;
                }

        mesh = new Mesh();

        m_collider = GetComponent<Collider>();
        var mf = GetComponent<MeshFilter>();
        mf.mesh = mesh;

        var uv = new Vector2[width * height];

        for (int i = 0; i < width; i++)
            for (int j = 0; j < height; j++)
            {
                vertices[j * width + i] = new Vector3(i - width / 2, 0.0f, j - height / 2);
                uv[j * width + i] = new Vector2((float)i / (float)width, (float)j / (float)height);
            }

        mesh.vertices = vertices;
        mesh.uv = uv;

        var triangles = new int[6 * width * height];

        int offset = 0;

        for (int i = 0; i < width - 1; i++)
            for (int j = 0; j < height - 1; j++)
            {
                triangles[offset] = i + j * width;
                triangles[offset + 2] = i + 1 + j * width;
                triangles[offset + 1] = i + j * width + width;

                triangles[offset + 3] = i + 1 + j * width;
                triangles[offset + 4] = i + j * width + width;
                triangles[offset + 5] = i + 1 + j * width + width;

                offset += 6;
            }

        mesh.triangles = triangles;
        mesh.MarkDynamic();
        mesh.RecalculateNormals();
        mesh.UploadMeshData(false);
    }

    Int2 WorldToGridCoordinates(Vector3 world)
    {
        Vector3 worldScaled = Vector3.Scale(world, transform.localScale.InvertVector());

        int i = Mathf.RoundToInt(Mathf.Clamp(worldScaled.x + width / 2, 0, width - 1));
        int j = Mathf.RoundToInt(Mathf.Clamp(worldScaled.z + height / 2, 0, height - 1));

        return new Int2 { x = i, y = j };
    }

    void OnCollisionEnter(Collision collision)
    {
        var contact = collision.contacts[0];
        var grid = WorldToGridCoordinates(contact.point);
     
        velocityBuffer[grid.x, grid.y] = collision.impulse.y * waveStrength;
    }

    void OnTriggerEnter(Collider otherCollider)
    {
        Rigidbody rigidBody = otherCollider.attachedRigidbody;

        if (rigidBody)
        {
            var point = m_collider.ClosestPointOnBounds(otherCollider.transform.position);
            var grid = WorldToGridCoordinates(point);

            velocityBuffer[grid.x, grid.y] = waveStrength * rigidBody.velocity.magnitude;
        }
    }

    void OnCollisionStay(Collision collision)
    {
        var contact = collision.contacts[0];
        var grid = WorldToGridCoordinates(contact.point);

        collision.rigidbody.AddForce(contact.normal * velocityBuffer[grid.x, grid.y] * waveStrength);
    }

    void OnTriggerStay(Collider otherCollider)
    {
        Rigidbody rigidBody = otherCollider.attachedRigidbody;

        if (rigidBody)
        {
            var point = m_collider.ClosestPointOnBounds(otherCollider.transform.position);
            var grid = WorldToGridCoordinates(point);

            rigidBody.AddForce(Vector3.up * velocityBuffer[grid.x, grid.y] * waveStrength);
        }
    }

    // Update is called once per frame
    void Update ()
    {
        if (Input.GetMouseButtonUp(0))
        {
            buffer[Random.Range(1, width-1), Random.Range(1, height-1), currentBuffer] = Random.Range(0.0f, waveStrength);
        }

        var nextBuffer = (currentBuffer + 1) % 2;
        for (int i = 1; i < width-1; i++)
            for (int j = 1; j < height-1; j++)
            {
                velocityBuffer[i, j] += ((buffer[i + 1, j, currentBuffer] + buffer[i, j + 1, currentBuffer] + buffer[i - 1, j, currentBuffer] + buffer[i, j - 1, currentBuffer]) / 4.0f - buffer[i, j, currentBuffer]) * spring;
                velocityBuffer[i,j] *= damping;
                buffer[i, j, nextBuffer] = buffer[i, j, currentBuffer] + velocityBuffer[i,j] * Time.deltaTime; 
            }
        currentBuffer = nextBuffer;
        
        for (int i = 0; i < width; i++)
            for (int j = 0; j < height; j++)
            {
                vertices[j * width + i] = new Vector3(i - width/2, buffer[i,j,currentBuffer], j - height/2);
            }

        mesh.vertices = vertices;
        mesh.RecalculateNormals();
    }
}
