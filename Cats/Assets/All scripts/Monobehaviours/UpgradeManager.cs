using UnityEngine;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour
{
    public uint Coins { get; private set; }
    public InfoUpgrade InfoUpgrade { get; private set; }

    public EventManager eventManager;
    public CurrentUpgrades currentUpgrades;

    [Header("UI")]
    [SerializeField] private Text _currentCoins;
    [Space(5)]
    [SerializeField] private Text _costValue;
    [SerializeField] private Text _speedValue;
    [SerializeField] private Text _weightValue;
    [SerializeField] private Text _qualityValue;
   
    private void Awake()
    {
        if (eventManager == null)
            eventManager = transform.GetComponentInChildren<EventManager>();

        if (currentUpgrades == null)
            currentUpgrades = GameObject.FindGameObjectWithTag("CurrentUpgrades").GetComponent<CurrentUpgrades>();

        eventManager.UpgradeTextCurrentCoinsEvent += UpdateTextCurrentCoins;
        eventManager.AddCoinsEvent += AddCoins;
        eventManager.SpendCoinsEvent += SpendCoins;
        eventManager.UpgradeTextInfoTypesEvent += UpgradeTextInfoType;
    }
    private void Start()
    {
        UpdateTextCurrentCoins();
    }
    public void AddCoins(uint coins)
    {
        Coins += coins;
    }
    public void SpendCoins(uint coins)
    {
        if (Coins - coins >= 0)
            Coins -= coins;
    }
    public void UpdateTextCurrentCoins()
    {
        _currentCoins.text = Coins.ToString();
    }
    public void GetInfoType(InfoUpgrade infoUpgrade)
    {
        InfoUpgrade = infoUpgrade;
    }
    private void UpgradeTextInfoType()
    {
        if (InfoUpgrade.Cost > 0)
            _costValue.text = InfoUpgrade.Cost.ToString();
        else
            _costValue.text = "---";

        if (InfoUpgrade.Speed > 0)
            _speedValue.text = InfoUpgrade.Speed.ToString();
        else
            _speedValue.text = "---";

        if (InfoUpgrade.Weight > 0)
            _weightValue.text = InfoUpgrade.Weight.ToString();
        else
            _weightValue.text = "---";

        if (InfoUpgrade.Quality > 0)
            _qualityValue.text = InfoUpgrade.Quality.ToString();
        else
            _qualityValue.text = "---";
    }
    private void OnDestroy()
    {
        eventManager.UpgradeTextCurrentCoinsEvent -= UpdateTextCurrentCoins;
        eventManager.AddCoinsEvent += AddCoins;
        eventManager.SpendCoinsEvent += SpendCoins;
        eventManager.UpgradeTextInfoTypesEvent -= UpgradeTextInfoType;
    }
}
