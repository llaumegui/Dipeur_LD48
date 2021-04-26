using System.Collections;
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
        if(collision.gameObject.tag == "Player" && PotionPack)
        {
            Physics2D.IgnoreCollision(collision.collider, collision.otherCollider);
            return;
        }

        if(collision.gameObject.tag == "MurFriable" && PotionPack)
        {
            if (collision.gameObject.TryGetComponent(out Ennemy script))
                script.Death();
            Explosion();
            return;
        }

        if(collision.gameObject.tag == "Knight" && PotionPack)
        {
            Controller.KnightPotions+=3;

            if (Controller.KnightPotions > Controller.KnightMaxPotions)
                Controller.KnightPotions = Controller.KnightMaxPotions;

            GameMaster.I.UIPotions(Controller.KnightPotions);

            Destroy(gameObject, .05f);
            return;
        }

        if(collision.gameObject.tag == "Knight" && !PotionPack)
        {
            Physics2D.IgnoreCollision(collision.collider, collision.otherCollider);
            return;
        }

        Explosion();
    }

    void Explosion()
    {
        GameObject instance = null;
        if (PotionPack)
            instance = Instantiate(ExplosionFX, transform.position, Quaternion.identity);
        else
            instance = Instantiate(ExplosionFXSmall, transform.position, Quaternion.identity);
        Destroy(gameObject, .05f);
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
