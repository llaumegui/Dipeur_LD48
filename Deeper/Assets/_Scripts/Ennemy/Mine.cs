using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : Ennemy
{
    public override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);

        if (collision.gameObject.tag == "Ascenseur" || collision.gameObject.tag == "Player" || collision.gameObject.tag == "Knight")
            Death();
    }

    public override void Death(bool AddScore = false)
    {
        ImportleSonLa.PlaySon("Explosion");
        base.Death(AddScore);
    }
}
