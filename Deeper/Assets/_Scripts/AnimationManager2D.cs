using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager2D : MonoBehaviour
{
	public SpriteRenderer TargetRenderer;
	public SpriteRenderer ShadowRenderer;
	public float Framerate = .1f;
	public AnimationsSprites[] AnimationList;

	[System.Serializable]
	public class AnimationsSprites
	{
		public AnimationManager2D.States State;
		public List<Sprite> Sprites;
		public float CustomFramerate;

		[Header("Debug")]
		public bool Loop;
		public bool Reverse;
		public bool DebugActivate;
	}

	int _index;
	float _timer;

	List<Sprite> _sprites;
	bool _looping;
	float _framerate;
	bool _hasShadow;

	float _timeAnim;

	public enum States
	{
		Idle,
		Aim,
		Move,
		Stun,
		Lancer,
		Death,
		Debug,
	}

	void Start()
	{
		if (TargetRenderer == null)
			TargetRenderer = GetComponent<SpriteRenderer>();
		_sprites = SetSprites(States.Idle);

		if (ShadowRenderer != null)
			_hasShadow = true;
	}

	// Update is called once per frame
	void Update()
	{
		DisplaySprite();

		CheckDebug();

		if(_timeAnim>0)
        {
			_timeAnim -= Time.deltaTime;
			if (_timeAnim <= 0)
				_looping = false;
        }
	}

	void DisplaySprite()
	{
		_timer += Time.deltaTime;
		if (_timer > _framerate)
		{
			_timer = 0;
			_index = (_index + 1) % _sprites.Count;

			if (!_looping && _index == 0)
			{
				_sprites = SetSprites(States.Idle);
			}

			TargetRenderer.sprite = _sprites[_index];
			if (_hasShadow)
				ShadowRenderer.sprite = _sprites[_index];
		}
	}

	public void Play(States state,bool reverse = false)
	{
		_looping = false;
		_sprites = SetSprites(state,reverse);
	}

	public void PlayLoop(States state, float time = 0,bool reverse = false)
	{
		Play(state,reverse);
		_looping = true;

		if (time > 0)
			_timeAnim = time;
	}

	void CheckDebug()
	{
		AnimationsSprites selected = null;
		foreach (AnimationsSprites anim in AnimationList)
		{
			if (anim.DebugActivate)
            {
				selected = anim;
			}

			anim.DebugActivate = false;
		}

		if(selected != null)
			_sprites = SetSprites(selected.State,selected.Reverse,selected.Loop);
	}

	List<Sprite> SetSprites(States state = States.Debug,bool reverse = false, bool debugLoop = false)
	{
		AnimationsSprites animSelected = null;

		foreach (AnimationsSprites anim in AnimationList)
		{
			if (anim.State == state)
				animSelected = anim;
		}

		if (animSelected == null)
			animSelected = AnimationList[0];

		ResetAnim();

		if (debugLoop)
			_looping = true;
		else
			_looping = false;


		if (animSelected.CustomFramerate > 0)
			_framerate = animSelected.CustomFramerate;
		else
			_framerate = Framerate;

		if(!reverse)
		return animSelected.Sprites;
		else
        {
			List<Sprite> reverseList = new List<Sprite>();
			for (int i = animSelected.Sprites.Count - 1; i >= 0; i--)
				reverseList.Add(animSelected.Sprites[i]);

			return reverseList;
        }
	}

	void ResetAnim()
	{
		_index = 0;
		_timer = 0;
	}
}
