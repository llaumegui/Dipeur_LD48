using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;

public class FeedBackButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public float ScaleIntensity = 1.5f;

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.DOScale(ScaleIntensity, .5f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.DOScale(1, .5f);
    }
}
