using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : Ennemy
{
    public AnimationManager2D Anim;

    public float TimeBeforeAttack;
    float _timeBeforeAttack;
    
    [Header("Throw")]
    public Vector2 ThrowDir;
    public float Power;

    bool _attacking;

    [Header("Instances")]
    public GameObject Bullet;
    public Transform BulletSpawn;

    public override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        if (transform.position.x > 0)
        {
            Target.transform.localScale = new Vector3(-.5f, .5f, 1);
            ThrowDir.x = -ThrowDir.x;
        }
    }

    public override void Update()
    {
        base.Update();

        if(!_attacking && Health>0)
        {
            _timeBeforeAttack += Time.deltaTime;
            if (_timeBeforeAttack >= TimeBeforeAttack)
            {
                _timeBeforeAttack = 0;
                _attacking = true;
                Anim.Play(AnimationManager2D.States.Lancer);

                StartCoroutine(AttackingCooldown());
            }
        }
    }

    IEnumerator AttackingCooldown()
    {
        yield return new WaitForSeconds(.5f);
        Throw();
        _attacking = false;
    }

    public void Throw()
    {
        GameObject instance = Instantiate(Bullet, BulletSpawn.position, Quaternion.identity,transform);
        if (instance.TryGetComponent(out Rigidbody2D rb))
            rb.AddForce(ThrowDir.normalized * Power, ForceMode2D.Force);
        if (instance.TryGetComponent(out Bullet script))
            script.Parent = this;
    }

    public override void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, (Vector2)transform.position + ThrowDir);
    }
}
