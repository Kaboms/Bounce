using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FireModificatorBehaviour : MonoBehaviour
{
	// Fire object with particle system
	public GameObject FireEffect;
	public Color FireColor = Color.red;
	private ParticleSystem _particleSystem;

	// Count of destructed tower until enable fire mode
	public int RequiredTowerDestruct = 15;

	private Color _standartColor;

	// When ball in fire mode we destructure objects on collision
	private bool _fireMode = false;

	private int _towerDestructed = 0;

	private MeshRenderer _meshRenderer;
	private TrailRenderer _trailRenderer;

	private UnityEvent<bool> _modeChangedEvent = new UnityEvent<bool>();

	//--------------------------------------------------------------------------

	private void Awake()
	{
		_meshRenderer = GetComponent<MeshRenderer>();
		_trailRenderer = GetComponent<TrailRenderer>();
		_particleSystem = FireEffect.GetComponent<ParticleSystem>();

		_standartColor = _meshRenderer.material.color;
	}
	//--------------------------------------------------------------------------

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("TowerDestructor"))
		{
			++_towerDestructed;

			Color newColor = Color.Lerp(_standartColor, FireColor, _towerDestructed * (1f / RequiredTowerDestruct));
			_meshRenderer.material.color = newColor;
			_trailRenderer.startColor = newColor;

			if (_towerDestructed == RequiredTowerDestruct)
				SetFireMode(true);
		}
		else if (other.gameObject.CompareTag("Enemy") || other.gameObject.layer == LayerMask.NameToLayer("Ground"))
		{
			if (_towerDestructed > 0)
			{
				_meshRenderer.material.color = _standartColor;
				_trailRenderer.startColor = _standartColor;

				if (_fireMode)
				{
					SetFireMode(false);

					if (other.gameObject.TryGetComponent<Destructible>(out Destructible destructor))
						destructor.Destruct();
				}
			}
			_towerDestructed = 0;
		}
	}
	//--------------------------------------------------------------------------

	private void SetFireMode(bool mode)
	{
		if (_fireMode == mode)
			return;

		_fireMode = mode;

		if (mode)
			_particleSystem.Play();
		else
			_particleSystem.Stop();

		_modeChangedEvent.Invoke(mode);
	}
	//--------------------------------------------------------------------------

	public void AddOnModeChangeListener(UnityAction<bool> listener)
	{
		_modeChangedEvent.AddListener(listener);
	}
	//--------------------------------------------------------------------------
}
