using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharaController : MonoBehaviour
{
    public Vector2 MaxDirs;

    [Header("Characters")]
    public GameObject Knight;
    public GameObject Wizard;
    public Vector2 PlayersPos;
    public float WizardPosY = 3;
    public float KnightOffsetY;

    Transform _knight;
    public Transform KnightPivot;
    Transform _wizard;
    bool _knightStun;

    [Header("Animation")]
    public FeedbackKnight FeedbackKnight;

    [Header("Movement")]
    [Range(0, 1)]public float Speed;
    [Range(0,.5f)]public float AccelerationWizard;
    [Range(0,.5f)]public float DecelerationWizard;
    [Range(0,.5f)]public float AccelerationKnight;
    [Range(0, .5f)] public float DecelerationKnight;

    float _xValue;
    float _yValue;
    float _bombInertia;

    [Header("Keycodes")]
    public KeyCode DropBombKeyCode;

    [Header("Instances")]
    public GameObject Potion;
    public Transform WizardSpawnPotion;
    public Transform KnightSpawnPotion;

    [Header("Bombs")]
    public float ReloadTime;
    bool _reloadingWizard;
    public int KnightMaxPotions;
    public int KnightPotions;

    [Header("Throw")]
    public float PowerChargeTime;
    public float MaxPower;
    float _powerMult;
    bool _throwing;
    float _timePowerCharge;
    float _reloadingTime;
    Vector2 _aimDir;

    bool _canThrow;
    bool _hasThrown;

    [Header("UI")]
    public GameObject CanvasPowerBar;
    public Image PowerBarFill;

    private void Awake()
    {
        _knight = Knight.GetComponent<Transform>();
        _wizard = Wizard.GetComponent<Transform>();
    }

    private void Start()
    {
        StartCoroutine(AntiThrow());

        KnightPotions = 2;
        GameMaster.I.UIPotions(KnightPotions);

        ApplyMovement();
    }

    IEnumerator AntiThrow()
    {
        yield return new WaitForSeconds(.5f);
        _canThrow = true;
    }

    private void Update()
    {
        Inputs();

        if (_reloadingWizard)
        {
            _reloadingTime += Time.deltaTime;

            if(_reloadingTime >=ReloadTime)
            {
                _reloadingWizard = false;
                _reloadingTime = 0;
            }
        }

        if(_throwing)
        {
            _timePowerCharge += Time.deltaTime / PowerChargeTime;

            _powerMult = Mathf.Abs(Mathf.Sin(_timePowerCharge));
            PowerBarFill.fillAmount = _powerMult;

            Aim();
        }

        if (_powerMult != 0)
            CanvasPowerBar.SetActive(true);
        else
            CanvasPowerBar.SetActive(false);
    }

    private void FixedUpdate()
    {
        Movement();
    }

    void Inputs()
    {
        if (Input.GetKeyDown(DropBombKeyCode) && !_reloadingWizard)
        {
            DropBomb();
        }

        if(Input.GetMouseButtonDown(0) && KnightPotions>0 && _canThrow)
            _throwing = true;
        if (Input.GetMouseButtonUp(0))
        {
            if(_throwing)
            {
                _throwing = false;
                ThrowBomb();
                StartCoroutine(FeedbackKnight.LockAnim(FeedbackKnight.AnimState.Throw,.2f));
            }
        }
    }

    void DropBomb()
    {
        _reloadingWizard = true;

        GameObject bomb = Instantiate(Potion, WizardSpawnPotion.position, Quaternion.Euler(0, 0, -_bombInertia * 360));

        if (bomb.TryGetComponent(out Rigidbody2D rb))
            rb.AddForce(new Vector2(-_bombInertia * 20, 0), ForceMode2D.Impulse);

        if (bomb.TryGetComponent(out Potion potion))
            potion.Controller = this;
        else
            Debug.LogWarning("Potion Not Linked to CharaController");
    }

    #region Throw
    void Aim()
    {
        Vector2 pointerPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        _aimDir = pointerPos - (Vector2)KnightSpawnPotion.position;

        if (_aimDir.x > 0)
            KnightPivot.localScale = new Vector3(-.5f, .5f, 1);
        else
            KnightPivot.localScale = new Vector3(.5f, .5f, 1);

        //animAim
        FeedbackKnight.State = FeedbackKnight.AnimState.Aim;
    }

    void ThrowBomb()
    {
        _timePowerCharge = 0;

        GameObject bomb = Instantiate(Potion, KnightSpawnPotion.position, Quaternion.identity);

        if (bomb.TryGetComponent(out Rigidbody2D rb))
            rb.AddForce(_aimDir*(MaxPower*_powerMult), ForceMode2D.Force);

        if (bomb.TryGetComponent(out Potion potion))
            potion.Controller = this;
        else
            Debug.LogWarning("Potion Not Linked to CharaController");

        KnightPotions--;
        GameMaster.I.UIPotions(KnightPotions);

        _powerMult = 0;
    }
    #endregion

    void Movement()
    {
        float xInput = 0;
        float yInput = 0;

        xInput = Input.GetAxisRaw("Horizontal") * Speed;

        if (!_throwing)
        yInput = Input.GetAxisRaw("Vertical") * Speed;

        if (xInput != 0)
            _xValue = Mathf.Lerp(_xValue, xInput, AccelerationWizard);
        else
            _xValue = Mathf.Lerp(_xValue, xInput, DecelerationWizard);

        if (yInput != 0)
            _yValue = Mathf.Lerp(_yValue, yInput, AccelerationKnight);
        else
            _yValue = Mathf.Lerp(_yValue, yInput, DecelerationKnight);

        if (_xValue < .01f && _xValue > -.01f && xInput ==0)
            _xValue = 0;
        if (_yValue < .01f && _yValue>-.1f && yInput==0)
            _yValue = 0;

        //Animations
        if (_yValue != 0)
            FeedbackKnight.State = FeedbackKnight.AnimState.Moving;
        else
            FeedbackKnight.State = FeedbackKnight.AnimState.Idle;


        float x = PlayersPos.x + _xValue;
        float y = PlayersPos.y + _yValue;

        if (x > MaxDirs.x || x < -MaxDirs.x)
        {
            _xValue = 0;
            if (x > MaxDirs.x)
                x = MaxDirs.x;
            else
                x = -MaxDirs.x;
        }
        else
            _bombInertia = _xValue;

        if (y > MaxDirs.y || y < -MaxDirs.y)
        {
            _yValue = 0;
            if (y > MaxDirs.y)
                y = MaxDirs.y;
            else
                y = -MaxDirs.y;
        }

        PlayersPos.x = x;
        PlayersPos.y = y;

        ApplyMovement();

        _bombInertia = Mathf.Lerp(_bombInertia, 0, DecelerationWizard);
        if (_bombInertia < .01f && _bombInertia > -.01f)
            _bombInertia = 0;        
    }

    void ApplyMovement()
    {
        _knight.position = PlayersPos + new Vector2(0, KnightOffsetY);
        _wizard.position = new Vector2(-PlayersPos.x, WizardPosY);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector3(-MaxDirs.x, WizardPosY, 0), new Vector3(MaxDirs.x, WizardPosY, 0));

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(new Vector3(0,-MaxDirs.y, 0), new Vector3(0,MaxDirs.y + KnightOffsetY, 0));
    }
}
