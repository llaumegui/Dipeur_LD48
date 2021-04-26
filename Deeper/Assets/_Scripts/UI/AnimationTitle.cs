using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AnimationTitle : MonoBehaviour
{
    public GameObject MenuToSpawn;
    public bool ChangeScale;

    private void Start()
    {
        if(ChangeScale)
        {
            transform.localScale = Vector3.one * 2f;
            transform.DOScale(1, .5f);
        }

        StartCoroutine(SpawnMenu());
    }

    IEnumerator SpawnMenu()
    {
        yield return new WaitForSeconds(1.5f);
        MenuToSpawn.SetActive(true);
    }
}
