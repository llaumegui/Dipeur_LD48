using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TexturePanning : MonoBehaviour
{
    SpriteRenderer spr;
    [SerializeField] float speed = 0.5f;
    public bool active;

    private void Awake()
    {
        spr = GetComponent<SpriteRenderer>();
    }

    public void Offset(bool positive)
    {
        spr.material.SetTextureOffset("_MainTex", Vector2.up * (positive ? speed : -speed) * Time.unscaledTime / 10);
    }

    private void Update()
    {
        if (active)
            Offset(true);
    }
}
