using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    bool _lock;

    [Header("Animation")]
    public AnimationManager2D AnimManager;
    int _antispam;

    private void Update()
    {
        if (!_lock)
            PlayAnimations();
    }

    void PlayAnimations()
    {
        float xInput = Input.GetAxisRaw("Horizontal");
        float yInput = Input.GetAxisRaw("Vertical");

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
