using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class PanelInfoRestaurant : MonoBehaviour
{
    [Inject] private readonly EventManager _eventManager;
    [Inject] private readonly RestaurantsManager _restaurantsManager;
    [Inject] private readonly Bank _bank;

    [SerializeField] private Transform _panel;
   
    [Header("Texts")]
    [SerializeField] private Text _nameText;
    [SerializeField] private Text _priceText;
    [SerializeField] private Text _recommendedProductText;
    [SerializeField] private Text _bonusRecommendedProductText;

    [Header("Buttons")]
    [SerializeField] private Button _buttonGoToRestaurant;
    [SerializeField] private ButtonBuyRestaurant _buttonBuyRestaurant;
    [SerializeField] private ButtonSellRestaurant _buttonSellRestaurant;

    private RestaurantInfo _info;
    private void Awake()
    {
        _eventManager.OpenPanelBuyRestaurantEvent += OpenPanelAndGetData;
        _eventManager.SaleModeEvent += SalaModeIsActive;
    }
    private void CheckRestaurantExist()
    {
        if(_bank.CheckBoughtRestaurant(_restaurantsManager.SelectedButtonRestaurant.name))
        {
            _buttonGoToRestaurant.gameObject.SetActive(true);
            _buttonBuyRestaurant.gameObject.SetActive(false);
        }
        else 
        {
            _buttonGoToRestaurant.gameObject.SetActive(false);
            _buttonBuyRestaurant.gameObject.SetActive(true);
        }
    }
    private void SalaModeIsActive(bool isActive)
    {
        if (isActive && _bank.CheckNumberBoughtRestaurants() > 1)/* Checks.CheckIndexBoughtRestaurantPlaces(_bank) > 1)*/
        {
            _buttonSellRestaurant.gameObject.SetActive(true);
            _buttonBuyRestaurant.gameObject.SetActive(false);
            _buttonGoToRestaurant.gameObject.SetActive(false);
        }
        else
            _buttonSellRestaurant.gameObject.SetActive(false);
    }
    public void GetInfo(RestaurantInfo info)
    {
        _info = info;
        _buttonBuyRestaurant.GetPrice(info.Price);

        if(_info != null)
            CheckRestaurantExist();
    }
    public void OpenPanelAndGetData(Vector3 position, RestaurantInfo info)
    {
        if(_panel != null)
            _panel.localPosition = position;

        _info = info;

        UpdateText();
    }
    private void UpdateText()
    {
        if (_info != null)
        {
            _nameText.text = _info.Name;
            _priceText.text = _info.Price.ToString();
            _recommendedProductText.text = _info.RecommendedProduct.ToString();
            _bonusRecommendedProductText.text = _info.BonusRecommendedProduct.ToString();
        }
    }

    private void OnDestroy()
    {
        _eventManager.OpenPanelBuyRestaurantEvent -= OpenPanelAndGetData;
        _eventManager.SaleModeEvent -= SalaModeIsActive;
    }
}
