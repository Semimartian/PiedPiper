using System;
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
    [SerializeField] private Transform piperHead;

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
        }
    }
    
    private void FixedUpdate()
    {
        float deltaTime = Time.fixedDeltaTime;
        float time = Time.time;
        // CheckBabiesKinDistance();
        ManageRodents(ref time, ref deltaTime);
      
    }

    private void ManageRodents(ref float time, ref float deltaTime)
    {
        //Bad programming ahead:
        bool piperIsAlive = piper.IsAlive;
        bool piperIsMoving =  piper.IsMoving;
        bool piperIsPlaying = piper.IsPlaying;
        Vector3 piperPosition =// piper.myTransform.position;
            piperHead.position;

        //Debug.Log("piperIsMoving: " + piperIsMoving);
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
                    if (piperIsAlive)
                    {
                        if (!piperIsPlaying)
                        {
                            rodent.ModifySpeed(ref deltaTime, ref piperPosition, 0);
                            rodent.WalkTowards(ref deltaTime, ref piperPosition, false);
                            if (rodent.myTransform.position.y < piperPosition.y)
                            {
                                rodent.MightJump();
                            }
                        }
                        else if(piperIsMoving)
                        {
                            //The split is weird, also weird method names
                            rodent.ModifySpeed(ref deltaTime, ref piperPosition, 1);
                            rodent.WalkTowards(ref deltaTime, ref piperPosition, true);
                        }
                        else
                        {

                            rodent.IdleRoutine(ref time, ref deltaTime, ref piperPosition, 2.5f);
                        }
                    }
                    else
                    {
                        rodent.IdleRoutine(ref time, ref deltaTime, ref piperPosition, 50);

                    }

                }
            }

        }
    }

    internal static void OnPiperDeath()
    {
        throw new NotImplementedException();
    }

    public static void OnChickDeath()
    {
        Debug.LogWarning("This method should be updated.");
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
