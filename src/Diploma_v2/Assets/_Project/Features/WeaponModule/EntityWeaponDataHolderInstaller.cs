using Zenject;

namespace _Project.Features.WeaponModule
{
    public class EntityWeaponDataHolderInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<EntityWeaponDataHolder>().AsSingle();
        }
    }
}