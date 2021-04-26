using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public Transform target;

    private void Update()
    {
        if (TryGetComponent(out HingeJoint2D joint) && target)
            joint.connectedAnchor = target.position;
    }
}
