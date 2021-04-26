using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomObject : MonoBehaviour
{
    public List<GameObject> Objects;

    private void Start()
    {
        GetRandomObject();
    }

    void GetRandomObject()
    {
        int id = Random.Range(0, Objects.Count);

        for(int i =0;i<Objects.Count;i++)
        {
            if(i == id)
            {
                Objects[i].SetActive(true);
            }
            else
            {
                Objects[i].SetActive(false);
            }
        }
    }
}
