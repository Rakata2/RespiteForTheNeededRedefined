using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class PersonMove : MonoBehaviour
{
    public GameObject PersonImage;
    public Transform StartPoint;
    public Transform Endpoint;
    public float speed = 4f;

    private static PersonMove instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void MovePerson()
    {
        transform.position = Vector3.Lerp(StartPoint.position, Endpoint.position, speed);
    }
}
