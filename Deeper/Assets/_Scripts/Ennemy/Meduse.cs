using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meduse : Ennemy
{
    [Header("Meduse Movement")]
    public AnimationCurve MovementCurve;
    [Range(0,.1f)] public float MeduseSpeed;

    public override void Move()
    {
        if (MaxDirs != Vector2.zero && Target != null && TimeMove != 0)
        {
            if (_target == null)
            {
                _target = Target.GetComponent<Transform>();
                _defaultPos = _target.position;
            }

            _timeMove += Time.deltaTime / TimeMove;

            if (_timeMove >= 1)
                _timeMove = 0;

            float value = MovementCurve.Evaluate(_timeMove);

            if (value % 1 == 0)
                SoundManager.Instance.PlayAudio("MeduseQuiBouge", _target);

            _target.position += (Vector3)((MaxDirs * value)*MeduseSpeed);
        }
    }
}
