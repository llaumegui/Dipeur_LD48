using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [HideInInspector] public Ennemy Parent;
    Rigidbody2D _rb;

    [Header("Rotation")]
    public Vector2 GravityDir;
    [Range(0, .2f)] public float LerpIntensity;

    bool _exploded;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (!_exploded)
            Rotation();
    }

    void Rotation()
    {
        Vector2 dir = _rb.velocity.normalized;
        float angleDif = Vector3.Angle(dir, GravityDir);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, -angleDif), LerpIntensity);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Ennemy")
        {
            Physics2D.IgnoreCollision(collision.collider, collision.otherCollider);
        }
        else
        {
            Explosion();

            if (collision.gameObject.tag == "Ascenseur" || collision.gameObject.tag == "Player" || collision.gameObject.tag == "Knight")
            {
                GameMaster.I.Health -= Parent.GetDamage(collision.gameObject.tag);
            }
        }
    }

    void Explosion()
    {
        _exploded = true;
        _rb.isKinematic = true;
        _rb.gravityScale = 0;
        _rb.velocity = Vector3.zero;
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
