using UnityEngine;

namespace Features.AnimationController.Scripts {
	public class SwitcherPredicate : MonoPredicate {
		[field: SerializeField] public override bool IsTrue { get; protected set; }
	}
}