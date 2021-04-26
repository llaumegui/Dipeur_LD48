using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SetScoreCanvas : MonoBehaviour
{
    TextMeshProUGUI _text;

    private void Start()
    {
        _text = GetComponent<TextMeshProUGUI>();
        _text.text = "SCORE : " + GameMaster.I.Score;
    }
}
