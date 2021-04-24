using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    #region Singleton
    static GameMaster _i;
    public static GameMaster I { get
        {
            return _i;
        }
    }
    #endregion



    private void Awake()
    {
        if (_i != null && _i != this)
            Destroy(gameObject);

        _i = this;
        DontDestroyOnLoad(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
