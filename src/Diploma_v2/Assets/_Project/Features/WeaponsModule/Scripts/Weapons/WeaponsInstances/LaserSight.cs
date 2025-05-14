using System;
using UnityEngine;

namespace Features.WeaponsModule.Scripts.Weapons.WeaponsInstances {
	public class LaserSight : MonoBehaviour {
		[SerializeField] private LineRenderer _lineRenderer;
		[SerializeField] private Transform _firePoint;
		[SerializeField] private LayerMask _blockingLayer;
		[SerializeField] private float _maxDistance = 100f;
		[SerializeField] private LineRendererConfiguration _lineRendererConfiguration;

		private void Awake() =>
			_lineRendererConfiguration.ApplyTo(_lineRenderer);

		private void Update() =>
			DrawLaser();

		private void OnEnable() =>
			_lineRenderer.gameObject.SetActive(true);

		private void OnDisable() =>
			_lineRenderer.gameObject.SetActive(false);

		private void DrawLaser() {
			_lineRenderer.SetPosition(0, _firePoint.position);

			RaycastHit2D hit = Physics2D.Raycast(_firePoint.position, _firePoint.right, _maxDistance, _blockingLayer);

			if (hit.collider != null)
				_lineRenderer.SetPosition(1, hit.point);
			else
				_lineRenderer.SetPosition(1, _firePoint.position + _firePoint.right * _maxDistance);
		}
	}
}