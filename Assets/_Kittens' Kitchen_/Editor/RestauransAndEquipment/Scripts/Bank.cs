using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Bank : MonoBehaviour
{
    [Inject] private readonly EventManager _eventManager;

    public float Gold { get; private set; }
    public int Dimonds { get; private set; }
    public int Cutlets { get; private set; }
    public int Sugar { get; private set; }

    /// <summary>
    /// Словарь инвестиций в каждом купленном ресторане
    /// </summary>
    public Dictionary<string, float> RestaurantInvestments { get; private set; } = new();

    /// <summary>
    /// Словарь подуровней в каждом оборудовании
    /// </summary>
    public Dictionary<string, int> SublevelEquipments { get; private set; } = new();

    /// <summary>
    /// Словарь количества улучшений оборудований в каждом купленном ресторане
    /// </summary>
    public Dictionary<string, int> NumberUpgradesEquipmentsInRestaurant { get; private set; } = new();

    /// <summary>
    /// Словарь купленного оборудования в каждом купленном ресторане
    /// </summary>
    public Dictionary<string, List<string>> BoughtEquipmentsInRestaurant { get; private set; } = new();

    /// <summary>
    /// Словарь купоенных улучшений ресторанов
    /// </summary>
    public Dictionary<string, List<string>> AllBoughtUpgradesInRestaurant { get; private set; } = new();
    /// <summary>
    /// Словарь возможных улучшений ресторана
    /// </summary>
    public Dictionary<string, List<string>> PossibleUpgradesRestaurant { get; private set; } = new();
    /// <summary>
    /// Словарь преобритённых типов оборудований в ресторане
    /// </summary>
    public Dictionary<string, List<int>> PurchasedTypesEquipmentInRestaurant { get; private set; } = new();
    /// <summary>
    /// Список купленных ресторанов
    /// </summary>
    public List<string> BoughtRestaurants { get; private set; } = new();
    /// Последний выбранный ресторан
    public int LastSelectedRestaurant { get; private set; }
    /// <summary>
    /// Словарь 
    /// </summary>
    public Dictionary<string, List<EquipmentTypes>> DataBoughtUpgradesInRestaurant { get; private set; } = new();

    private void Awake()
    {
        _eventManager.ChangeValueGoldEvent += ChangeValueGold;
        _eventManager.ChangeValueDimondsEvent += ChangeValueDimonds;
        _eventManager.ChangeValueSugarEvent += ChangeValueSugar;
        _eventManager.ChangeValueCutletsEvent += ChangeValueCutlets;
    }
    public void ChangeNumberLastSelectedRestaurant(int numberRestaurant)
    { LastSelectedRestaurant = numberRestaurant; }
    private void ChangeValueGold(float value)
    {
        Gold += value;
    }
    private void ChangeValueDimonds(int value)
    {
        Dimonds += value;
    }
    private void ChangeValueCutlets(int value)
    {
        Cutlets += value;
    }
    private void ChangeValueSugar(int value)
    {
        Sugar += value;
    }
    private void OnDestroy()
    {
        _eventManager.ChangeValueGoldEvent -= ChangeValueGold;
        _eventManager.ChangeValueDimondsEvent -= ChangeValueDimonds;
        _eventManager.ChangeValueSugarEvent -= ChangeValueSugar;
        _eventManager.ChangeValueCutletsEvent -= ChangeValueCutlets;
    }
    public void AddBughtRestaurant(string nameRestaurant)
    {
        if(!BoughtRestaurants.Contains(nameRestaurant))
            BoughtRestaurants.Add(nameRestaurant);
    }
    public bool CheckBoughtRestaurant(string nameRestaurant)
    {
        bool isBought = false;

        if (BoughtRestaurants.Contains(nameRestaurant))
            isBought = true;

        return isBought;
    }
    public int CheckNumberBoughtRestaurants()
    {
        int number = BoughtRestaurants.Count;

        return number;
    }
    public void RemoveBughtRestaurant(string nameRestaurant)
    {
        BoughtRestaurants.Remove(nameRestaurant);
    }
    ///   RestaurantInvestments
    public void RegisterSublevelEquipment(string nameInfo, int sublevel)
    {
        if (SublevelEquipments.ContainsKey(nameInfo))
            SublevelEquipments[nameInfo] = sublevel;
        else
            SublevelEquipments.Add(nameInfo, sublevel);
        
        //add save
    }
    public void UnRegisterSubLevelEquipment(string nameInfo)
    {
        if (SublevelEquipments.ContainsKey(nameInfo))
            SublevelEquipments.Remove(nameInfo);
        //add save
    }
    public float GetSubLevelequipment(string nameInfo)
    {
        return SublevelEquipments[nameInfo];
    }
    ///  SublevelEquipments
    public void RegisterRestaurantInvestments(string nameInfo, float investments)
    {
        if (RestaurantInvestments.ContainsKey(nameInfo))
        {
            float invert = GetRestaurantInvestments(nameInfo) + investments;
            RestaurantInvestments[nameInfo] = invert;
        }
        else
            RestaurantInvestments.Add(nameInfo, investments);

        //add save
    }
    public void UnRegisterRestaurantInvestments(string nameInfo)
    {
        RestaurantInvestments.Remove(nameInfo);
        //add save
    }
    public float GetRestaurantInvestments(string nameInfo)
    {
        return RestaurantInvestments[nameInfo];
    }
    /// NumberUpgradesEquipmentsInRestaurant
    ///
    public void RegisterNumberUpgradesEquipmentsInRestaurant(string nameInfo, int number = 1)
    {
        if (NumberUpgradesEquipmentsInRestaurant.ContainsKey(nameInfo))
        {
            int num = GetNumberUpgradesEquipmentsInRestaurant(nameInfo) + 1;
            NumberUpgradesEquipmentsInRestaurant[nameInfo] = num;
        }
        else
            NumberUpgradesEquipmentsInRestaurant.Add(nameInfo, number);

        //add save
    }
    public void UnRegisterNumberUpgradesEquipmentsInRestaurant(string nameInfo)
    {
        if (NumberUpgradesEquipmentsInRestaurant.ContainsKey(nameInfo))
            NumberUpgradesEquipmentsInRestaurant.Remove(nameInfo);
        //add save
    }
    public int GetNumberUpgradesEquipmentsInRestaurant(string nameInfo)
    {
        int number = 0;

        if (NumberUpgradesEquipmentsInRestaurant.ContainsKey(nameInfo))
            number = NumberUpgradesEquipmentsInRestaurant[nameInfo];
        else
            RegisterNumberUpgradesEquipmentsInRestaurant(nameInfo, 0);

        return number;
    }
    /// BoughtEquipmentsInRestaurant
    public void RegisterBoughtEquipmentInRestaurant(string nameRestaurant, string nameEquipment)
    {
        List<string> list;
        if (BoughtEquipmentsInRestaurant.ContainsKey(nameRestaurant))
        {
            list = BoughtEquipmentsInRestaurant[nameRestaurant];
            list.Add(nameEquipment);
            BoughtEquipmentsInRestaurant[nameRestaurant] = list;
        }
        else
        {
            list = new();
            list.Add(nameEquipment);
            BoughtEquipmentsInRestaurant.Add(nameRestaurant, list);
        }

        //add save
    }
    public void UnRegisterBoughtEquipmentInRestaursnt(string nameRestaurant)
    {
        if (BoughtEquipmentsInRestaurant.ContainsKey(nameRestaurant))
            BoughtEquipmentsInRestaurant.Remove(nameRestaurant);
        //add save
    }
    public bool CheckBoughtEquipmentInRestaurant(string nameRestaurant, string nameEquipment)
    {
        bool isExist = false;

        if (BoughtEquipmentsInRestaurant.ContainsKey(nameRestaurant))
        {
            List<string> list = BoughtEquipmentsInRestaurant[nameRestaurant];

            if (list.Contains(nameEquipment))
                isExist = true;
        }

        return isExist;
    }
    public int GetNumberBoughtEquipmentInRestaurant(string nameRestaurant)
    {
        int number = 0;

        if (BoughtEquipmentsInRestaurant.ContainsKey(nameRestaurant))
        {
            List<string> list = BoughtEquipmentsInRestaurant[nameRestaurant];

            number = list.Count;
        }

        return number;
    }
    /// BoughtUpgradesRestaurant
    public void RegisterAllBoughtUpgradesInRestaurant(string nameRestaurant, string nameUpgrade)
    {
        List<string> list;
        if (AllBoughtUpgradesInRestaurant.ContainsKey(nameRestaurant))
        {
            list = AllBoughtUpgradesInRestaurant[nameRestaurant];
            list.Add(nameUpgrade);
            AllBoughtUpgradesInRestaurant[nameRestaurant] = list;
        }
        else
        {
            list = new();
            list.Add(nameUpgrade);
            AllBoughtUpgradesInRestaurant.Add(nameRestaurant, list);
        }

        //add save
    }
    public void UnAllBoughtUpgradesInRestaurant(string nameRestaurant)
    {
        if (AllBoughtUpgradesInRestaurant.ContainsKey(nameRestaurant))
            AllBoughtUpgradesInRestaurant.Remove(nameRestaurant);
        //add save
    }
    public bool CheckAllBoughtUpgradesInRestaurant(string nameRestaurant, string nameUpgrade)
    {
        bool isExist = false;

        if (AllBoughtUpgradesInRestaurant.ContainsKey(nameRestaurant))
        {
            List<string> list = AllBoughtUpgradesInRestaurant[nameRestaurant];

            if (list.Contains(nameUpgrade))
                isExist = true;
        }

        return isExist;
    }
    //public int GetAllBoughtUpgradesInRestaurant(string nameRestaurant)
    //{
    //    int number = 0;

    //    if (AllBoughtUpgradesInRestaurant.ContainsKey(nameRestaurant))
    //    {
    //        List<string> list = AllBoughtUpgradesInRestaurant[nameRestaurant];

    //        number = list.Count;
    //    }

    //    return number;
    //}
    /// PossibleUpgradesRestaurant
    public void RegisterPossibleUpgradesRestaurant(string nameRestaurant, string nameUpgrade)
    {
        List<string> list;
        if (PossibleUpgradesRestaurant.ContainsKey(nameRestaurant))
        {
            list = PossibleUpgradesRestaurant[nameRestaurant];
            list.Add(nameUpgrade);
            PossibleUpgradesRestaurant[nameRestaurant] = list;
        }
        else
        {
            list = new();
            list.Add(nameUpgrade);
            PossibleUpgradesRestaurant.Add(nameRestaurant, list);
        }

        //add save
    }
    public void UnRegisterPossibleUpgradesRestaurant(string nameRestaurant)
    {
        if (PossibleUpgradesRestaurant.ContainsKey(nameRestaurant))
            PossibleUpgradesRestaurant.Remove(nameRestaurant);
        //add save
    }
    public bool CheckPossibleUpgradesRestaurant(string nameRestaurant)
    {
        bool isExist = false;

        if (PossibleUpgradesRestaurant.ContainsKey(nameRestaurant))
                isExist = true;

        return isExist;
    }
    public List<string> LookAllPossibleUpgradesInRestaurant(string nameRestaurant)
    {
        List<string> allUpgrades = null;

        if (PossibleUpgradesRestaurant.ContainsKey(nameRestaurant))
            allUpgrades = PossibleUpgradesRestaurant[nameRestaurant];

        return allUpgrades;
    }
    //public int GetNumberPossibleUpgradesRestaurant(string nameRestaurant)
    //{
    //    int number = 0;

    //    if (PossibleUpgradesRestaurant.ContainsKey(nameRestaurant))
    //    {
    //        List<string> list = PossibleUpgradesRestaurant[nameRestaurant];

    //        number = list.Count;
    //    }

    //    return number;
    //}
    /// PurchasedTypesEquipmentInRestaurant
    public void RegisterPurchasedTypesEquipmentInRestaurant(string nameRestaurant, int numberType)
    {
        List<int> list;

        if (PurchasedTypesEquipmentInRestaurant.ContainsKey(nameRestaurant))
        {
            list = PurchasedTypesEquipmentInRestaurant[nameRestaurant];
            list.Add(numberType);
            PurchasedTypesEquipmentInRestaurant[nameRestaurant] = list;
        }
        else
        {
            list = new();
            list.Add(numberType);
            PurchasedTypesEquipmentInRestaurant.Add(nameRestaurant, list);
        }

        //add save
    }
    public void UnRegisterPurchasedTypesEquipmentInRestaurant(string nameRestaurant)
    {
        if (PurchasedTypesEquipmentInRestaurant.ContainsKey(nameRestaurant))
            PurchasedTypesEquipmentInRestaurant.Remove(nameRestaurant);
        //add save
    }
    public bool CheckPurchasedTypesEquipmentInRestaurant(string nameRestaurant, int numberType)
    {
        bool isExist = false;

        if (PurchasedTypesEquipmentInRestaurant.ContainsKey(nameRestaurant))
        {
            List<int> list = PurchasedTypesEquipmentInRestaurant[nameRestaurant];

            if (list.Contains(numberType))
                isExist = true;
        }
        return isExist;
    }
    /// ParametrsBoughtUpgradesInRestaurant
    public void RegisterDataBoughtUpgradesInRestaurant(string nameRestaurant, EquipmentTypes type)
    {
        List<EquipmentTypes> list;

        if (DataBoughtUpgradesInRestaurant.ContainsKey(nameRestaurant))
        {
            list = DataBoughtUpgradesInRestaurant[nameRestaurant];
            list.Add(type);
            DataBoughtUpgradesInRestaurant[nameRestaurant] = list;
        }
        else
        {
            list = new();
            list.Add(type);
            DataBoughtUpgradesInRestaurant.Add(nameRestaurant, list);
        }

        //add save
    }
    public void UnRegisterDataBoughtUpgradesInRestaurant(string nameRestaurant)
    {
        if (DataBoughtUpgradesInRestaurant.ContainsKey(nameRestaurant))
            DataBoughtUpgradesInRestaurant.Remove(nameRestaurant);
        //add save
    }
    public bool CheckDataBoughtUpgradesInRestaurant(string nameRestaurant, EquipmentTypes type)
    {
        bool isExist = false;

        if (DataBoughtUpgradesInRestaurant.ContainsKey(nameRestaurant))
        {
            List<EquipmentTypes> list = DataBoughtUpgradesInRestaurant[nameRestaurant];

            if (list.Contains(type))
                isExist = true;
        }
        return isExist;
    }
    public List<EquipmentTypes> GetAllUpgradeTypesInrestaurant(string nameRestaurant, EquipmentTypes type)
    {
        List<EquipmentTypes> types = new();

        if (DataBoughtUpgradesInRestaurant.ContainsKey(nameRestaurant))
        {
            foreach (var item in DataBoughtUpgradesInRestaurant[nameRestaurant])
                if (item == type)
                    types.Add(item);
        }

        return types;
    }
    public int GetNumberAllUpgradeTypeInrestaurant(string nameRestaurant, EquipmentTypes type)
    {
        int number = GetAllUpgradeTypesInrestaurant(nameRestaurant, type).Count;

        return number;
    }
}
