using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //[SerializeField] private Transform[] birds;
    /*public static Transform[] Birds;*/
    //[SerializeField] private float DetermineBirdOrderInterval = 0.5f;
    private static Baby[] babies;
    private static Mother mother;

    private static GameManager instance;
    // Start is called before the first frame update

    private void Awake()
    {
        instance = this;

        mother = FindObjectOfType<Mother>();

        GameManager.babies = FindObjectsOfType<Baby>();

        MakeAllBabiesFollowMother();

    }

    void Start()
    {
        //  DetermineBirdOrder();

        //CheckBabiesKinDistance();
    }

    private void MakeAllBabiesFollowMother()
    {
        Transform motherTransform = mother.transform;
        for (int i = 0; i < babies.Length; i++)
        {
            Baby baby = babies[i];
            baby.isAlive = true;
            baby.closestKin = motherTransform;
        }
    }

    
    private void FixedUpdate()
    {
        float deltaTime = Time.fixedDeltaTime;
        float time = Time.time;
        // CheckBabiesKinDistance();
        ManageBirds(ref time, ref deltaTime);
      
    }

    private void ManageBirds(ref float time, ref float deltaTime)
    {
        bool motherIsMoving = mother.IsMoving;

        for (int i = 0; i < babies.Length; i++)
        {
            Baby baby = babies[i];
            if (baby.isAlive)
            {
                if (baby.IsFrightened)
                {
                    baby.FrightendRoutine(ref deltaTime);
                }
                else
                {
                    if (motherIsMoving)
                    {
                        //The split is weird, also weird method names
                        baby.CheckForKinDistance(ref deltaTime);
                        baby.GoTowardsKin(ref deltaTime);
                    }
                    else
                    {
                        baby.IdleRoutine(ref time, ref deltaTime);
                    }
                }
            }

        }
    }


    private struct BirdDistanceData
    {
        // public Transform birdTransform;
        public Baby baby;
        public float distance;
    }

    private void DetermineBirdOrder()
    {
        float DetermineBirdOrderInterval = 1;
        Debug.LogError("Not Implemented!");
        int length = babies.Length;
        BirdDistanceData[] birdDistances = new BirdDistanceData[length];
        Vector3 motherPosition = mother.transform.position;
        for (int i = 0; i < length; i++)
        {
            birdDistances[i] = new BirdDistanceData
            { distance = Vector3.Distance(motherPosition, babies[i].transform.position), baby = babies[i] };
        }

        //Sort:
        for (int i = 0; i < length - 1; i++)
        {
            for (int j = i + 1; j < length; j++)
            {
                if (birdDistances[i].distance > birdDistances[j].distance)
                {
                    BirdDistanceData swapCopy = birdDistances[i];
                    birdDistances[i] = birdDistances[j];
                    birdDistances[j] = swapCopy;
                }
            }
        }
        // Debug.Log("Bird Order:");
        birdDistances[0].baby.closestKin = mother.transform;
        for (int i = 1; i < length; i++)
        {
            //  Debug.Log(birdDistances[i].distance);
            birdDistances[i].baby.closestKin = birdDistances[i - 1].baby.transform;
        }

        Invoke("DetermineBirdOrder", DetermineBirdOrderInterval);

    }


}
