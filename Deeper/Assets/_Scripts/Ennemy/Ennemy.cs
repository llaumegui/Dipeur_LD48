﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public abstract class Ennemy : MonoBehaviour
{
    public GameObject Target;
    protected Transform _target;

    [Header("Feedbacks")]
    [SerializeField] protected SpriteRenderer spriteRender;
    [SerializeField] protected float hitSpeed = 0.1f;
    public GameObject DeathFX;
    [SerializeField] protected string DeathSoundID;

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
        spriteRender = GetComponentInChildren<SpriteRenderer>();
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

    public virtual void Hit(float Damage)
    {
        Health -= Damage;
        spriteRender?.material.DOComplete();
        spriteRender?.material.DOFloat(1, "_Emission", hitSpeed);
        spriteRender?.material.DOFloat(0, "_Emission", hitSpeed/2).SetDelay(hitSpeed);
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
        if (AddScore)
            GameMaster.I.Score += ScoreValue;

        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (DeathFX)
            Instantiate(DeathFX, transform.position, Quaternion.identity);
        if (!string.IsNullOrEmpty(DeathSoundID))
            SoundManager.Instance.PlayAudio(DeathSoundID, transform);
    }

    public virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ascenseur" || collision.gameObject.tag == "Player" || collision.gameObject.tag == "Knight")
        {
            GameMaster.I.Health -= GetDamage(collision.gameObject.tag);

            if (collision.gameObject.tag == "Knight")
                GameMaster.I.PlayFeedBack(GameMaster.CharacterType.Knight);

            if (collision.gameObject.tag == "Player")
                GameMaster.I.PlayFeedBack(GameMaster.CharacterType.Wizard);

            if (collision.gameObject.tag == "Ascenseur")
                GameMaster.I.PlayFeedBack(GameMaster.CharacterType.Ascenseur);

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
