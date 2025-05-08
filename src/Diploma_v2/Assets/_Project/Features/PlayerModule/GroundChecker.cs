using UnityEngine;

namespace _Project.Features.PlayerModule
{
    public class GroundChecker : MonoBehaviour
    {
        [Header("Checks")] 
        [SerializeField] private Transform _groundCheckPoint;
        [SerializeField] private Vector2 _groundCheckSize = new Vector2(0.49f, 0.03f);

        [Header("Layers & Tags")]
        [SerializeField] private LayerMask _groundLayer;

        public bool CheckGrounded() =>
            Physics2D.OverlapBox(_groundCheckPoint.position, _groundCheckSize, 0, _groundLayer);
    }
}