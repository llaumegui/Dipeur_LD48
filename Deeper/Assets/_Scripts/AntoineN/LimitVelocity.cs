using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitVelocity : MonoBehaviour
{
    [SerializeField] Vector2 maxVelocity;
    Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        rb.rotation = Mathf.Clamp(rb.rotation, maxVelocity.x, maxVelocity.y);
        Quaternion currentTor = transform.rotation;
        Vector3 vector = currentTor.eulerAngles;
        vector.z = Mathf.Clamp(vector.z, maxVelocity.x, maxVelocity.y);
        currentTor.eulerAngles = vector;
        //transform.rotation = currentTor;
    }
}
