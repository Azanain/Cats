using System;
using _Kittens__Kitchen_.Editor.Production.Scripts.Upgrades.Voilt;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

[RequireComponent(typeof(Image))]
public class VoiltView : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private Image icon;
    [SerializeField] private Text level;
    
    private static bool _isSelected;

    public const float PRESSED = 0.5f;
    public const float NOT_PRESSED = 1f;
    
    private VoiltData _currentData;
    private VoiltShop _voiltShop;

    public void Initialize(VoiltShop voiltShop)
    {
        _voiltShop = voiltShop;
    }

    public void DisplayVoilt(VoiltData voilt)
    {
        _currentData = voilt;
        icon.sprite = voilt.Icon;
        level.text = "Level " + voilt.CurrentLevel;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _voiltShop.SetSelectedVoilt(_currentData, this);
    }

    public void SetPressed()
    {
        SetVisible(PRESSED);
    }

    public void SetNotPressed()
    {
        SetVisible(NOT_PRESSED);
    }

    private void SetVisible(float constVisible)
    {
        foreach (var image in gameObject.GetComponentsInChildren<Image>())
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, constVisible);
        }
    }

    private void OnDestroy()
    {
        _isSelected = false;
    }
}
