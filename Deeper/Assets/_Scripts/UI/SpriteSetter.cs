using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteSetter : MonoBehaviour
{
    Image _img;
    public Sprite DefaultSprite;
    public List<Sprite> Sprites;

    int _chancePercent = 10;

    private void OnEnable()
    {
        _img = GetComponent<Image>();
        if (Sprites.Count > 0)
            RandomSprite();
    }

    void RandomSprite()
    {
        int r = Random.Range(1, 101);
        Debug.Log(r);
        if (r <= _chancePercent)
        {
            int randomSprite = Random.Range(0, Sprites.Count);
            _img.sprite = Sprites[randomSprite];
        }
        else
            _img.sprite = DefaultSprite;
    }
}
