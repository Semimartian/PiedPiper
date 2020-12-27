using System;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class Window : MonoBehaviour
{
    public Action Opened = delegate{ }; 
    public Action Closed = delegate{ };

    private CanvasGroup _canvasGroup;
    private bool        _isOpened;

    private const float _animationSpeed = 3f;
    private float       _animationTimer = 0f;

    public bool openOnStart   = false;
    public bool skipAnimation = false;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();

        if ( openOnStart )
            _isOpened = true;
    }

    public bool IsOpened
    {
        get
        {
            return _isOpened;
        }
    }

    public void Open()
    {
        _canvasGroup.interactable = true;
        _canvasGroup.blocksRaycasts = true;
        _isOpened = true;
        _animationTimer = ( skipAnimation ? 1f : 0f );

        Opened.Invoke();
    }

    public void Open( float delay )
    {
        _isOpened       = true;
        _animationTimer = delay * -_animationSpeed;
    }

    public void Close()
    {
        _canvasGroup.interactable   = false;
        _canvasGroup.blocksRaycasts = false;
        _isOpened = false;
        _animationTimer = ( skipAnimation ? 0f : 1f );

        Closed.Invoke();
    }

    public void Close( float delay )
    {
        Close();
        _animationTimer = 1 + delay * _animationSpeed;
    }

    private void Update()
    {
        _animationTimer += Time.deltaTime * ( _isOpened ? _animationSpeed : -_animationSpeed );
        _canvasGroup.alpha = Mathf.Lerp( 0f, 1f, _animationTimer );

        if ( _isOpened && !_canvasGroup.interactable && _canvasGroup.alpha == 1f ) {
            _canvasGroup.interactable   = true;
            _canvasGroup.blocksRaycasts = true;
        }

        if ( PlayerPrefs.GetInt( "HUD_Hidden", 0 ) == 1 )
            _canvasGroup.alpha = 0;
    }
}
