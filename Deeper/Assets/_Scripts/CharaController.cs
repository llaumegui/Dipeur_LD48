using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharaController : MonoBehaviour
{
    public bool DebugControl;

    public Vector2 MaxDirs;

    [Header("Characters")]
    public GameObject Knight;
    public GameObject Wizard;
    public Vector2 PlayersPos;
    public float WizardPosY = 3;
    public float KnightOffsetY;
    public Transform KnightCenter;
    [HideInInspector] public float TimeKnightStun;
    [HideInInspector] public float TimeWizardStun;

    Transform _knight;
    public Transform KnightPivot;
    Transform _wizard;
    bool _knightStun;
    bool _wizardStun;
    bool _antispamStunKnight;
    bool _antispamStunWizard;

    [Header("Animation")]
    public FeedbackKnight KnightFeedback;
    public FeedbackWizard WizardFeedback;

    [Header("Movement Wizard")]
    [Range(0, 1)] public float SpeedWizard;
    [Range(0, .5f)] public float AccelerationWizard;
    [Range(0, .5f)] public float DecelerationWizard;

    [Header("Movement Knight")]
    [Range(0, 1)]public float SpeedKnight;
    [Range(0,.5f)]public float AccelerationKnight;
    [Range(0, .5f)]public float DecelerationKnight;
    [Range(0, .1f)]public float DescentLerp;
    [HideInInspector] public float OffsetWallKnight;

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
    public Transform AimArrow;
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
    public GameObject CanvasPowerBarKnight;
    public Image PowerBarFillKnight;
    public GameObject CanvasReloadBarWizard;
    public Image ReloadBarFillWizard;

    [Header("Sons")]
    public AudioSource SonsMouvementKnight;
    public AudioSource SonsMouvementWizard;
    float _defaultVolumeKnight;
    float _defaultVolumeWizard;

    private void Awake()
    {
        _knight = Knight.GetComponent<Transform>();
        _wizard = Wizard.GetComponent<Transform>();

        _defaultVolumeKnight = SonsMouvementKnight.volume;
        _defaultVolumeWizard = SonsMouvementWizard.volume;
    }

    private void Start()
    {
        AimArrow.gameObject.SetActive(false);
        StartCoroutine(ThrowCooldown());

        KnightPotions = 2;
        GameMaster.I.UIPotions(KnightPotions);

        ApplyMovement();
    }

    IEnumerator ThrowCooldown()
    {
        _canThrow = false;
        yield return new WaitForSeconds(.5f);
        _canThrow = true;
        //KnightPivot.localScale = new Vector3(.5f, .5f, 1);
    }

    private void Update()
    {
        Vector3 targetPos = CanvasPowerBarKnight.transform.position;
        targetPos.x = KnightPivot.position.x;
        targetPos.y = KnightPivot.position.y;
        CanvasPowerBarKnight.transform.position = KnightPivot.position;
        if (!GameMaster.I.GameOver || DebugControl)
        {
            Inputs();

            if (_reloadingWizard)
            {
                CanvasReloadBarWizard.SetActive(true);

                _reloadingTime += Time.deltaTime / ReloadTime;
                ReloadBarFillWizard.fillAmount = _reloadingTime;

                if (_reloadingTime >= 1)
                {
                    _reloadingWizard = false;
                    _reloadingTime = 0;
                }
            }
            else
                CanvasReloadBarWizard.SetActive(false);

            if (_throwing && !_knightStun)
            {
                _timePowerCharge += Time.deltaTime / PowerChargeTime;

                _powerMult = Mathf.Abs(Mathf.Sin(_timePowerCharge));
                PowerBarFillKnight.fillAmount = _powerMult;

                Aim();
            }

            if(TimeKnightStun>0)
            {
                _knightStun = true;

                if(!_antispamStunKnight)
                {
                    _antispamStunKnight = true;
                    StartCoroutine(KnightFeedback.LockAnim(FeedbackKnight.AnimState.Stun, TimeKnightStun));
                }

                TimeKnightStun -= Time.deltaTime;
            }
            else
            {
                _antispamStunKnight = false;
                _knightStun = false;
            }

            if (TimeWizardStun > 0)
            {
                _wizardStun = true;

                if (!_antispamStunWizard)
                {
                    _antispamStunWizard = true;
                    WizardFeedback.LockAnim(FeedbackWizard.AnimState.Stun, TimeKnightStun);
                }

                TimeWizardStun -= Time.deltaTime;
            }
            else
            {
                _antispamStunWizard = false;
                _wizardStun = false;
            }

            if (_powerMult != 0)
                CanvasPowerBarKnight.SetActive(true);
            else
                CanvasPowerBarKnight.SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        if(!GameMaster.I.GameOver || DebugControl)
        Movement();
    }

    void Inputs()
    {
        if (Input.GetKeyDown(DropBombKeyCode) && !_reloadingWizard && !_wizardStun)
        {
            DropBomb();
        }

        if (Input.GetMouseButtonDown(0) && KnightPotions>0 && _canThrow && !_knightStun)
        {
            _throwing = true;
            AimArrow.gameObject.SetActive(true);
            SoundManager.Instance.PlayAudio("sor_la_possion", default, 0);
        }
        if (Input.GetMouseButtonUp(0))
        {
            if(_throwing && !_knightStun)
            {
                _throwing = false;
                ThrowBomb();
                AimArrow.gameObject.SetActive(false);
                StartCoroutine(KnightFeedback.LockAnim(FeedbackKnight.AnimState.Throw,.2f));
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
        {
            potion.Controller = this;
            potion.PotionPack = true;
        }
        else
            Debug.LogWarning("Potion Not Linked to CharaController");
    }

    #region Throw
    void Aim()
    {
        Vector2 pointerPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        _aimDir = pointerPos - (Vector2)KnightCenter.position;

        if (_aimDir.x > 0)
            KnightPivot.localScale = new Vector3(-.5f, .5f, 1);
        else
            KnightPivot.localScale = new Vector3(.5f, .5f, 1);

        AimArrow.position = ((Vector2)KnightCenter.position) + ((_aimDir.normalized)*2.5f);

        //animAim
        KnightFeedback.State = FeedbackKnight.AnimState.Aim;
    }

    void ThrowBomb()
    {
        _timePowerCharge = 0;

        GameObject bomb = Instantiate(Potion, KnightSpawnPotion.position, Quaternion.identity);

        if (bomb.TryGetComponent(out Rigidbody2D rb))
            rb.AddForce(_aimDir.normalized*(MaxPower*_powerMult), ForceMode2D.Force);

        if (bomb.TryGetComponent(out Potion potion))
            potion.Controller = this;
        else
            Debug.LogWarning("Potion Not Linked to CharaController");

        KnightPotions--;
        GameMaster.I.UIPotions(KnightPotions);
        _powerMult = 0;
        SoundManager.Instance.PlayAudio("PotionWoosh", default, 0);
        StartCoroutine(ThrowCooldown());
    }
    #endregion

    void Movement()
    {
        float xInput = 0;
        float yInput = 0;

        if(!_wizardStun)
        {
            if (!GameMaster.I.Coop)
                xInput = Input.GetAxisRaw("Horizontal1P") * SpeedWizard;
            else
                xInput = Input.GetAxisRaw("Horizontal2P") * SpeedWizard;
        }

        if (!_throwing && !_knightStun)
        {
            if (!GameMaster.I.Coop)
                yInput = Input.GetAxisRaw("Vertical1P") * SpeedKnight;
            else
                yInput = Input.GetAxisRaw("Vertical2P") * SpeedKnight;
        }

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

        float yOffset=0;
        if(yInput==0)
        {
            yOffset = Mathf.Lerp(0, -1, DescentLerp);
        }

        float x = PlayersPos.x + _xValue;
        float y = PlayersPos.y + _yValue + yOffset + OffsetWallKnight;

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

        //Animations
        if (_xValue != 0)
        {
            SonsMouvementWizard.volume = _defaultVolumeWizard;
            WizardFeedback.State = FeedbackWizard.AnimState.Moving;
        }
        else
        {
            SonsMouvementWizard.volume = 0;
            WizardFeedback.State = FeedbackWizard.AnimState.Idle;
        }

        if (_yValue != 0)
        {
            SonsMouvementKnight.volume = _defaultVolumeKnight;
            KnightFeedback.State = FeedbackKnight.AnimState.Moving;
        }
        else
        {
            SonsMouvementKnight.volume = 0;
            KnightFeedback.State = FeedbackKnight.AnimState.Idle;
        }

        ApplyMovement();

        _bombInertia = Mathf.Lerp(_bombInertia, 0, DecelerationWizard);
        if (_bombInertia < .01f && _bombInertia > -.01f)
            _bombInertia = 0;
    }

    public void ApplyMovement()
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
