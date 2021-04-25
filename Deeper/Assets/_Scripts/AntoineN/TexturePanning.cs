using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TexturePanning : MonoBehaviour
{
    SpriteRenderer spr;
    [SerializeField] float speed = 0.5f;
    bool active;

    public float Speed
    {
        get => speed;
        set
        {
            active = true;
            speed = value;
        }
    }

    private void Awake()
    {
        spr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (active)
            spr.material.mainTextureOffset = Vector2.up * Speed * (Time.unscaledTime % 1);
    }

    private void FixedUpdate()
    {
        active = false;
    }
}
