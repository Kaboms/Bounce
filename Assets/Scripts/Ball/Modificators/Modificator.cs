using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Modificator : Destructible
{
	protected abstract void ApplyModificator(GameObject target);
	//--------------------------------------------------------------------------

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			ApplyModificator(other.gameObject);
			Destroy(gameObject);
		}
	}
	//--------------------------------------------------------------------------
}
