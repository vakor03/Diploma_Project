using _Project.Features.ObjectPoolModule;
using _Project.Features.WeaponsModule.Scripts.Projectiles.ProjectilesCoreModule;
using UnityEngine;

namespace _Project.Features.WeaponsModule.Scripts.Projectiles.ProjectilesPool {
	[CreateAssetMenu(fileName = nameof(ProjectilePoolConfiguration) + "_Default",
		menuName = "Configurations/WeaponsModule/" + nameof(ProjectilePoolConfiguration))]
	public class ProjectilePoolConfiguration : ObjectPoolConfiguration<ProjectileType> { }
}