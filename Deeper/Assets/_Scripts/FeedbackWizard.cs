﻿using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class FeedbackWizard : MonoBehaviour
{
    public enum AnimState
    {
        Idle,
        Stun,
        Moving,
    }

    public AnimState State;
    bool _lock;
    float xInput;
    float yInput;
    [SerializeField] SpriteRenderer wizardSprite;
    [SerializeField] float hitSpeed = 0.1f;

    [Header("Animation")]
    public AnimationManager2D AnimManager;
    int _antispam;

    private void Update()
    {
        if (!_lock)
            PlayAnimations();
    }

    public void Hit()
    {
        wizardSprite?.material.DOComplete();
        wizardSprite?.material.DOFloat(1, "_Emission", hitSpeed);
        wizardSprite?.material.DOFloat(0, "_Emission", hitSpeed / 2).SetDelay(hitSpeed);
    }

    void PlayAnimations()
    {
        xInput = Input.GetAxisRaw("Horizontal1P");
        yInput = Input.GetAxisRaw("Vertical1P");

        //Stuned
        if (State != AnimState.Stun)
        {   
            //move
            if (State == AnimState.Moving)
            {
                if ((yInput > 0 || xInput > 0) && _antispam != 1)
                {
                    _antispam = 1;
                    AnimManager.PlayLoop(AnimationManager2D.States.Move, 0, true);
                }

                if ((yInput < 0 || xInput < 0) && _antispam != -1)
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
