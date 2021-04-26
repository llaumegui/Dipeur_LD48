using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class MenuInput : MonoBehaviour
{
    public KeyCode KeyInput;
    public bool BackToMainMenu;

    public UnityEvent PlayEvent;

    private void Update()
    {
        if (Input.GetKeyDown(KeyInput))
            if (BackToMainMenu)
                SceneManager.LoadScene(0);
            else
                PlayEvent.Invoke();
    }
}
