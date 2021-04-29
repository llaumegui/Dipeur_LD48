using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public enum TypeOfPrefab
    {
        Mine,
        BigMine,
        Zombie,
        JellyFish,
        Fly,
        Plank,
        Empty,
    }

    public GameObject EndPrefab;
    bool _endPlaced;
    GameMaster _gm;
    int _id;
    public int NbrOfInstances;
    public float OffsetInstances = 1.5f;

    [Header("Instances Placement")]
    public float YPosSpawn;
    public Transform PanningTransform;
    public Transform PanningWallTransform;
    public Transform StillTransform;

    [Header("RandomPlacement")]
    public Vector2 MinMaxPlacements;

    [Header("Level Design")]
    public List<LevelData> LevelOrder;
    public List<LevelPrefab> AllPrefabs;

    private void Awake()
    {
        _gm = GetComponent<GameMaster>();
    }

    private void Start()
    {
        //changeSeed
        Random.InitState(Time.frameCount);
        Debug.Log(Time.frameCount);
    }

    public void TriggerSpawn()
    {
        if(_id<LevelOrder.Count || _gm.InfiniteMode || _id<NbrOfInstances)
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
        else
        {
            if(!_endPlaced)
            {
                _endPlaced = true;
                GameObject instance = Instantiate(EndPrefab, new Vector2(0, YPosSpawn), Quaternion.identity);
                instance.transform.parent = PanningWallTransform;
            }
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
        int nbr = (int)Random.Range(MinMaxPlacements.x, MinMaxPlacements.y);

        if (levelOrderParameters)
            if (LevelOrder[_id].InstanceNumbers != 0)
                nbr = LevelOrder[_id].InstanceNumbers;

        if (prefab.Type == TypeOfPrefab.Plank || prefab.Type == TypeOfPrefab.Zombie)
            nbr = 1;

        float[] pos = new float[nbr];

        for(int i =0;i<nbr;i++)
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

            if (prefab.Type == TypeOfPrefab.Plank || prefab.Type == TypeOfPrefab.Zombie)
                t.parent = PanningWallTransform;

            if(i>0)
            {
                for(int j = i-1;j>=0;j--)
                {
                    float minRange = pos[j] - OffsetInstances;
                    float maxRange = pos[j] + OffsetInstances;
                    if(pos[i]>=minRange && pos[i]<=maxRange)
                    {
                        pos[i] = -100;
                        Destroy(instance);
                        return;
                    }

                }
            }

            pos[i] = x;


            t.position = new Vector2(x, YPosSpawn);
        }

    }

    [System.Serializable]
    public class LevelPrefab
    {
        [Header("Values")]
        public TypeOfPrefab Type;
        public GameObject Instance;
        [HideInInspector] public float XPosition;
        public Vector2 MinMaxXSpawn;
        public bool IsStill;

        [Header("FixedPos")]
        public bool HasFixedPos;
        public List<float> FixedPos;

        public virtual void SetVariables()
        {
            if (!HasFixedPos)
            {
                XPosition = Random.Range(MinMaxXSpawn.x, MinMaxXSpawn.y);
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
        public int InstanceNumbers;
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
