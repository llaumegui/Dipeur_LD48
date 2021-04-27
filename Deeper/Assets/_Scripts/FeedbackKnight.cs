using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FeedbackKnight : MonoBehaviour
{
    public enum AnimState
    {
        Idle,
        Stun,
        Moving,
        Aim,
        Throw,
    }

    public AnimState State;
    [SerializeField] SpriteRenderer knightSprite;
    [SerializeField] float hitSpeed = 0.1f;
    bool _lock;
    float xInput;
    float yInput;

    [Header("Animation")]
    public AnimationManager2D AnimManager;
    int _antispam;

    [Header("Chains")]
    [SerializeField] TexturePanning[] chains;
    [SerializeField] Transform finalHinge;
    [SerializeField] Transform knightPod;

    public void Hit()
    {
        knightSprite?.material.DOComplete();
        knightSprite?.material.DOFloat(1, "_Emission", hitSpeed);
        knightSprite?.material.DOFloat(0, "_Emission", hitSpeed / 2).SetDelay(hitSpeed);
    }

    private void Update()
    {
        xInput = Input.GetAxisRaw("Horizontal1P");
        yInput = Input.GetAxisRaw("Vertical1P");
        RopeAnim();
        if (!_lock)
            PlayAnimations();
    }

    private void FixedUpdate()
    {

    }

    void RopeAnim()
    {
        if (!finalHinge || !knightPod)
            return;

        knightPod.position = finalHinge.position;
        Quaternion newRot = finalHinge.rotation;
        newRot.eulerAngles += Vector3.forward * 90;
        knightPod.rotation = newRot;

        if (yInput > 0)
        {
            for (int i = 0; i < chains.Length; i++)
            {
                chains[i].Offset(i % 2 == 0);
            }
        }
        else if (yInput < 0)
        {
            for (int i = 0; i < chains.Length; i++)
            {
                chains[i].Offset(i % 2 != 0);
            }
        }
    }

    void PlayAnimations()
    {
        //Stuned
        if (State != AnimState.Stun)
        {
            //a lancé
            if (State != AnimState.Throw)
            {
                //lancer
                if (State != AnimState.Aim)
                {
                    //move
                    if (State==AnimState.Moving)
                    {
                        if ((yInput > 0 || xInput>0) && _antispam !=1)
                        {
                            _antispam = 1;
                            Debug.Log("ALLO");
                            AnimManager.PlayLoop(AnimationManager2D.States.Move, 0, true);
                        }

                        if ((yInput < 0 || xInput<0) && _antispam != -1)
                        {
                            _antispam = -1;
                            AnimManager.PlayLoop(AnimationManager2D.States.Move);
                        }
                    }
                    else
                    {
                        //idle
                        if (_antispam != 0)
                        {
                            _antispam = 0;
                            AnimManager.Play(AnimationManager2D.States.Idle);
                        }
                    }
                }
                else
                {
                    if (_antispam != 2)
                    {
                        _antispam = 2;
                        AnimManager.PlayLoop(AnimationManager2D.States.Aim);
                        Debug.Log("AimAnimPlay");

                    }
                }
            }
            else
            {
                if (_antispam != 3)
                {
                    _antispam = 3;
                    AnimManager.Play(AnimationManager2D.States.Lancer);
                }
            }
        }
        else
        {
            if (_antispam != 4)
            {
                _antispam = 4;
                AnimManager.PlayLoop(AnimationManager2D.States.Stun, 2);
            }
        }
    }

    public IEnumerator LockAnim(AnimState state, float time)
    {
        State = state;
        _lock = true;
        PlayAnimations();
        yield return new WaitForSeconds(time);
        _lock = false;
    }
}
