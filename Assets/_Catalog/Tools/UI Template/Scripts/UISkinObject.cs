using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UISkinObject : MonoBehaviour
{
    public Component                  ColorObject;
    public UISkinManager.UiObjectType Type;

    private Color    _color;
    private Image    _image;
    private TMP_Text _text;
    private bool     _isImage;

    void Start()
    {
        _color = UISkinManager.ObjectColors[Type];

        if (ColorObject is Image)
        {
            _image       = ColorObject as Image;
            _image.color = _color;
            _isImage     = true;
        }
        else if(ColorObject is TMP_Text)
        {
            _text        = ColorObject as TMP_Text;
            _text.color  = _color;
            _isImage     = false;
        }
    }

    void Update()
    {
        if (UISkinManager.Instance.AlwaysUpdate)
        {
            _color = UISkinManager.ObjectColors[Type];

            if(_isImage)
                _image.color = _color;
            else
                _text.color  = _color;
        }
    }
}
