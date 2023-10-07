using UnityEngine;

[CreateAssetMenu]
public class UpgradeRestaurantInfo : ScriptableObject
{
    [SerializeField] private EquipmentTypes _typeUpgrade;
    [SerializeField] private string _title;
    [SerializeField] private string _description;
    [SerializeField] private int _price;
    [SerializeField] private int _increaseValueProduct;
    [SerializeField] private int _priceIncrease;
    [SerializeField] private Sprite _sprite;

    public EquipmentTypes TypeUpgrade => _typeUpgrade;
    public string Title => _title;
    public string Description => _description;
    public int Price => _price;
    public int PriceIncrease => _priceIncrease;
    public int IncreaseValueProduct => _increaseValueProduct;
    public Sprite Sprite => _sprite;
}

