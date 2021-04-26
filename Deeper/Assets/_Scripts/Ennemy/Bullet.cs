using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [HideInInspector] public Ennemy Parent;
    public GameObject FXExplosion;
    Rigidbody2D _rb;

    [Header("Rotation")]
    public Vector2 GravityDir;
    [Range(0, .2f)] public float LerpIntensity;

    bool _exploded;
    public float Mult = 1;

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
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, (-angleDif * Mult)), LerpIntensity);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Explosion();

        if (collision.gameObject.tag == "Ascenseur" || collision.gameObject.tag == "Player" || collision.gameObject.tag == "Knight")
        {
            GameMaster.I.Health -= Parent.GetDamage(collision.gameObject.tag);
        }
    }

    void Explosion()
    {
        _exploded = true;
        _rb.isKinematic = true;
        _rb.gravityScale = 0;
        _rb.velocity = Vector3.zero;

        GameObject instance = Instantiate(FXExplosion, transform.position, Quaternion.identity);
        Destroy(instance, 1f);
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
