using UnityEngine;

namespace Features.WeaponsModule.Scripts.Weapons.WeaponsInstances {
	[CreateAssetMenu(menuName = "Configurations/Global/"+nameof(LayersConfiguration), fileName = nameof(LayersConfiguration) +"_Default", order = 0)]
	public class LayersConfiguration : ScriptableObject {
		[field: SerializeField] public LayerMask EnemyLayerMask { get; private set; }
		[field: SerializeField] public LayerMask EnvironmentLayerMask { get; private set; }
		[field: SerializeField] public LayerMask PlayerLayerMask { get; private set; }
	}
}