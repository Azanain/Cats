public struct Checks
{
    /// Проверка наличия золота
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static bool CheckEnoughGold(float value, Bank bank)
    {
        bool isEnough = false;

        if (bank.Gold - value >= 0)
            isEnough = true;

        return isEnough;
    }
    /// <summary>
    /// проверка достаточно-ли золота
    /// </summary>
    /// <param name="price"></param>
    /// <returns></returns>
    public static bool CheckEnoughGoldForBuyRestaurant(float price, Bank bank)
    {
        bool isEnough = false;

        if (bank.Gold >= price)
            isEnough = true;

        return isEnough;
    }
    /// <summary>
    /// Проверка на первую покупку оборудования
    /// </summary>
    /// <returns></returns>
    public static bool CheckFirstBuyEquipment(Bank bank)
    {
        bool firtTime = true;

        if (bank.BoughtEquipmentsInRestaurant.Count != 0)
            firtTime = false;

        return firtTime;
    }
    /// <summary>
    /// Проверка, хватит ли денег для покупки оборудования
    /// </summary>
    /// <returns></returns>
    public static bool CheckEnoughGoldForBuyEquipment(float price, Bank bank)
    {
        bool enoughGold = false;

        if (bank.Gold >= price || CheckFirstBuyEquipment(bank))
            enoughGold = true;

        return enoughGold;
    }
    /// <summary>
    /// Проверка, хватит ли денег для покупки улучшения оборудования
    /// </summary>
    /// <returns></returns>
    public static bool CheckEnoughGoldForBuyUpdateEquipment(int numberUpdate, Bank bank)
    {
        bool enoughGold = false;
        float price = EquipmentImprovementSystem.StartingPriceImprovement + (numberUpdate * EquipmentImprovementSystem.IncreasePriceProductAfterImprovement);

        if (bank.Gold >= price)
            enoughGold = true;

        return enoughGold;
    }
    /// <summary>
    /// Проверка на дублирование купленного оборудования
    /// </summary>
    /// <returns></returns>
    public static bool CheckDuplicationBoughtEquipment(Bank bank)
    {
        bool isDuplicat = false;

        if(!bank.BoughtEquipmentsInRestaurant.ContainsKey(EquipmentManager.InfoRestaurant.Name))
            isDuplicat = true;

        return isDuplicat;
    }
    /// <summary>
    /// Проверка, есть ли хоть 1 пустой слот оборудования
    /// </summary>
    /// <returns></returns>
    public static bool CheckEmptyEquipmentSlot(int maxNumberEquipments, string nameRestaurant, Bank bank)
    {
        bool emptySlotExist = false;

        if (bank.GetNumberBoughtEquipmentInRestaurant(nameRestaurant) < maxNumberEquipments)
            emptySlotExist = true;

        return emptySlotExist;
    }
}

