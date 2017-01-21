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
    public Material material;
    public float damping = 0.99f;
    public float spring = 0.3f;
    public float waveStrength = 20.0f;

    const int width = 100;
    const int height = 100;

    private float[,,] buffer = new float[width,height,2];
    private float[,] velocityBuffer = new float[width, height];
    private int currentBuffer = 0;

	// Use this for initialization
	void Start ()
    {
        for (int i = 0; i < width; i++)
            for (int j = 0; j < height; j++)
                for (int k = 0; k < 2; k++)
                {
                    buffer[i, j, k] = 0.0f;
                }
    }

    void OnCollisionEnter(Collision collision)
    {
        var contact = collision.contacts[0];

        Vector3 contactScaled = Vector3.Scale(contact.point, transform.localScale.InvertVector());

        int i = Mathf.RoundToInt(Mathf.Clamp(contactScaled.x + 50, 0, 100));
        int j = Mathf.RoundToInt(Mathf.Clamp(contactScaled.z + 50, 0, 100));

        velocityBuffer[i, j] = collision.impulse.y * waveStrength;
    }

    void OnCollisionStay(Collision collision)
    {
        var contact = collision.contacts[0];

        Vector3 contactScaled = Vector3.Scale(contact.point, transform.localScale.InvertVector());

        int i = Mathf.RoundToInt(Mathf.Clamp(contactScaled.x + 50, 0, 100));
        int j = Mathf.RoundToInt(Mathf.Clamp(contactScaled.z + 50, 0, 100));

        collision.rigidbody.AddForce(contact.normal * velocityBuffer[i, j] * waveStrength);
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
	}

    void OnRenderObject()
    {
        material.SetPass(0);

        GL.PushMatrix();
        GL.MultMatrix(transform.localToWorldMatrix);
        GL.Begin(GL.QUADS);

        for(int i=0;i<99;i++)
            for (int j = 0; j < 99; j++)
            {
                GL.Color(new Color(0.25f, 0.25f, 0.5f + buffer[i, j, currentBuffer] / waveStrength));
                GL.Vertex3(i-width/2, buffer[i, j, currentBuffer], j - height / 2);
                GL.Color(new Color(0.25f, 0.25f, 0.5f + buffer[i, j+1, currentBuffer] / waveStrength));
                GL.Vertex3(i - width / 2, buffer[i, j + 1, currentBuffer], j - height / 2 + 1);
                GL.Color(new Color(0.25f, 0.25f, 0.5f + buffer[i+1, j+1, currentBuffer] / waveStrength));
                GL.Vertex3(i - width / 2 + 1, buffer[i + 1, j + 1, currentBuffer], j - height / 2 + 1);
                GL.Color(new Color(0.25f, 0.25f, 0.5f+buffer[i+1, j, currentBuffer] / waveStrength));
                GL.Vertex3(i - width / 2 + 1, buffer[i + 1, j, currentBuffer], j- height/2);
            }
        GL.End();
        GL.PopMatrix();
    }
}
