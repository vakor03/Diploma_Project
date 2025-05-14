using UnityEngine;

namespace Features.AnimationController.Scripts {
	public class MonoPredicate : MonoBehaviour, IPredicate {
		public virtual bool IsTrue { get; protected set; }
	}
}