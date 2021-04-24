using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameMaster : MonoBehaviour
{
    #region Singleton
    static GameMaster _i;
    public static GameMaster I { get
        {
            return _i;
        }
    }
    #endregion

    [Header("Values")]
    public float DistanceTick;

    public int Score;
    public float Health;
    public float MaxHealth;

    int _score;
    float _health;
    float _timeDistance;

    [Header("UI")]
    public UIKnightPotions KnightPotionsScript;
    public TextMeshProUGUI ScoreText;
    public Image HealthFillBar;

    private void Awake()
    {
        Health = MaxHealth;

        if (_i != null && _i != this)
            Destroy(gameObject);

        _i = this;
    }

    void Start()
    {
        
    }

    void Update()
    {
        _timeDistance += Time.deltaTime;
        if(_timeDistance>=DistanceTick)
        {
            _timeDistance = 0;
            Score++;
        }



        if(_score!=Score)
        {
            _score = Score;
            UIScore();
        }
        if(_health != Health)
        {
            _health = Health;
            UIHealth();
        }
    }

    public void UIPotions(int potions)
    {
        KnightPotionsScript.UpdateUI(potions);
    }

    void UIScore()
    {
        ScoreText.text = "SCORE : " + Score;
    }

    void UIHealth()
    {
        float hpFill = (Health/MaxHealth);
        Debug.Log(hpFill);
        HealthFillBar.fillAmount = hpFill;
    }
}
