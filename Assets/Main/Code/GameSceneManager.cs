using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneManager : MonoBehaviour
{

    [SerializeField] private Animator fenceAnimator;
    private int sceneIndex = 0;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            AdvanceScene();
           
        }
    }


    public void AdvanceScene()
    {
        switch (sceneIndex)
        {
            case 0:
                {
                    fenceAnimator.SetTrigger("Open");
                }
                break;

            case 1:
                {
                    StartCoroutine(ReleaseButterflies());
                }
                break;
        }
        sceneIndex++;
    }

   [SerializeField] private ParticleSystem[] butterflyEffects;
    private IEnumerator ReleaseButterflies()
    {
        for (int i = 0; i < butterflyEffects.Length; i++)
        {
            butterflyEffects[i].Play();
            yield return new WaitForSeconds(0.35f);
        }
    }
}
