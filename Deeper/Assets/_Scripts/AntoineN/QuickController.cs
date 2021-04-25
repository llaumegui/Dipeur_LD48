using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickController : MonoBehaviour
{
    Rigidbody2D rb => GetComponent<Rigidbody2D>();
    public float speed = 10;

    void Update()
    {
        float xinput = Input.GetAxisRaw("Horizontal");
        rb.AddForce(Vector2.right * xinput * speed, ForceMode2D.Force);
    }
}
