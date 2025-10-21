using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionPointerBob : MonoBehaviour
{
    [SerializeField] private float speed = 2f;
    [SerializeField] private float amplitude = 0.02f;    
    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    
    void Update()
    {
        float newY = startPos.y + Mathf.Sin(Time.time * speed) * amplitude;
        transform.position = new Vector3(startPos.x, newY, startPos.z);
    }
}
