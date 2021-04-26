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
    public bool GameOver;

    [Header("Controller")]
    public CharaController ControllerScript;
    [Range(0, .1f)] public float KnightOffsetWall;
    [HideInInspector] public bool OffsetWall;

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
    [Header("LevelPanning")]
    [Range(1,2)]public float PanningTick;
    [Range(1,3)]public float PanningWallTick;
    Transform _panningTransform;
    Transform _panningWallTransform;

    [Header("UI")]
    public UIKnightPotions KnightPotionsScript;
    public TextMeshProUGUI ScoreText;
    public Image HealthFillBar;

    private void Awake()
    {
        _ld = GetComponent<LevelGenerator>();
        _timeSpawnLevel = MinMaxTimeSpawnLevel.y;
        _panningTransform = _ld.PanningTransform;
        _panningWallTransform = _ld.PanningWallTransform;

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
        if(!GameOver)
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
            _panningTransform.position += Vector3.up * (Time.deltaTime / PanningTick);
            _panningWallTransform.position += Vector3.up * (Time.deltaTime / PanningWallTick);
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

        if (Health <= 0)
            EndGame();
    }

    void EndGame(bool win = false)
    {
        GameOver = true;
        //trigger end;
    }

    private void FixedUpdate()
    {
        if (OffsetWall)
        {
            ControllerScript.OffsetWallKnight = KnightOffsetWall;
            OffsetWall = false;
        }
        else
            ControllerScript.OffsetWallKnight = 0;
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
