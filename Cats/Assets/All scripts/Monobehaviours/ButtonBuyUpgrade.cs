using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonBuyUpgrade : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private UpgradeManager _upgradeManager;
    [SerializeField] private Button _button;

    private InfoUpgrade _infoUpgrade => _upgradeManager.InfoUpgrade;
    private EventManager _eventManager;
    private bool _canBuy;
    private int _numberSelectedUpgrade;
    private void Awake()
    {
        if (_upgradeManager == null)
            _upgradeManager = GameObject.FindGameObjectWithTag("GameData").GetComponentInChildren<UpgradeManager>();

        if (_button == null)
            _button = GetComponent<Button>();

        _eventManager = _upgradeManager.eventManager;

        _eventManager.CheckActiveButtonButEvent += CheckValueCoins;
        _eventManager.UpgradeTextInfoTypesEvent += CheckValueCoins;
        _eventManager.SelectUpdateEvent += SelectNumberEvent;
    }
    private void Start()
    {
        _button.interactable = false;
    }
    private void SelectNumberEvent(int number)
    {
        _numberSelectedUpgrade = number;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (_canBuy)
        {
            _eventManager.SpendCoins(_infoUpgrade.Cost);
            _eventManager.DeleteUpgrade(_numberSelectedUpgrade);
        }
    }
    private void CheckValueCoins()
    {
        if (_infoUpgrade != null)
        {
            if (_infoUpgrade.Cost <= _upgradeManager.Coins)
            {
                _button.interactable = true;
                _canBuy = true;
            }
            else
            {
                _button.interactable = false;
                _canBuy = false;
            }
        }
    }
    private void OnDestroy()
    {
        _eventManager.CheckActiveButtonButEvent -= CheckValueCoins;
        _eventManager.UpgradeTextInfoTypesEvent -= CheckValueCoins;
        _eventManager.SelectUpdateEvent -= SelectNumberEvent;
    }
}
