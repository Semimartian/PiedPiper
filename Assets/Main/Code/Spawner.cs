using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public enum SpawnableObjects
    {
        DrumStick = 0, Flame = 1, Puff = 2, HairStream = 3, TriggeredMouseTrap = 4, Blood = 5, Length = 6
    }
    [System.Serializable]
    private struct SpawnableObjectDefinition
    {
        public SpawnableObjects name;
        public GameObject preFab;
        public int poolSize;
    }

    public static Spawner instance;
    [SerializeField] private Rigidbody triggeredMouseTrapPreFab;//TODO: Add to spawnables

    [SerializeField] private SpawnableObjectDefinition[] spawnableObjectDefinitions;
    private GameObject[][] spawnablesPool;
    private int[] spawnableIndices;

    private void Awake()
    {
        instance = this;
        InitialisePool();
    }

    private void InitialisePool()
    {
        Transform allSpawnablesParent = new GameObject("AllSpawnables").transform;
        allSpawnablesParent.position = new Vector3();

        int spawnableObjectsLength = (int)SpawnableObjects.Length;
        spawnablesPool = new GameObject[spawnableObjectsLength][];
        spawnableIndices = new int[spawnableObjectsLength];

        for (int i = 0; i < spawnableObjectsLength; i++)
        {
            SpawnableObjects spawnableObjectName = (SpawnableObjects)i; 
            SpawnableObjectDefinition spawnableObjectDefinition = new SpawnableObjectDefinition();
            //Look for definition:
            for (int j = 0; j < spawnableObjectDefinitions.Length; j++)
            {
                if(spawnableObjectDefinitions[j].name == spawnableObjectName)
                {
                    spawnableObjectDefinition = spawnableObjectDefinitions[j];
                    goto DefinitionFound;
                }
            }

            Debug.LogError($"There is a missing spawnable definition for '{ spawnableObjectName.ToString()}'! Fix that.");
            continue;

            DefinitionFound:
            {
                Transform parent = new GameObject(spawnableObjectName.ToString() + "s").transform;
                parent.position = new Vector3();
                parent.SetParent(allSpawnablesParent);

                GameObject[] spawnableArray = spawnablesPool[i] = new GameObject[spawnableObjectDefinition.poolSize];
                for (int j = 0; j < spawnableArray.Length; j++)
                {
                    GameObject spawnable = spawnableArray[j] = Instantiate(spawnableObjectDefinition.preFab);
                    spawnable.transform.SetParent(parent);
                    spawnable.SetActive(false);
                }
            }
        }
    }

    public GameObject Spawn(SpawnableObjects spawnableObjectName, Vector3? spawnPosition = null )
    {
        int spawnableArrayIndex = (int)spawnableObjectName;
        ref int spawnableIndex = ref spawnableIndices[spawnableArrayIndex];

        GameObject spawnedObject = spawnablesPool[spawnableArrayIndex][spawnableIndex];
        spawnedObject.SetActive(true);
        if (spawnPosition != null)
        {
            spawnedObject.transform.position = (Vector3)spawnPosition;
        }
        Spawnable spawnable = spawnedObject.GetComponent<Spawnable>();
        if (spawnable != null)
        {
            spawnable.Spawn();
        }

        spawnableIndex++;
        if(spawnableIndex >= spawnablesPool[spawnableArrayIndex].Length)
        {
            spawnableIndex = 0;
        }

        return spawnedObject;
    }

    public Rigidbody SpawnTriggeredMouseTrap()
    {
        return Instantiate(triggeredMouseTrapPreFab);
    }
}
