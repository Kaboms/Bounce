using UnityEngine;

public class ShieldModificatorBehaviour : MonoBehaviour
{
    private BallController _ballController;
    //--------------------------------------------------------------------------

    private void Awake()
    {
        _ballController = GetComponentInParent<BallController>();
        _ballController.SetImmortal(true);
        _ballController.HitEvent.AddListener(zui);
    }
    //--------------------------------------------------------------------------

    private void zui()
    {
        Destroy(gameObject);
    }
}