using System.Collections.Generic;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(ButtonSelectUpgradeRestaurantPool))]
public class UpgradesRestaurantManager : MonoBehaviour
{
    [HideInInspector] [Inject] public EventManager eventManager;
    [HideInInspector] [Inject] public EquipmentManager equipmentManager;
    [HideInInspector] [Inject] public Bank bank;

    [SerializeField] private GameObject _panel;
    [SerializeField] private ButtonSelectUpgradeRestaurantPool _pool;
    private UpgradeRestaurantInfo _selectedUpgradeRestaurantInfo;

    private UpgradeRestaurantInfo[] _allUpgrades;
    private string[] _allUpgradeNames;
    private void Awake()
    {
        eventManager.ConfirmSelectingRestaurantAndgoToItEvent += RestaurantIsChanged;
        eventManager.ChangeActivePanelUpgradeRestaurantEvent += ChangeActivePanel;

        if (_pool == null)
            _pool = GetComponent<ButtonSelectUpgradeRestaurantPool>();
    }
    private void Start()
    {
        RestaurantIsChanged();
    }
    /// <summary>
    /// Действие при смене ресторана
    /// </summary>
    /// <param name="restaurant"></param>
    private void RestaurantIsChanged()
    {
        CheckCreationListPossibleUpgrades();
    }
    /// <summary>
    /// При открытии панели улучшений   
    /// </summary>
    /// <param name="isAcive"></param>
    public void ChangeActivePanel(bool isAcive)
    {
        _panel.SetActive(isAcive);
    }
    /// <summary>
    /// Проверяет создан ли лист улучшений для этого ресторана
    /// </summary>
    private void CheckCreationListPossibleUpgrades()
    {
        if (!bank.CheckPossibleUpgradesRestaurant(EquipmentManager.InfoRestaurant.name))
            CreateDictionaryUpgrades();
        
        PrepareAllUpdatesForButtonsAndCreate();

        CheckNeedCreateButtons();
    }
    /// <summary>
    /// Проверка надобности создания новых кнопок или отключение лишних
    /// </summary>
    private void CheckNeedCreateButtons()
    {
        if (_pool.pool.Count < _allUpgradeNames.Length)//если колво созданных кнопок меньше нужного
        {
            int missingNumber = _allUpgradeNames.Length - _pool.pool.Count;

            _pool.Initialize(missingNumber, true);
            
            for (int i = 0; i < _pool.pool.Count; i++)
            {
                _pool.pool[i].GetDataOnCreate(this, eventManager, bank, _allUpgrades[i]);
                _pool.pool[i].gameObject.SetActive(true);
            }
        }
        else
        {
            _pool.Initialize(_allUpgrades.Length, false);

            for (int i = 0; i < _pool.pool.Count; i++)
                if (i > _allUpgradeNames.Length - 1)
                    _pool.pool[i].gameObject.SetActive(false);

            int numberUpgrade = 0;

            foreach (var item in _pool.pool)
            {
                if(item.gameObject.activeSelf)
                { 
                    item.GetDataOnCreate(this, eventManager, bank, _allUpgrades[numberUpgrade]);
                    numberUpgrade++;
                    item.gameObject.SetActive(true);
                }
            }
        }
    }
    /// <summary>
    /// Создание словаря доступных улучшений для этого ресторана 
    /// </summary>
    private void CreateDictionaryUpgrades()
    {
        UpgradeRestaurantInfo[] allUpgrades = Resources.LoadAll<UpgradeRestaurantInfo>("UpgradesRestaurant");//берётся массив всех улучшений

        List<EquipmentTypes> allTypesUpgrades = new(EquipmentManager.InfoRestaurant.AllTypesEquipmentsInRestautant);//список всего оборудования в ресторане

        for (int i = 0; i < allUpgrades.Length; i++)//перебор каждого улучшения на наличие типа
        {
            if (allTypesUpgrades.Contains(allUpgrades[i].TypeUpgrade))//если соответствует, то добавляется в банк доступных улучшений этого ресторана
                bank.RegisterPossibleUpgradesRestaurant(EquipmentManager.InfoRestaurant.name, allUpgrades[i].name);
        }
    }
    public int CheckPrice(UpgradeRestaurantInfo info)
    {
        int price = int.MaxValue;

        if (info != null)
        {
            int numberUpgradeOnType = bank.GetNumberAllUpgradeTypeInrestaurant(EquipmentManager.InfoRestaurant.name, info.TypeUpgrade);
            int infoPrice = info.Price;
            int increasePrice = infoPrice +
                numberUpgradeOnType * info.PriceIncrease;

            price = infoPrice + increasePrice;
        }
    
        return price;
    }
    /// <summary>
    /// Покупка улучшения
    /// </summary>
    /// <param name="info"></param>
    public void BuyUpgrade(UpgradeRestaurantInfo info)
    {
        if (Checks.CheckEnoughGold(CheckPrice(info), bank) && info != null)
        {
            _selectedUpgradeRestaurantInfo = info;
            var nameRestaurant = EquipmentManager.InfoRestaurant.name;

            eventManager.ChangeValueGold(-CheckPrice(info));
            bank.RegisterAllBoughtUpgradesInRestaurant(nameRestaurant, _selectedUpgradeRestaurantInfo.name);
            bank.RegisterRestaurantInvestments(nameRestaurant, _selectedUpgradeRestaurantInfo.Price);
            bank.RegisterDataBoughtUpgradesInRestaurant(EquipmentManager.InfoRestaurant.name, _selectedUpgradeRestaurantInfo.TypeUpgrade);

            eventManager.UpgradeRestaurantIsBoughtEvent(info.TypeUpgrade);
        }
    }
     /// <summary>
     /// Подготавливает все info улучшений этого ресторана
     /// </summary>
    private void PrepareAllUpdatesForButtonsAndCreate()
    {
        _allUpgradeNames = bank.LookAllPossibleUpgradesInRestaurant(EquipmentManager.InfoRestaurant.name).ToArray();//имена всех доступных улучшений этого ресторана
        int numAllUpgradeNames = _allUpgradeNames.Length;

        _allUpgrades = new UpgradeRestaurantInfo[numAllUpgradeNames];

        for (int i = 0; i < _allUpgradeNames.Length; i++)    //загрузка улучшений из ресерсов по имени
            _allUpgrades[i] = Resources.Load<UpgradeRestaurantInfo>($"UpgradesRestaurant/{_allUpgradeNames[i]}");
    }
    /// <summary>
    /// Выбор улучшения
    /// </summary>
    /// <param name="info"></param>
    public void SelectUpgrade(UpgradeRestaurantInfo info)
    {
        _selectedUpgradeRestaurantInfo = info;
    }
    private void OnDestroy()
    {
        eventManager.ConfirmSelectingRestaurantAndgoToItEvent -= RestaurantIsChanged;
        eventManager.ChangeActivePanelUpgradeRestaurantEvent -= ChangeActivePanel;
    }
}
