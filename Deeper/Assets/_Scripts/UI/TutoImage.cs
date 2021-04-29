using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutoImage : MonoBehaviour
{
    public Sprite Solo;
    public Sprite Coop;


    private void OnEnable()
    {
        if (GamePresets.Coop)
            GetComponent<Image>().sprite = Coop;
        else
            GetComponent<Image>().sprite = Solo;
    }
}
