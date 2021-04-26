using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIKnightPotions : MonoBehaviour
{
    public TextMeshProUGUI AmmoText;

    public Sprite PotionSprite;
    public float Scale;

    [Header("Debug")]
    public bool DebugBool;
    public int NewValue;

    private void Update()
    {
        if(DebugBool)
        {
            DebugBool = false;
            UpdateUI(NewValue);
        }
    }


    public void UpdateUI(int potions)
    {
        if (AmmoText != null)
            AmmoText.text = potions.ToString();

        //Debug.LogWarning(transform.childCount);

        if(transform.childCount<potions)
        {
            int children = transform.childCount;
            for(int i =0;i<potions- children; i++)
            {
                GameObject icon = new GameObject("icon", typeof(Image));

                icon.transform.SetParent(transform);
                icon.transform.localScale = Vector3.one * Scale;
                icon.GetComponent<Image>().sprite = PotionSprite;
            }
        }

        int activeChildren = 0;
        for (int i = 0; i < transform.childCount; i++)
            if (!transform.GetChild(i).gameObject.activeInHierarchy)
                continue;
            else
                activeChildren++;

        if (activeChildren != potions)
        {
            if (activeChildren < potions)
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    if (activeChildren < potions)
                    {
                        if (transform.GetChild(i).gameObject.activeInHierarchy)
                            continue;
                        else
                        {
                            transform.GetChild(i).gameObject.SetActive(true);
                            activeChildren++;
                        }
                    }
                    else
                        break;
                }
            }
            else
            {
                for (int i = transform.childCount-1; i >= 0; i--)
                {
                    if (activeChildren > potions)
                    {
                        if (!transform.GetChild(i).gameObject.activeInHierarchy)
                            continue;
                        else
                        {
                            transform.GetChild(i).gameObject.SetActive(false);
                            activeChildren--;
                        }
                    }
                    else
                        break;
                }
            }
        }
        else
            return;
    }
}
