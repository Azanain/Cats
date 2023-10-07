using System;
using System.Collections;
using UnityEngine;
using _Kittens__Kitchen_.Editor.Production.Scripts.Upgrades.Voilt;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Zenject;
using Button = UnityEngine.UI.Button;

public class VoiltShop : MonoBehaviour
{
    [Inject] private readonly Bank _bank;
    [Inject] private readonly EventManager _eventManager;
    
    [SerializeField] private Transform content;
    [SerializeField] private VoiltView voiltPrefab;
    [SerializeField] private Button buyButton;
    [SerializeField] private Text costUpgradeText;
    [SerializeField] private VoiltsInfo voiltsInfo;

    private VoiltData _currentVoilt;
    private VoiltView _selectedVoiltView;
    private event Action BuyButtonUpdated;

    private void Awake()
    {
        _eventManager.ChangeValueGoldEvent += UpdateBuyButton;
        buyButton.onClick.AddListener(CallBackBuyButton);
        BuyButtonUpdated += InvokeUpdateBuyButton;
    }

    private void OnEnable()
    {
        InitializeVoilts();
        UpdateBuyButton();
    }

    private void OnDisable()
    {
        DestroyAllChildren();
        
        _eventManager.ChangeValueGoldEvent -= UpdateBuyButton;
        buyButton.onClick.RemoveListener(CallBackBuyButton);
        BuyButtonUpdated -= InvokeUpdateBuyButton;
    }

    private void InitializeVoilts()
    {
        RefreshVoiltItems();
        UpdateBuyButton();
    }

    private void CallBackBuyButton()
    {
        OnBuyButtonClick();
        DeselectVoilt();
    }

    private void OnBuyButtonClick()
    {
        if (!IsVoiltSelected())
        {
            Debug.LogWarning("Voilt is not selected");
            DeselectVoilt();
            return;
        }
        
        float voiltCost = _currentVoilt.Cost;
        bool isEnoughCurrency = Checks.CheckEnoughGold(voiltCost, _bank);
        
        bool isMaxLevelVoilt = _currentVoilt.IsMaxLevel();

        if (!isEnoughCurrency)
        {
            Debug.LogWarning("Not enough currency!");
            return;
        }
        
        voiltsInfo.PurchasedVoilts.Add(_currentVoilt);
        _eventManager.ChangeValueGold(-voiltCost);

        if (!isMaxLevelVoilt)
            _currentVoilt.CurrentLevel++;

        RefreshVoiltItems();
    }

    private void DeselectVoilt()
    {
        bool isMaxLevelVoilt = _currentVoilt.IsMaxLevel();
        
        if (!isMaxLevelVoilt) return;

        _currentVoilt = null;
        buyButton.interactable = false;
        costUpgradeText.text = "Select upgrade!";
    }

    private bool IsVoiltSelected()
    {
        bool voiltSelected = _currentVoilt != null;

        return voiltSelected;
    }

    private void CreateVoilt(VoiltData voiltData)
    {
        VoiltView newVoiltView = Instantiate(voiltPrefab, content.position, Quaternion.identity, content.transform);
        newVoiltView.Initialize(this);

        if (newVoiltView != null)
        {
            newVoiltView.DisplayVoilt(voiltData);
        }

        Voilt newVoilt = null;
        switch (voiltData.Type)
        {
            case VoiltTypes.Tips:
                newVoilt = newVoiltView.gameObject.AddComponent<Tips>();
                break;
            case VoiltTypes.Investments:
                newVoilt = newVoiltView.gameObject.AddComponent<Investments>();
                break;
            case VoiltTypes.BagInvestor:
                newVoilt = newVoiltView.gameObject.AddComponent<BagInvestor>();
                break;
            case VoiltTypes.SuitCase:
                newVoilt = newVoiltView.gameObject.AddComponent<SuitCase>();
                break;
            case VoiltTypes.WildPig:
                newVoilt = newVoiltView.gameObject.AddComponent<WildPig>();
                break;
        }
        
        if (newVoilt != null)
            newVoilt.Initialize(voiltData.Type, newVoiltView, voiltData);
    }

    private void RefreshVoiltItems()
    {
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }

        foreach (var voilt in voiltsInfo.AllVoilts)
        {
            if (!voilt.IsMaxLevel()) 
                CreateVoilt(voilt);
        }
    }
    
    private void DestroyAllChildren()
    {
        int childCount = content.childCount;

        for (int i = childCount - 1; i >= 0; i--)
        {
            Destroy(content.GetChild(i).gameObject);
        }
    }

    private void InvokeUpdateBuyButton()
    {
        UpdateBuyButton();
    }

    private void UpdateBuyButton(float gold = 0)
    {
        bool voiltNull = _currentVoilt == null;
        
        if (voiltNull) return;
        
        if (IsVoiltSelected())
            costUpgradeText.text = _currentVoilt.Cost.ToString();
        
        bool enoughCurrency = Checks.CheckEnoughGold(_currentVoilt.Cost, _bank);

        buyButton.interactable = enoughCurrency;
    }

    public void SetSelectedVoilt(VoiltData selectedVoilt, VoiltView selectedVoiltView)
    {
        if (_selectedVoiltView != null)
        {
            _selectedVoiltView.SetNotPressed();
            _selectedVoiltView = null;
        }

        if (_selectedVoiltView != selectedVoiltView)
        {
            _selectedVoiltView = selectedVoiltView;
            _selectedVoiltView.SetPressed();
        }
        else
        {
            _selectedVoiltView = null;
        }

        _currentVoilt = selectedVoilt;
        BuyButtonUpdated?.Invoke();
    }
}
