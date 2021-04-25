using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ennemy : MonoBehaviour
{
    public GameObject Target;
    protected Transform _target;

    [Header("Values")]
    public int Health;
    bool _triggerDeath;

    [Header("Movement")]
    public Vector2 MaxDirs;
    public float TimeMove;
    protected float _timeMove;
    protected Vector2 _defaultPos;

    public enum PlayerType
    {
        Ascenseur,
        Knight,
        Wizard,
        Empty,
    }

    [System.Serializable]
    public class DamageData
    {
        public PlayerType Type;
        public float Damage;
        public int TimeStun;
    }

    public DamageData[] DamageCharacters = new DamageData[(int)PlayerType.Empty];

    public virtual void Awake()
    {
        _defaultPos = transform.position;
    }

    public virtual void Update()
    {
        Move();

        if(Health<=0 && !_triggerDeath)
        {
            _triggerDeath = true;
            Death();
        }
    }

    public virtual void Move()
    {
        if (MaxDirs != Vector2.zero && Target != null && TimeMove != 0)
        {
            if (_target == null)
            {
                _target = Target.GetComponent<Transform>();
            }

            _timeMove += Time.deltaTime / TimeMove;

            float value = Mathf.Cos(_timeMove);
            Debug.Log(value);
            _target.position = (Vector2)transform.position+ (MaxDirs * value);
        }
    }

    public abstract void Death();

    public abstract void OnCollisionEnter2D(Collision2D collision);

    public virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(transform.position, MaxDirs*2);
    }
}
