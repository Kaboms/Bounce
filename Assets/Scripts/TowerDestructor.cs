using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Tower destruct when bounce touch object with this component.
/// </summary>
public class TowerDestructor : MonoBehaviour
{
	public UnityEvent DestructorEnter { get; private set; } = new UnityEvent();
	//--------------------------------------------------------------------------

	private void OnTriggerEnter(Collider other)
	{
		DestructorEnter.Invoke();
	}
	//--------------------------------------------------------------------------
}
