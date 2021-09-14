using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldModificatorBehaviour : MonoBehaviour
{
	private BallController _ballController;

	private GameObject _shieldEffect;
	//--------------------------------------------------------------------------

	private void Awake()
	{
		_ballController = GetComponent<BallController>();
		_ballController.SetImmortal(true);
	}
	//--------------------------------------------------------------------------

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Enemy"))
		{
			_ballController.SetImmortal(false);
			_shieldEffect.SetActive(false);
			Destroy(this);
		}
	}
	//--------------------------------------------------------------------------

	public void SetShieldEffect(GameObject shieldEffect)
	{
		_shieldEffect = shieldEffect;

		_shieldEffect.SetActive(true);
		_shieldEffect.transform.SetParent(transform);
		_shieldEffect.transform.localPosition = Vector3.zero;
	}
	//--------------------------------------------------------------------------

}