using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonBuyUpgradeRestaurant : MonoBehaviour, IPointerClickHandler
{
    [Header("ButtonData")]
    [SerializeField] private Button _button;
    [SerializeField] private ButtonSelectUpgradeRestaurant _buttonSelectUpgradeResturant;
    private UpgradesRestaurantManager _upgradesRestaurantManager;
   
    private Bank _bank;
    private UpgradeRestaurantInfo _upgradeRestaurantInfo;
    private bool _canPress;
    private EventManager EventManager => _buttonSelectUpgradeResturant._eventManager;

    public void GetData(UpgradesRestaurantManager upgradesRestaurantManager, Bank bank, UpgradeRestaurantInfo info)
    {
        if(_upgradesRestaurantManager == null)
            _upgradesRestaurantManager = upgradesRestaurantManager;

        if(_bank == null)
            _bank = bank;

        _upgradeRestaurantInfo = info;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (_canPress && !_bank.CheckBoughtEquipmentInRestaurant(EquipmentManager.InfoRestaurant.name, _upgradeRestaurantInfo.name))
        {
            _buttonSelectUpgradeResturant.SelectUpgrade();
            _upgradesRestaurantManager.BuyUpgrade(_upgradeRestaurantInfo);
            ChangeActiveButton();
        }
    }
    private void Start()
    {
        if(EventManager != null)
            EventManager.ChangeValueGoldEvent += GoldAdded;
    }
    private void GoldAdded(float value)
    {
        if(value > 0)
            ChangeActiveButton();
    }
    private void OnEnable()
    {
        ChangeActiveButton();
    }
    public void ChangeActiveButton()
    {
        if (Checks.CheckEnoughGold(_upgradesRestaurantManager.CheckPrice(_upgradeRestaurantInfo), _bank) 
            && _bank.CheckPurchasedTypesEquipmentInRestaurant(EquipmentManager.InfoRestaurant.name, (int)_upgradeRestaurantInfo.TypeUpgrade))
        {
            _canPress = true;
            _button.interactable = true;
        }
        else
        {
            _canPress = false;
            _button.interactable = false;
        }
    }
    private void OnDisable()
    {
        if (EventManager != null)
            EventManager.ChangeValueGoldEvent -= GoldAdded;
    }
}
