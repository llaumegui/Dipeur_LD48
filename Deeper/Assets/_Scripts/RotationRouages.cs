using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationRouages : MonoBehaviour
{
    public float Dir;
    public float TimeRot;
    float _time;

    private void Update()
    {
        _time += Time.deltaTime;
        if(_time>=TimeRot)
        {
            _time = 0;
            transform.Rotate(Vector3.forward * Dir);
        }
    }
}
