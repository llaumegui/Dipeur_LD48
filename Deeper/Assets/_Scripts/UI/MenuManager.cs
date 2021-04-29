using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void PlayButton()
    {
        SceneManager.LoadScene(1);
    }
    public void QuitButton()
    {
        Application.Quit();
    }

    public void SetInfiniteMode(bool activated)
    {
        if (activated)
            GamePresets.InfiniteMode = true;
        else
            GamePresets.InfiniteMode = false;
    }

    public void SetCoopMode(bool activated)
    {
        if (activated)
            GamePresets.Coop = true;
        else
            GamePresets.Coop = false;
    }
}
