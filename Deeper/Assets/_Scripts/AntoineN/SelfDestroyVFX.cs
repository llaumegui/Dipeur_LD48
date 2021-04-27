using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestroyVFX : MonoBehaviour
{
    public float destroyDelay = 5f;
    ParticleSystem fx;

    IEnumerator Start()
    {
        fx = GetComponent<ParticleSystem>();
        yield return new WaitForSeconds(fx.main.duration + fx.main.startLifetimeMultiplier);
        Destroy(gameObject, destroyDelay);
    }
}
