using UnityEngine;
using Zenject;

public class RestaurantsManager : MonoBehaviour
{
    [Inject] private readonly EventManager eventManager;
    [Inject] private readonly EquipmentManager _equipmentManager;
    [Inject] private readonly RestaurantsManager restaurantsManager;
    [Inject] private readonly Bank bank;

    [SerializeField] private ButtonRestaurant[] _totalRestaurants;//все рестораны
    [SerializeField] private PanelInfoRestaurant _panelBuyRestaurant;

    public ButtonRestaurant SelectedButtonRestaurant { get; private set; }
    private void Awake()
    {
        eventManager.SelectRestaurantEvent += SelectRestaurant;
        eventManager.BuyRestaurantEvent += BuyRestaurant;
        
        LoadSelectedRestaurant();
    }
    private void Start()
    {
        LoadSelectedRestaurant();
    }
    /// <summary>
    /// Загрузка выбранного ресторана или дефолтного и переход в него 
    /// </summary>
    private void LoadSelectedRestaurant()
    {
        if (SelectedButtonRestaurant == null)
        {
            SelectedButtonRestaurant = _totalRestaurants[bank.LastSelectedRestaurant];
            _equipmentManager.GetInfoRestaurant(SelectedButtonRestaurant.Info);
        }

        if (bank.BoughtRestaurants.Count == 0)
            bank.AddBughtRestaurant(SelectedButtonRestaurant.name);
    }
    /// <summary>
    /// Покупка ресторана с проверкой
    /// </summary>
    private void BuyRestaurant()
    {
        if (SelectedButtonRestaurant != null && !bank.CheckBoughtRestaurant(restaurantsManager.SelectedButtonRestaurant.name)
            && Checks.CheckEnoughGold(SelectedButtonRestaurant.Info.Price, bank))
        {
            bank.BoughtRestaurants.Add(SelectedButtonRestaurant.name);
            eventManager.ChangeValueGold(-SelectedButtonRestaurant.Info.Price);
            bank.RegisterRestaurantInvestments(SelectedButtonRestaurant.Info.name, SelectedButtonRestaurant.Info.Price);
        }
    }
    /// <summary>
    /// Создание и сохранение данных о Купленных ресторанах
    /// </summary>
    public void SellRestaurant()
    {
        if (bank.CheckNumberBoughtRestaurants() > 1 && bank.CheckBoughtRestaurant(restaurantsManager.SelectedButtonRestaurant.name))
        {
            string nameRestaurant = SelectedButtonRestaurant.Info.name;
            float gold = bank.GetRestaurantInvestments(nameRestaurant);
            bank.UnRegisterRestaurantInvestments(nameRestaurant);
            bank.UnRegisterSubLevelEquipment(nameRestaurant);
            bank.UnRegisterNumberUpgradesEquipmentsInRestaurant(nameRestaurant);
            bank.UnRegisterBoughtEquipmentInRestaursnt(nameRestaurant);
            bank.UnRegisterPurchasedTypesEquipmentInRestaurant(nameRestaurant);
            bank.UnRegisterPossibleUpgradesRestaurant(nameRestaurant);
            bank.UnAllBoughtUpgradesInRestaurant(nameRestaurant);
            bank.RemoveBughtRestaurant(nameRestaurant);
            bank.UnRegisterDataBoughtUpgradesInRestaurant(nameRestaurant);

            eventManager.ChangeValueGold(gold);

            bank.BoughtRestaurants.Remove(SelectedButtonRestaurant.name);
        }
    }
    /// <summary>
    /// Получение информации о выбранном ресторане
    /// </summary>
    public RestaurantInfo GetInfoRestaurant()
    {
        var info = SelectedButtonRestaurant.Info;

        return info;
    }
    public void GetInfoToPanel(RestaurantInfo info)
    {
        _panelBuyRestaurant.GetInfo(info);
    }
    private void SelectRestaurant(ButtonRestaurant buttonRestaurant)
    {
        if (buttonRestaurant != null)
        {
            SelectedButtonRestaurant = buttonRestaurant;
            int number = buttonRestaurant.transform.GetSiblingIndex();
            bank.ChangeNumberLastSelectedRestaurant(number);
        }
    }
    private void OnDestroy()
    {
        eventManager.SelectRestaurantEvent -= SelectRestaurant;
        eventManager.BuyRestaurantEvent -= BuyRestaurant;
    }
}
