using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Destruct tower when the bounce touches object with this component.
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
