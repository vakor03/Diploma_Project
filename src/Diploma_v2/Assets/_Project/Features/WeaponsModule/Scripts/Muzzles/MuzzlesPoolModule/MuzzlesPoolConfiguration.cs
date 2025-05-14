using _Project.Features.ObjectPoolModule;
using UnityEngine;

namespace _Project.Features.WeaponsModule.Scripts.Muzzles.MuzzlesPoolModule {
	[CreateAssetMenu(fileName = nameof(MuzzlesPoolConfiguration) + "_Default",
		menuName = "Configurations/WeaponsModule/" + nameof(MuzzlesPoolConfiguration))]
	public class MuzzlesPoolConfiguration : ObjectPoolConfiguration<MuzzleType> { }
}