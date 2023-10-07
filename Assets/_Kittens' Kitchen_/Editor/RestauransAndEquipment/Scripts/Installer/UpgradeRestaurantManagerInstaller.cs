using UnityEngine;
using Zenject;

public class UpgradeRestaurantManagerInstaller : MonoInstaller
{
    [SerializeField] private UpgradesRestaurantManager _upgradesRestaurantManager;
    public override void InstallBindings()
    {
        Container.Bind<UpgradesRestaurantManager>().FromInstance(_upgradesRestaurantManager).AsSingle();
    }
}
