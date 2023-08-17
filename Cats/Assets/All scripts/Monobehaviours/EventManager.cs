using System;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public Action UpgradeTextInfoTypesEvent;
    public Action UpgradeTextCurrentCoinsEvent;
    public Action CheckActiveButtonButEvent;

    public Action<int> SelectUpdateEvent;
    public Action<int> DeleteUpgradeEvent;

    public Action<uint> AddCoinsEvent;
    public Action<uint> SpendCoinsEvent;

    public void UpgradeTextInfoTypes()
    { UpgradeTextInfoTypesEvent?.Invoke(); }
    public void AddCoins(uint coins)
    {
        AddCoinsEvent?.Invoke(coins);
        CheckActiveButtonButEvent?.Invoke();
    }
    public void SpendCoins(uint coins)
    {
        SpendCoinsEvent?.Invoke(coins);
        CheckActiveButtonButEvent?.Invoke();
        UpgradeTextCurrentCoins();
    }
    public void UpgradeTextCurrentCoins()
    { UpgradeTextCurrentCoinsEvent?.Invoke(); }
    public void SelectUpdate(int number)
    { SelectUpdateEvent?.Invoke(number); }
    public void DeleteUpgrade(int number)
    { DeleteUpgradeEvent?.Invoke(number); }
}
