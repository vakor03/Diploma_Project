using _Project.Features.StatsModule;

namespace _Project.Features.WeaponModule
{
    public interface IWeapon
    {
        public IStatService<WeaponStats> Stats { get; }
        public void InitWeaponStats(IStatService<WeaponStats> weaponStats);
    }
}