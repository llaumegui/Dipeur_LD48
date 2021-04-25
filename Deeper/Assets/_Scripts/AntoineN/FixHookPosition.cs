using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixHookPosition : MonoBehaviour
{
    [SerializeField] Transform anchor;
    Vector2 startPos;
    HingeJoint2D joint2d;

    private void Awake()
    {
        joint2d = GetComponent<HingeJoint2D>();
        startPos = transform.position;
    }

    private void Update()
    {
       joint2d.anchor = new Vector2(anchor.position.x, startPos.y);
    }
}
