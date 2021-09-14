using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldModificator : Modificator
{
	public GameObject ShieldEffect;

	protected override void ApplyModificator(GameObject target)
	{
		if (!target.GetComponent<ShieldModificatorBehaviour>())
		{
			ShieldModificatorBehaviour behavior = target.AddComponent<ShieldModificatorBehaviour>();
			behavior.SetShieldEffect(Instantiate(ShieldEffect));
		}
	}
	//--------------------------------------------------------------------------
}
