using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    //[SerializeField] private Transform[] birds;
    /*public static Transform[] Birds;*/
    //[SerializeField] private float DetermineBirdOrderInterval = 0.5f;
    private static Rodent[] rodents;
    private static Piper piper;

    private static GameManager instance;
    [SerializeField] private RodentsUI rodentsUI;
    [SerializeField] private Transform piperHead;

    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject victoryScene;


    private void Awake()
    {
        Initailise();
    }

    private void Initailise()
    {
        instance = this;

        piper = FindObjectOfType<Piper>();

        GameManager.rodents = FindObjectsOfType<Rodent>();

        MakeAllRodentsFollowPiper();

        UpdateNumberOfRodents();

        gameOverPanel.SetActive(false);
        winPanel.SetActive(false);
        victoryScene.SetActive(false);
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
               /* if (rodent.IsFrightened)
                {
                    rodent.FrightendRoutine(ref deltaTime);
                }
                else*/
                {
                    if (piperIsAlive)
                    {
                        if (piperIsPlaying)
                        {
                            if (piperIsMoving)
                            {
                                //The split is weird, also weird method names
                                rodent.ModifySpeed(ref deltaTime, ref piperPosition, 3);
                                rodent.WalkTowards(ref deltaTime, ref piperPosition, true);
                            }
                            else
                            {
                                rodent.IdleRoutine(ref time, ref deltaTime, ref piperPosition, 10f);
                            }
                        }
                        else //Piper is NOT playing
                        {
                            rodent.ModifySpeed(ref deltaTime, ref piperPosition, 0);
                            rodent.WalkTowards(ref deltaTime, ref piperPosition, false);
                            if (rodent.myTransform.position.y < piperPosition.y)
                            {
                                rodent.MightJump();
                            }
                        }

                    }
                    else
                    {
                        rodent.IdleRoutine(ref time, ref deltaTime, ref piperPosition, 1000f);
                    }
                }
            }
        }
    }
    #region Debugging:
    private void Update()
    { 
        if (Input.GetKeyDown(KeyCode.K))
        {
            KillAllRodents();
        }
    }

    private void KillAllRodents()
    {
        for (int i = 0; i < rodents.Length; i++)
        {
            Rodent rodent = rodents[i];
            if (rodent.isAlive)
            {
                rodent.ForceDeath();
            } 
        }
    }

    #endregion
    public static void OnRodentDeath()
    {
        UpdateNumberOfRodents();
    }

    public static void UpdateNumberOfRodents()
    {
        int relevantRodents = 0;
        for (int i = 0; i < rodents.Length; i++)
        {
            Rodent rodent = rodents[i];
            if (rodent.isAlive)// && !rodent.IsFrightened)
            {
                relevantRodents++;
            }
        }

        //Debug.LogWarning("This method should be updated.");
        instance.UpdateRodentsUI(relevantRodents);

        if (relevantRodents == 0)
        {
            instance.OnAllRodetsAreDead();
        }
    }

    public static void OnPiperPanic()
    {
        instance.HideRodentsUI();
    }

    private void HideRodentsUI()
    {
        rodentsUI.Hide();
    }

    private void UpdateRodentsUI(int numberOfRodents)
    {
        rodentsUI.UpdateText(numberOfRodents);
    }

    private void OnAllRodetsAreDead()
    {
        piper.Dance();
        OnWin();
    }

    public void RestartLevel()
    {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(sceneIndex);
    }

    public void NextLevel()
    {
        Debug.Log("NextLevel()");
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        int sceneCount = SceneManager.sceneCountInBuildSettings;
        if (nextSceneIndex >= sceneCount)//TODO: This trick ain't gonna work once we add non-level scenes, beware...
        {
            Debug.Log($"nextSceneIndex = {nextSceneIndex}, sceneCount = {sceneCount} , going back to 0");

            nextSceneIndex = 0;
        }
        SceneManager.LoadScene(nextSceneIndex);
    }

    public static void OnGameOver(float delay)
    {
        instance.Invoke("ShowGameOverScreen", delay);
    }

    public static void OnWin()
    {
        instance.winPanel.SetActive(true);
        instance.victoryScene.SetActive(true);

    }

    private void ShowGameOverScreen()
    {
        instance.gameOverPanel.SetActive(true);
    }
}
