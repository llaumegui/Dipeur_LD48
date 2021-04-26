using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionExplosion : MonoBehaviour
{
    public float Damage;
    public float TimeExplosion;

    float _time;

    private void Update()
    {
        _time += Time.deltaTime;
        if (_time >= TimeExplosion)
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ennemy")
        {
            Debug.Log(collision.gameObject);
            if (collision.gameObject.TryGetComponent(out Ennemy script))
                script.Health -= Damage;
            else
            {
                if (collision.gameObject.transform.parent.TryGetComponent(out Ennemy ennemy))
                    ennemy.Health -= Damage;
            }
        }

        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Ascenseur")
            GameMaster.I.Health -= Damage;

    }
}
