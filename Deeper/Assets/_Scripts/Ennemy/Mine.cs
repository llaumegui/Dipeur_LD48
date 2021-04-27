using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Mine : Ennemy
{
    [SerializeField] SpriteRenderer[] renderers;

    public override void Hit(float Damage)
    {
        base.Hit(Damage);
        foreach (var item in renderers)
        {
            item?.material.DOComplete();
            item?.material.DOFloat(1, "_Emission", hitSpeed);
            item?.material.DOFloat(0, "_Emission", hitSpeed / 2).SetDelay(hitSpeed);
        }
    }

    public override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);

        if (collision.gameObject.tag == "Ascenseur" || collision.gameObject.tag == "Player" || collision.gameObject.tag == "Knight")
            Death();
    }

    public override void Death(bool AddScore = false)
    {
        SoundManager.Instance.PlayAudio("Explosion", transform);
        base.Death(AddScore);
    }
}
