using UnityEngine;
using Zenject;

public class UpgradeEquipmentsManager : MonoBehaviour
{
    [Inject] public  EquipmentManager _equipmentManager;
    [Inject] public  EventManager _eventManager;
    [Inject] public  Bank _bank;
   
    [SerializeField] private ButtonBoughtEquipment _buttonBoughtEquipmentPrefab;
    [SerializeField] private GameObject _panel;

    [SerializeField] private ButtonBoughtEquipment[] _buttonBoughtEquipments;

    public bool PanelIsOpened { get; private set; }

    private void Awake()
    {
        _eventManager.ScrollViewUpgradesActiveEvent += ChangeActivePanel;
    }

    private void ChangeActivePanel(bool isActive)
    {
        _panel.SetActive(isActive);

        if (isActive)
            CheckUpgrades();
    }
    public void ChangeActivatePanel()
    {
        if (PanelIsOpened)
            PanelIsOpened = false;
        else
            PanelIsOpened = true;
    }
    public void ActivePanelListBoughtUpgrages() // переделать, при выборе ресторана создаёт 
    {
        for (int i = 0; i < _panel.transform.childCount; i++)
            Destroy(_panel.transform.GetChild(i).gameObject);

        _buttonBoughtEquipments = new ButtonBoughtEquipment?[EquipmentManager.InfoRestaurant.EquipmentInfos.Length];

        for (int i = 0; i < _buttonBoughtEquipments.Length; i++)
        {
            var button = Instantiate(_buttonBoughtEquipmentPrefab, _panel.transform);
            button.GetEquipmentInfo(EquipmentManager.InfoRestaurant.EquipmentInfos[i], _eventManager);
            button.upgradeEquipments = this;
            _buttonBoughtEquipments[i] = button;
        }
    }
    private void Start()
    { 
        Invoke("ActivePanelListBoughtUpgrages", .1f);
    }
    private void CheckUpgrades()
{
        ActivePanelListBoughtUpgrages();

        for (int i = 0; i < _buttonBoughtEquipments.Length; i++)
        {
            if (_bank.CheckBoughtEquipmentInRestaurant(EquipmentManager.InfoRestaurant.name, _buttonBoughtEquipments[i].EquipmentInfo.name))
                _buttonBoughtEquipments[i].gameObject.SetActive(true);
            else
                _buttonBoughtEquipments[i].gameObject.SetActive(false);
        }
    }
    private void OnDestroy()
    {
        _eventManager.ScrollViewUpgradesActiveEvent -= ChangeActivePanel;
    }
}
