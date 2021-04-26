using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ennemy : MonoBehaviour
{
    public GameObject Target;
    protected Transform _target;

    public GameObject DeathFX;

    [Header("Values")]
    public float Health;
    public int ScoreValue;
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

        if (Health<=0 && !_triggerDeath)
        {
            _triggerDeath = true;
            Death(true);
        }
        else
            Move();
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
            //Debug.Log(value);
            _target.position = (Vector2)transform.position+ (MaxDirs * value);
        }
    }

    public float GetDamage(string tag)
    {
        PlayerType type = PlayerType.Empty;

        if (tag == "Ascenseur")
            type = PlayerType.Ascenseur;
        else if (tag == "Knight")
            type = PlayerType.Knight;
        else
            type = PlayerType.Wizard;
        if (type != PlayerType.Empty)
        {
            foreach(DamageData data in DamageCharacters)
            {
                if(data.Type == type)
                {
                    return data.Damage;
                }
            }

            return 0;
        }
        else
            return 0;
    }

    public virtual void Death(bool AddScore = false)
    {
        GameObject instance = null;

        if (DeathFX != null)
            instance = Instantiate(DeathFX, transform.position, Quaternion.identity);

        if (AddScore)
            GameMaster.I.Score += ScoreValue;

        Destroy(instance, .5f);
        Destroy(gameObject, .025f);
    }

    public virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ascenseur" || collision.gameObject.tag == "Player" || collision.gameObject.tag == "Knight")
        {
            GameMaster.I.Health -= GetDamage(collision.gameObject.tag);
            GameMaster.I.ScreenShake();

            if (collision.gameObject.tag == "Ascenseur")
                Death();
        }
    }

    public virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(transform.position, MaxDirs*2);
    }
}
