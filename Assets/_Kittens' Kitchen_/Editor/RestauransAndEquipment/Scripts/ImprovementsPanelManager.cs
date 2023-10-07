using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class ImprovementsPanelManager : MonoBehaviour
{
    [Inject] private readonly EquipmentManager _equipmentManager;
    [Inject] private readonly EventManager _eventManager;
    [Inject] private readonly Bank _bank;

    [SerializeField] private Text _priceText;
    [SerializeField] private Text _sublevel;
    [SerializeField] private Text _levelText;
    [SerializeField] private Text _priceProductText;

    public GameObject panel;

    private void Awake()
    {
        _eventManager.ChangeActiveImprovementsPanelEvent += ChangeActiveImprovementsPanel;
        _eventManager.UpdateTextImprovementsPanelEvent += CalculateData;
    }
    private void ChangeActiveImprovementsPanel(bool isActive)
    {
        if (PurchasePanel.SelectedEquipmentPlace.equipmentInfo != null)
        {
            panel.SetActive(isActive);
            CalculateData();
        }
    }
    private void CalculateData()
    {
        float price = EquipmentImprovementSystem.StartingPriceImprovement +
            EquipmentImprovementSystem.StartingPriceImprovement * EquipmentImprovementSystem.PercentIncreasePriceEquipmentAfterImprovement / 100
            * _bank.GetNumberUpgradesEquipmentsInRestaurant(EquipmentManager.InfoRestaurant.name);

        var place = PurchasePanel.SelectedEquipmentPlace;

        int subLevel = place.Sublevel % 10;
        int level = place.Sublevel / 10;
        level++;

        UpdateText(place, price, subLevel, level);

        _equipmentManager.GetPriceUpdateEquipment(price);
    }
    private void UpdateText(EquipmentPlace place, float price, int sublevel, int level)
    {
        _priceText.text = $"{(int)price}";
        _sublevel.text = $"{sublevel}";
        _levelText.text = $"{level}";

        _priceProductText.text =
            $"{PurchasePanel.SelectedEquipmentPlace.equipmentInfo.PriceProduct + place.Sublevel * EquipmentImprovementSystem.IncreasePriceProductAfterImprovement} " +
            $"+ {EquipmentImprovementSystem.IncreasePriceProductAfterImprovement}";
    }
    private void OnDestroy()
    {
        _eventManager.ChangeActiveImprovementsPanelEvent -= ChangeActiveImprovementsPanel;
        _eventManager.UpdateTextImprovementsPanelEvent -= CalculateData;
    }
}
