using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //[SerializeField] private Transform[] birds;
    /*public static Transform[] Birds;*/
    //[SerializeField] private float DetermineBirdOrderInterval = 0.5f;
    private static Rodent[] rodents;
    private static Piper piper;

    private static GameManager instance;
    [SerializeField] private ChicksUI chicksUI;

    // Start is called before the first frame update

    private void Awake()
    {
        instance = this;

        piper = FindObjectOfType<Piper>();

        GameManager.rodents = FindObjectsOfType<Rodent>();

        MakeAllRodentsFollowPiper();

        UpdateChicksUI();
    }

    private void MakeAllRodentsFollowPiper()
    {
        Transform motherTransform = piper.transform;
        for (int i = 0; i < rodents.Length; i++)
        {
            Rodent rodent = rodents[i];
            rodent.isAlive = true;
            rodent.followTarget = motherTransform;
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
        bool piperIsMoving = piper.IsMoving;

        for (int i = 0; i < rodents.Length; i++)
        {
            Rodent rodent = rodents[i];
            if (rodent.isAlive)
            {
                if (rodent.IsFrightened)
                {
                    rodent.FrightendRoutine(ref deltaTime);
                }
                else
                {
                    if (piperIsMoving)
                    {
                        //The split is weird, also weird method names
                        rodent.CheckForKinDistance(ref deltaTime);
                        rodent.GoTowardsKin(ref deltaTime);
                    }
                    else
                    {
                        rodent.IdleRoutine(ref time, ref deltaTime);
                    }
                }
            }

        }
    }

    public static void OnChickDeath()
    {
       instance.UpdateChicksUI();
    }

    private void UpdateChicksUI()
    {
        int relevantChicks = 0;
        for (int i = 0; i < rodents.Length; i++)
        {
            Rodent rodent = rodents[i];
            if (rodent.isAlive && !rodent.IsFrightened)
            {
                relevantChicks++;
            }
        }

        chicksUI.UpdateText(relevantChicks);
    }
}
