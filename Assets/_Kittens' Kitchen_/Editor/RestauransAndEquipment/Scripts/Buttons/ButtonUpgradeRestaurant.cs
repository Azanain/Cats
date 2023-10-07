using UnityEngine.EventSystems;
using Zenject;

public class ButtonUpgradeRestaurant : ParentButton, IPointerClickHandler
{
    [Inject] private readonly UpgradesRestaurantManager _upgradesRestaurantManager;
    [Inject] private readonly Bank _bank;

    public void OnPointerClick(PointerEventData eventData)
    {
        OnPress();
    }
    public override void OnPressButtonTrue()
    {
        if (_bank.GetNumberBoughtEquipmentInRestaurant(EquipmentManager.InfoRestaurant.name) > 0)
            _upgradesRestaurantManager.ChangeActivePanel(true);
        else if(_bank.GetNumberBoughtEquipmentInRestaurant(EquipmentManager.InfoRestaurant.name) == 0)
        {
            canPress = true;
            _upgradesRestaurantManager.ChangeActivePanel(false);
        }
    }
}
