using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TexturePanning : MonoBehaviour
{
    SpriteRenderer spr;
    [SerializeField] Vector2 direction;

    public Vector2 Direction { get => direction; set => direction = value; }

    private void Awake()
    {
        spr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        spr.material.mainTextureOffset = Direction * (Time.unscaledTime % 1);
    }
}
