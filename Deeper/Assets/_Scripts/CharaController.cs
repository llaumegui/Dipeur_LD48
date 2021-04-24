using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    Transform _wizard;

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
    Transform _wizardSpawnPotion;

    [Header("Bombs")]
    public float ReloadTime;
    bool _reloadingWizard;
    public int KnightMaxPotions;
    public int KnightPotions;

    float _reloadingTime;


    private void Awake()
    {
        _knight = Knight.GetComponent<Transform>();
        _wizard = Wizard.GetComponent<Transform>();
        _wizardSpawnPotion = _wizard.GetChild(0);
    }

    private void Start()
    {
        KnightPotions = 2;
        GameMaster.I.UIPotions(KnightPotions);

        ApplyMovement();
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
    }

    void Movement()
    {
        float xInput = Input.GetAxisRaw("Horizontal") * Speed;
        float yInput = Input.GetAxisRaw("Vertical") * Speed;

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

    void DropBomb()
    {
        _reloadingWizard = true;

        GameObject bomb = Instantiate(Potion, _wizardSpawnPotion.position, Quaternion.identity);

        if (bomb.TryGetComponent(out Rigidbody2D rb))
            rb.AddForce(new Vector2(-_bombInertia * 50, -5), ForceMode2D.Impulse);

        if (bomb.TryGetComponent(out Potion potion))
            potion.Controller = this;
        else
            Debug.LogWarning("Potion Not Linked to CharaController");
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector3(-MaxDirs.x, WizardPosY, 0), new Vector3(MaxDirs.x, WizardPosY, 0));

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(new Vector3(0,-MaxDirs.y, 0), new Vector3(0,MaxDirs.y + KnightOffsetY, 0));
    }
}
