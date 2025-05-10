using DG.Tweening;
using UnityEngine;

namespace _Project.Features.ExperienceModule {
    public class XPOrb : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private SpriteRenderer orbSprite;
        [SerializeField] private Collider2D orbCollider;
        [SerializeField] private ParticleSystem pickupFX;
    
        [Header("Settings")]
        [SerializeField] private float initialBounceHeight = 0.5f;
        [SerializeField] private float initialBounceDuration = 0.5f;
    
        private float xpValue;
        private bool isMoving;
        private XPOrbData orbData;
        private Sequence moveSequence;
    
        public float XPValue => xpValue;
    
        public void Initialize(float xp, XPOrbData data)
        {
            xpValue = xp;
            orbData = data;
            orbSprite.color = data.orbColor;
        
            // Spawn animation
            transform.localScale = Vector3.zero;
            transform.DOScale(1f, 0.3f).SetEase(Ease.OutBack);
        
            // Initial bounce
            Vector3 initialPos = transform.position;
            transform.DOMoveY(initialPos.y + initialBounceHeight, initialBounceDuration)
                .SetEase(Ease.OutQuad)
                .OnComplete(() => 
                {
                    transform.DOMoveY(initialPos.y, initialBounceDuration * 0.5f)
                        .SetEase(Ease.InQuad);
                });
        }
    
        public void MoveToTarget(Transform target)
        {
            if (isMoving) return;
        
            isMoving = true;
            orbCollider.enabled = false;
        
            // Create movement sequence
            moveSequence = DOTween.Sequence();
        
            // Move to target
            moveSequence.Append(transform.DOMove(target.position, 1f / orbData.moveSpeed)
                .SetEase(orbData.moveCurve));
        
            // Dissolve effect
            moveSequence.AppendCallback(() => 
            {
                if (pickupFX != null)
                {
                    pickupFX.Play();
                }
            
                // Scale down during dissolve
                transform.DOScale(0f, orbData.dissolveDuration)
                    .SetEase(Ease.InBack);
            
                // Fade out sprite
                orbSprite.DOFade(0f, orbData.dissolveDuration)
                    .OnComplete(() => 
                    {
                        // Call pickup event here if needed
                        Destroy(gameObject);
                    });
            });
        }
    
        public void CancelMovement()
        {
            if (moveSequence != null && moveSequence.IsActive())
            {
                moveSequence.Kill();
            }
            isMoving = false;
            orbCollider.enabled = true;
        }
    
        private void OnDestroy()
        {
            if (moveSequence != null && moveSequence.IsActive())
            {
                moveSequence.Kill();
            }
        }
    }
}