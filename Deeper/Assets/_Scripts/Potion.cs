using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : MonoBehaviour
{
    [HideInInspector] public CharaController Controller;

    public CircleCollider2D CircleCollider;
    public float ExplosionScale;

    Rigidbody2D _rb;

    public GameObject ExplosionObject;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    //Potion Throw
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Physics2D.IgnoreCollision(collision.collider, collision.otherCollider);
            return;
        }

        if (collision.gameObject.tag == "Wall" || collision.gameObject.tag == "Ennemy")
        {
            Explosion();
        }

        if(collision.gameObject.tag == "Knight")
        {
            Controller.KnightPotions+=3;

            if (Controller.KnightPotions > Controller.KnightMaxPotions)
                Controller.KnightPotions = Controller.KnightMaxPotions;

            GameMaster.I.UIPotions(Controller.KnightPotions);

            Destroy(gameObject, .05f);
            return;
        }
    }

    //Explosion
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Wall" || collision.gameObject.tag == "Ennemy")
        {
            Debug.Log("ALLO");
        }
    }

    void Explosion()
    {
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
}
