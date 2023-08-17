using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonAddCoins : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private UpgradeManager _upgradeManager;
    [SerializeField] private uint _addCoins;

    private void Awake()
    {
        if (_upgradeManager == null)
            _upgradeManager = GameObject.FindGameObjectWithTag("GameData").GetComponentInChildren<UpgradeManager>();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        _upgradeManager.eventManager.AddCoins(_addCoins);
        _upgradeManager.eventManager.UpgradeTextCurrentCoins();
    }
}
