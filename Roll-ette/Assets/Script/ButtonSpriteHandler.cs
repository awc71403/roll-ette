using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonSpriteHandler : MonoBehaviour, IPointerUpHandler, IPointerExitHandler, IPointerDownHandler
{
    [SerializeField]
    private Sprite unpressed;

    [SerializeField]
    private Sprite pressed;

    [SerializeField]
    private Image image;

    [SerializeField]
    private Sprite disabled;

    private bool isDisabled;


    void Awake()
    {
        image.sprite = unpressed;
    }

    public void OnPointerUp(PointerEventData data)
    {
        if (!isDisabled)
        {
            image.sprite = unpressed;
            print("up");
        }
    }

    public void OnPointerDown(PointerEventData data)
    {
        if (!isDisabled)
        {
            image.sprite = pressed;
            print("down");
        }
    }

    public void OnPointerExit(PointerEventData data)
    {
        if (!isDisabled)
        {
            image.sprite = unpressed;
            print("exit");
        }
    }

    public void setDisabled(bool state)
    {
        isDisabled = state;
        if (state)
        {
            image.sprite = disabled;
        } else
        {
            image.sprite = unpressed;
        }
    }
}
