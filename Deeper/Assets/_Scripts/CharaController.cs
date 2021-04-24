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
    float _wizardY = 5;

    Transform _knight;
    Transform _wizard;

    [Header("Movement")]
    [Range(0, 1)]public float Speed;
    [Range(0,1)]public float InertiaIntensity;
    float _xValue;
    float _yValue;

    [Header("Keycodes")]
    public KeyCode DropBomb;

    private void Awake()
    {
        _knight = Knight.GetComponent<Transform>();
        _wizard = Wizard.GetComponent<Transform>();
    }

    private void Start()
    {
        ApplyMovement();
    }

    private void Update()
    {

    }

    private void FixedUpdate()
    {
        Inputs();
    }

    void Inputs()
    {
        float x = Input.GetAxisRaw("Horizontal") * Speed;
        float y = Input.GetAxisRaw("Vertical") * Speed;

        _xValue = Mathf.Lerp(_xValue, x, 1-InertiaIntensity);
        _yValue = Mathf.Lerp(_yValue, y, 1 - InertiaIntensity);

        Movement();
    }

    void Movement()
    {
        float x = PlayersPos.x + _xValue;
        float y = PlayersPos.y + _yValue;

        if(x>MaxDirs.x || x<-MaxDirs.x)
        {
            if (x > MaxDirs.x)
                x = MaxDirs.x;
            else
                x = -MaxDirs.x;
        }

        if (y > MaxDirs.y || y < -MaxDirs.y)
        {
            if (y > MaxDirs.y)
                y = MaxDirs.y;
            else
                y = -MaxDirs.y;
        }

        PlayersPos.x = x;
        PlayersPos.y = y;

        ApplyMovement();
    }

    void ApplyMovement()
    {
        _knight.position = PlayersPos;
        _wizard.position = new Vector2(-PlayersPos.x, _wizardY);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector3(-MaxDirs.x, _wizardY, 0), new Vector3(MaxDirs.x, _wizardY, 0));

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(new Vector3(0,-MaxDirs.y, 0), new Vector3(0,MaxDirs.y, 0));
    }
}
