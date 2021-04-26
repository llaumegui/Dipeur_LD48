using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : MonoBehaviour
{
    [HideInInspector] public CharaController Controller;

    public CircleCollider2D CircleCollider;
    public float ExplosionScale;

    Rigidbody2D _rb;

    public SpriteRenderer PotionRenderer;
    public Sprite PotionPackSprite;
    public GameObject ExplosionObject;

    public bool PotionPack;
    public Vector2 SmallBigDamage;
    float _damage;

    [Header("Rotation")]
    public Vector2 GravityDir;
    [Range(0,.2f)] public float LerpIntensity;

    bool _exploded;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        if (PotionPack)
        {
            PotionRenderer.sprite = PotionPackSprite;
            _damage = SmallBigDamage.y;
        }
        else
            _damage = SmallBigDamage.x;
    }

    private void FixedUpdate()
    {
        if(!_exploded)
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

        if (collision.gameObject.tag == "Wall" || collision.gameObject.tag == "Ennemy" || ((collision.gameObject.tag == "Player" || collision.gameObject.tag == "Knight" || collision.gameObject.tag == "Ascenseur") && !PotionPack))
        {
            Explosion();
        }
    }

    //Explosion
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ennemy")
        {
            Debug.Log(collision.gameObject);
            if (collision.gameObject.TryGetComponent(out Ennemy script))
                script.Health -= _damage;
            else
            {
                if (collision.gameObject.transform.parent.TryGetComponent(out Ennemy ennemy))
                    ennemy.Health -= _damage;
            }
        }

        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Ascenseur")
            GameMaster.I.Health -= _damage;

    }

    void Explosion()
    {
        _exploded = true;
        _rb.isKinematic = true;
        _rb.gravityScale = 0;
        _rb.velocity = Vector3.zero;
        ExplosionObject.SetActive(true);
        Destroy(gameObject, .5f);
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
