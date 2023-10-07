using UnityEngine;
using Zenject;

public class EquipmentManager : MonoBehaviour
{
    [Inject] private readonly EventManager _eventManager;
    [Inject] private readonly EquipmentManager _equipmentManager;
    [Inject] private readonly Bank _bank;

    public PurchasePanel purchasePanel;
  
    [SerializeField] private EquipmentPlacesHolder _equpmentPlacesHolder;
    [SerializeField] private UpgradeEquipmentsManager _upgradeEquipmentsManager;
    public static RestaurantInfo InfoRestaurant { get; private set; }

    private float _priceUpgradeEquipment;

    //public EventManager GetEventManager()
    //{
    //    return _eventManager;
    //}
    /// <summary>
    /// Покупка улучшения выбранного оборудования, с проверкой на достаточность золота
    /// </summary>
    public void BuyUpdateEquipment()
    {
        if (Checks.CheckEnoughGoldForBuyUpdateEquipment(_bank.GetNumberUpgradesEquipmentsInRestaurant(InfoRestaurant.name), _bank))
        {
            _eventManager.ChangeValueGold(-_priceUpgradeEquipment);
            UpgradeEquipment();
            _eventManager.ChangeValueDimond(EquipmentImprovementSystem.DimondsLevelEquipmentReward);
        }
    }
    /// <summary>
    /// Покупка оборудования
    /// </summary>
    public void BuyEquipment()
    {
        if (Checks.CheckEmptyEquipmentSlot(InfoRestaurant.MaxNumberEquipmentPlaces, InfoRestaurant.name, _bank)
            && Checks.CheckEnoughGoldForBuyEquipment(PurchasePanel.SelectedEquipmentPlace.equipmentInfo.Price, _bank) 
            && Checks.CheckDuplicationBoughtEquipment(_bank))
        {
            var info = PurchasePanel.SelectedEquipmentPlace.equipmentInfo;

            if (!Checks.CheckFirstBuyEquipment(_bank))
            {
                _eventManager.ChangeValueGold(-info.Price);
                _bank.RegisterRestaurantInvestments(InfoRestaurant.name, info.Price);
            }

            _bank.RegisterPurchasedTypesEquipmentInRestaurant(InfoRestaurant.name, (int)info.EquipmentTypes);
            Debug.Log($"купил оборудование {(int)info.EquipmentTypes} в ресторан {InfoRestaurant.name}");
            _bank.RegisterBoughtEquipmentInRestaurant(InfoRestaurant.name, PurchasePanel.SelectedEquipmentPlace.equipmentInfo.name);
        }
    }
    /// <summary>
    /// Получение инфы о выбранном ресторане
    /// </summary>
    /// <param name="info"></param>
    public void GetInfoRestaurant(RestaurantInfo info)
    {
        InfoRestaurant = info;
        _equpmentPlacesHolder.CreateEquipmentPlaces(info.EquipmentInfos, info.SpawnPositions);
    }
    /// <summary>
    /// Получение стоимости улучшения выбранного оборудования
    /// </summary>
    /// <param name="price"></param>
    public void GetPriceUpdateEquipment(float price)
    {
        _priceUpgradeEquipment = price;
    }
    public void UpgradeEquipment()
    {
        var place = PurchasePanel.SelectedEquipmentPlace;
        place.ChangeLevel(1);//добавил сублевел в выбранное оборудование
        _bank.RegisterSublevelEquipment(place.equipmentInfo.name, place.Sublevel);//сохранил уровень в выбранном оборудовании
        _bank.RegisterRestaurantInvestments(InfoRestaurant.name, _priceUpgradeEquipment);//прибавляет цену улучшения к инвестициям и сохраняет
        _bank.RegisterNumberUpgradesEquipmentsInRestaurant(InfoRestaurant.name);
    }
}
