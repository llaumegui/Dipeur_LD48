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

    public bool InfiniteMode;

    [Header("Values")]
    public float DistanceTick;

    public int Score;
    public float Health;
    public float MaxHealth;

    int _score;
    float _health;
    float _timeDistance;

    [Header("Level Design")]
    LevelGenerator _ld;
    public Vector2 MinMaxTimeSpawnLevel;
    float _timeSpawnLevel;
    float _timeLevel;
    public SpriteRenderer Panning;
    Transform _panningTransform;

    [Header("UI")]
    public UIKnightPotions KnightPotionsScript;
    public TextMeshProUGUI ScoreText;
    public Image HealthFillBar;

    private void Awake()
    {
        _ld = GetComponent<LevelGenerator>();
        _timeSpawnLevel = MinMaxTimeSpawnLevel.y;
        _panningTransform = _ld.PanningTransform;
        Panning.size += Vector2.up * 1000;

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
        if(Health>0)
        {
            _timeDistance += Time.deltaTime;
            if (_timeDistance >= DistanceTick)
            {
                _timeDistance = 0;
                Score++;
            }
            _timeLevel += Time.deltaTime;
            if (_timeLevel >= _timeSpawnLevel)
            {
                _timeLevel = 0;
                _ld.TriggerSpawn();
            }

            //Panning
            _panningTransform.position += Vector3.up * (Time.deltaTime / DistanceTick);
            Panning.size -= new Vector2(0, Time.deltaTime / DistanceTick);
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
