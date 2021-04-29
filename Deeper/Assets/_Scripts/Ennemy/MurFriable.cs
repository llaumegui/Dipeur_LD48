using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MurFriable : Ennemy
{
    bool _enableOffset;

    public override void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ascenseur")
        {
            GameMaster.I.PlayFeedBack(GameMaster.CharacterType.Ascenseur);
            Death();
        }
        else
            return;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Knight")
            _enableOffset = true;
    }

    private void FixedUpdate()
    {
        if (_enableOffset)
        {
            _enableOffset = false;
            GameMaster.I.OffsetWall = true;
        }
        else
            GameMaster.I.OffsetWall = false;
    }

    public override void Death(bool AddScore = false)
    {
        SoundManager.Instance.PlayAudio("Explosion", transform);
        base.Death(AddScore);
    }
}
