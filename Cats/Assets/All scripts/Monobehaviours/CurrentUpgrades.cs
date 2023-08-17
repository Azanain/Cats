using System.Collections.Generic;
using UnityEngine;

public class CurrentUpgrades : MonoBehaviour
{
    [SerializeField] private EventManager _eventManager;

    [Range(1, 8)][SerializeField] private int _numberCurrentUpgrades;
    [SerializeField] private ButtonUpgrade _prefabButtonUpgrade;

    [SerializeField] private List<InfoUpgrade> _infoUpgrades = new List<InfoUpgrade>();
    [SerializeField] private List<ButtonUpgrade> _buttonsUpdate = new List<ButtonUpgrade>();
    private List<int> _numbers = new List<int>();
    private void Start()
    {
        _eventManager.DeleteUpgradeEvent += DeleteSelectedUpgrade;

        CreateNewListUpdates();
    }
    public void CreateNewListUpdates()
    {
        for (int i = 0; i < transform.childCount; i++)
            Destroy(transform.GetChild(i).gameObject);

        _infoUpgrades.Clear();
        _buttonsUpdate.Clear();
        _numbers.Clear();

        if (_numberCurrentUpgrades == 0)
            _numberCurrentUpgrades = Random.Range(4,8);

        GetUpgradesFromResources();
        CreateButtonsUpgrade();
    }
    private void GetUpgradesFromResources()
    {
        for (int i = 0; i < _numberCurrentUpgrades; i++)
        {
            int maxNum = Resources.LoadAll<InfoUpgrade>($"Info/").Length;
            int index = Random.Range(1, maxNum + 1); 

            while (_numbers.Contains(index))
                index = Random.Range(1, maxNum + 1);

            _numbers.Add(index);
        }

        GetInfoUpgrades();
    }
    private void GetInfoUpgrades()
    { 
          for (int i = 0; i < _numberCurrentUpgrades; i++)
            _infoUpgrades.Add(Resources.Load<InfoUpgrade>($"Info/InfoObject_{_numbers[i]}"));
    }
    private void CreateButtonsUpgrade()
    {
        for (int i = 0; i < _numberCurrentUpgrades; i++)
        {
            var buttonUpgrade = Instantiate(_prefabButtonUpgrade, transform);
            buttonUpgrade.GetInfo(_infoUpgrades[i]);
            _buttonsUpdate.Add(buttonUpgrade);
        }
    }
    public void DeleteSelectedUpgrade(int number)
    {
        Debug.Log(number);
        _buttonsUpdate.Remove(_buttonsUpdate[number]);
        _infoUpgrades.Remove(_infoUpgrades[number]);
        _numbers.Remove(_numbers[number]);
        var child = transform.GetChild(number);
        Destroy(child.gameObject);
    }
    private void OnDestroy()
    {
        _eventManager.DeleteUpgradeEvent += DeleteSelectedUpgrade;
    }
}
