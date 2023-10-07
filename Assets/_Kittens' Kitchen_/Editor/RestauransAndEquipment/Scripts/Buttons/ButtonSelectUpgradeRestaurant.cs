using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonSelectUpgradeRestaurant : MonoBehaviour, IPointerClickHandler
{
    [HideInInspector] public EventManager _eventManager;
    [HideInInspector] public Bank _bank;
    [HideInInspector] public UpgradesRestaurantManager _upgradesRestaurantManager;

    [SerializeField] private ButtonBuyUpgradeRestaurant _buttonBuy;

    [Header("UI")]
    [SerializeField] private Image _imageUpgrade;
    [SerializeField] private Text _title;
    [SerializeField] private Text _decription;
    [SerializeField] private Text _price;

    public UpgradeRestaurantInfo UpgradeRestaurantInfo { get; private set; }

    [SerializeField] private int _increasePriceUpgradeRestaurant = 0;
    private void Start()
    {
        _eventManager.UpgradeRestaurantIsBoughtEvent += UpdateTextOnEvent;
    }
    private void UpdateTextOnEvent(EquipmentTypes type)
    {
        UpdateText();
    }
    public void GetDataOnCreate(UpgradesRestaurantManager upgradesRestaurantManager, EventManager eventManager, Bank bank, UpgradeRestaurantInfo info)
    {
        if(_upgradesRestaurantManager == null)
            _upgradesRestaurantManager = upgradesRestaurantManager;

        if(_eventManager == null)
            _eventManager = eventManager;

        if(_bank == null)
            _bank = bank;

        UpgradeRestaurantInfo = info;

        UpdateText();

        _buttonBuy.GetData(upgradesRestaurantManager, bank, info);
    }
    public void UpdateText()
    { 
        if (UpgradeRestaurantInfo != null)
        {
            _imageUpgrade.sprite = UpgradeRestaurantInfo.Sprite;
            _title.text = UpgradeRestaurantInfo.Title;
            _price.text = _upgradesRestaurantManager.CheckPrice(UpgradeRestaurantInfo).ToString();
            _decription.text = UpgradeRestaurantInfo.Description;
        }
    }
    public void SelectUpgrade()
    {
        if (UpgradeRestaurantInfo != null)
            _upgradesRestaurantManager.SelectUpgrade(UpgradeRestaurantInfo);
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        SelectUpgrade();
    }
    private void OnDestroy()
    {
        _eventManager.UpgradeRestaurantIsBoughtEvent -= UpdateTextOnEvent;
    }
}
