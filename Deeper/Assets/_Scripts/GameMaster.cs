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
    public Texture2D AimCursor;
    [HideInInspector] public bool GameOver;

    [Header("Controller")]
    public CharaController ControllerScript;
    [Range(0, .1f)] public float KnightOffsetWall;
    [HideInInspector] public bool OffsetWall;

    [Header("Values")]
    public float DistanceTick;

    [HideInInspector] public int Score;
    [HideInInspector] public float Health;
    public float MaxHealth;

    int _score;
    float _health;
    float _timeDistance;

    [Header("Level Design")]
    public Vector2 MinMaxTimeSpawnLevel;
    public float TimeDecrease;
    float _timeSpawnLevel;
    float _timeLevel;
    LevelGenerator _ld;

    [Header("LevelPanning")]
    [Range(1,2)]public float PanningTick;
    [Range(1,3)]public float PanningWallTick;
    Transform _panningTransform;
    Transform _panningWallTransform;

    [Header("UI")]
    public UIKnightPotions KnightPotionsScript;
    public TextMeshProUGUI ScoreText;
    public Image HealthFillBar;

    [Header("End")]
    public GameObject CanvasEndGame;
    public GameObject VictoryScreen;
    public GameObject DefeatScreen;

    [Header("Mise en Scène")]
    public List<Vector2> ExplosionsPos;
    public GameObject ExplosionFX;

    [Header("ScreenShake")]
    Camera _mainCam;
    Vector3 _camDefaultPos;
    public float ScreenShakeIntensity;
    public float ShakeTick;
    float _shakeTime;
    bool _shaking;

    private void Awake()
    {
        Cursor.SetCursor(AimCursor,Vector2.zero,CursorMode.Auto);
        _mainCam = Camera.main;
        _camDefaultPos = _mainCam.transform.position;
        _ld = GetComponent<LevelGenerator>();
        _timeSpawnLevel = MinMaxTimeSpawnLevel.y;
        _panningTransform = _ld.PanningTransform;
        _panningWallTransform = _ld.PanningWallTransform;

        Health = MaxHealth;

        if (_i != null && _i != this)
            Destroy(gameObject);

        _i = this;
    }

    private void Start()
    {
        if(TimeDecrease==0)
        {
            TimeDecrease = MinMaxTimeSpawnLevel.y/10;
        }
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

                _timeSpawnLevel -= TimeDecrease;
                if (_timeSpawnLevel <= MinMaxTimeSpawnLevel.x)
                    _timeSpawnLevel = MinMaxTimeSpawnLevel.x;
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

        if(_shaking)
        {
            _shakeTime += Time.deltaTime / ShakeTick;
            if(_shakeTime>=1)
            {
                _shakeTime = 0;
                _shaking = false;
                _mainCam.transform.position = _camDefaultPos;
            }
        }
    }

    public void EndGame(bool win = false)
    {
        GameOver = true;
        if(win)
            StartCoroutine(Victory());
        else
            StartCoroutine(Defeat());
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

    IEnumerator Victory()
    {
        yield return new WaitForSeconds(1);

        CanvasEndGame.SetActive(true);
        VictoryScreen.SetActive(true);
    }

    public void ScreenShake()
    {
        _shaking = true;
        float x = Random.Range(0, ScreenShakeIntensity);
        float y = Random.Range(0, ScreenShakeIntensity);
        _mainCam.transform.position += new Vector3(x, y);
    }

    IEnumerator Defeat()
    {
        if(ExplosionsPos.Count>0)
        {
            for(int i = 0;i<5;i++)
            {
                GameObject instance = Instantiate(ExplosionFX, ExplosionsPos[Random.Range(0, ExplosionsPos.Count)], Quaternion.identity);
                ScreenShake();
                yield return new WaitForSeconds(.3f);
            }
        }
        yield return new WaitForSeconds(.5f);
        CanvasEndGame.SetActive(true);
        DefeatScreen.SetActive(true);

    }

    private void OnDrawGizmos()
    {
        if(ExplosionsPos.Count>0)
        {
            foreach(Vector2 pos in ExplosionsPos)
            {
                Gizmos.DrawWireSphere(pos, .1f);
            }
        }
    }
}
