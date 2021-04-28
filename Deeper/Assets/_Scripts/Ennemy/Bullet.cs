using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [HideInInspector] public Ennemy Parent;
    public GameObject FXExplosion;
    [SerializeField] ParticleSystem trailFX;
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
        if (collision.gameObject.tag == "Ascenseur" || collision.gameObject.tag == "Player" || collision.gameObject.tag == "Knight")
        {
            GameMaster.I.Health -= Parent.GetDamage(collision.gameObject.tag);
            switch (collision.gameObject.tag)
            {
                case "Ascenseur":
                    GameMaster.I.PlayFeedBack(GameMaster.CharacterType.Ascenseur);
                    break;
                case "Player":
                    GameMaster.I.PlayFeedBack(GameMaster.CharacterType.Wizard);
                    break;
                case "Knight":
                    GameMaster.I.PlayFeedBack(GameMaster.CharacterType.Knight);
                    break;
                default:
                    break;
            }
        }

        Explosion();
    }

    void Explosion()
    {
        _exploded = true;
        _rb.isKinematic = true;
        _rb.gravityScale = 0;
        _rb.velocity = Vector3.zero;
        trailFX.Stop();
        trailFX.gameObject.AddComponent<SelfDestroyVFX>().destroyDelay = 2f;
        trailFX.transform.parent = null;
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (FXExplosion)
            Instantiate(FXExplosion, transform.position, Quaternion.identity);
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
