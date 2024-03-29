﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : MonoBehaviour
{
    [HideInInspector] public CharaController Controller;
    Rigidbody2D _rb;
    public SpriteRenderer PotionRenderer;
    public Sprite PotionPackSprite;
    public GameObject ExplosionFX;
    public GameObject ExplosionFXSmall;
    public bool PotionPack;
    [SerializeField] ParticleSystem trailFX;

    [Header("Rotation")]
    public Vector2 GravityDir;
    [Range(0,.2f)] public float LerpIntensity;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        if (PotionPack)
            PotionRenderer.sprite = PotionPackSprite;
    }

    private void FixedUpdate()
    {
        Rotation();
    }

    void Rotation()
    {
        Vector2 dir = _rb.velocity.normalized;
        float angleDif = Vector3.Angle(dir, GravityDir);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, angleDif),LerpIntensity);
    }

    //Potion Throw
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (PotionPack)
            {
                Physics2D.IgnoreCollision(collision.collider, collision.otherCollider);
                return;
            }
            else
                return;
        }

        if (collision.gameObject.tag == "MurFriable" && PotionPack)
        {
            if (collision.gameObject.TryGetComponent(out Ennemy script))
                script.Death();

            Explosion();
            return;
        }

        if (collision.gameObject.tag == "Knight" && PotionPack)
        {
            if (PotionPack)
            {
                Controller.KnightPotions += 3;

                if (Controller.KnightPotions > Controller.KnightMaxPotions)
                    Controller.KnightPotions = Controller.KnightMaxPotions;

                GameMaster.I.UIPotions(Controller.KnightPotions);

                Destroy(gameObject);
                return;
            }
            else
            {
                Physics2D.IgnoreCollision(collision.collider, collision.otherCollider);
                return;
            }
        }

        Explosion();
    }

    void Explosion()
    {
        trailFX.Stop();
        trailFX.gameObject.AddComponent<SelfDestroyVFX>().destroyDelay = 2f;
        trailFX.transform.parent = null;
        SoundManager.Instance.PlayAudio("Explosion", transform);

        if (PotionPack)
            Instantiate(ExplosionFX, transform.position, Quaternion.identity);
        else
            Instantiate(ExplosionFXSmall, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + (Vector3)GravityDir);
    }
}
