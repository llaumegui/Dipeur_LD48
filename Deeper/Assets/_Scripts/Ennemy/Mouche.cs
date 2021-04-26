using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouche : Ennemy
{

    public override void Death(bool AddScore = false)
    {
        ImportleSonLa.PlaySon("MoucheKimeur");
        base.Death(AddScore);
    }
}
