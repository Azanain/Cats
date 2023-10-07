using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Zenject;

public class CheatMenu
{
    [Inject] private static EventManager _eventManager;
    
    [MenuItem("Cheat Menu/Add Money", priority = 15)]
    private static void AddMoney()
    {
        if (_eventManager != null)
        {
            _eventManager.ChangeValueGold(5000);
        }
    }
}
