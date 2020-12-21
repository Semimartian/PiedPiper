using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ToggleButton : MonoBehaviour
{
    public static Action<bool> Toggled = delegate{ };

    private Button _button;
    public Image   image;

    public Sprite activeSprite;
    public Sprite inactiveSprite;

    private bool _currentlyActive = true;

    private void Awake()
    {
        _button = GetComponent<Button>();

        RefreshSprite();

        _button.onClick.AddListener( OnButtonClicked );
    }

    private void OnDestroy()
    {
        _button.onClick.RemoveListener( OnButtonClicked );
    }

    private void OnButtonClicked()
    {
        _currentlyActive = !_currentlyActive;
        RefreshSprite();

        Toggled.Invoke( _currentlyActive );
    }

    private void RefreshSprite()
    {
        image.sprite = ( _currentlyActive ? activeSprite : inactiveSprite );
    }
}
