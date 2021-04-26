using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [Header("Instances Placement")]
    public float YPosSpawn;
    public Transform PanningTransform;
    public Transform PanningWallTransform;
    public Transform StillTransform;

    GameMaster _gm;
    int _id;

    public enum TypeOfPrefab
    {
        Mine,
        BigMine,
        Skeleton,
        JellyFish,
        Fly,
        Plank,
        Empty,
    }

    [Header("Level Design")]
    public List<LevelData> LevelOrder;

    public List<LevelPrefab> AllPrefabs;

    private void Awake()
    {
        _gm = GetComponent<GameMaster>();
    }

    public void TriggerSpawn()
    {
        if(_id<LevelOrder.Count || _gm.InfiniteMode)
        {
            if(_id<LevelOrder.Count)
            {
                InstantiatePrefab(GetPrefab(LevelOrder[_id].Type),true);
            }
            else
            {
                InstantiatePrefab(GetPrefab());
            }


            _id++;
        }
    }

    LevelPrefab GetPrefab(TypeOfPrefab type = TypeOfPrefab.Empty)
    {
        LevelPrefab prefab = null;

        if (type == TypeOfPrefab.Empty)
            prefab = AllPrefabs[Random.Range(0, AllPrefabs.Count)];
        else
        {
            foreach(LevelPrefab lp in AllPrefabs)
            {
                if (lp.Type != type || prefab != null)
                    continue;
                else
                    prefab = lp;
            }
        }

        return prefab;
    }

    void InstantiatePrefab(LevelPrefab prefab,bool levelOrderParameters = false)
    {
        GameObject instance = Instantiate(prefab.Instance);
        Transform t = instance.GetComponent<Transform>();

        prefab.SetVariables();

        float x = prefab.XPosition;

        if (levelOrderParameters)
            if (LevelOrder[_id].EnableCustomPos)
                x = LevelOrder[_id].CustomPos;

        if (prefab.IsStill)
            t.parent = StillTransform;
        else
            t.parent = PanningTransform;

        if (prefab.Type == TypeOfPrefab.Plank || prefab.Type == TypeOfPrefab.Skeleton)
            t.parent = PanningWallTransform;

        t.position = new Vector2(x, YPosSpawn);

    }

    [System.Serializable]
    public class LevelPrefab
    {
        [Header("Values")]
        public TypeOfPrefab Type;
        public GameObject Instance;
        [HideInInspector]public float XPosition;
        public Vector2 MinMaxXSpawn;
        public bool IsStill;

        [Header("FixedPos")]
        public bool HasFixedPos;
        public List<float> FixedPos;

        public virtual void SetVariables()
        {
            if(!HasFixedPos)
            {
                if (MinMaxXSpawn != Vector2.zero)
                    XPosition = Random.Range(MinMaxXSpawn.x, MinMaxXSpawn.y);
                else
                    XPosition = 0;
            }
            else
            {
                int randID = Random.Range(0, FixedPos.Count);
                XPosition = FixedPos[randID];
            }
        }
    }

    [System.Serializable]
    public class LevelData
    {
        public TypeOfPrefab Type;
        [Header("CustomPos")]
        public bool EnableCustomPos;
        public int CustomPos;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(Vector3.up * YPosSpawn, Vector3.one*.5f);
    }
}
