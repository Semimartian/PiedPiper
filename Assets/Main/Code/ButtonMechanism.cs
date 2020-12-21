using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonMechanism : MonoBehaviour
{
    public UnityEvent OnButtonPressed;
    public UnityEvent OnButtonUnPressed;

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.GetComponentInParent<Mother>())
        {
            Debug.Log("Button Pressed!");

            OnButtonPressed.Invoke();
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.GetComponentInParent<Mother>())
        {
            Debug.Log("Button UnPressed!");
            OnButtonUnPressed.Invoke();
        }
    }    

}
