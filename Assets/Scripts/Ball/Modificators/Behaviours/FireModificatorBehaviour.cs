using UnityEngine;
using UnityEngine.Events;

public class FireModificatorBehaviour : MonoBehaviour
{
    private BallController _ballController;

    // Fire object with particle system
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


    //--------------------------------------------------------------------------

    private void Awake()
    {
        _ballController = GetComponentInParent<BallController>();
        _ballController.TriggerEnterEvent.AddListener(OnParentTriggerEnter);

        _meshRenderer = GetComponentInParent<MeshRenderer>();
        _trailRenderer = GetComponentInParent<TrailRenderer>();
        _particleSystem = GetComponent<ParticleSystem>();

        _standartColor = _meshRenderer.material.color;
    }
    //--------------------------------------------------------------------------

    private void OnParentTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy") || other.gameObject.layer == LayerMask.NameToLayer("Ground"))
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

        if (other.gameObject.CompareTag("TowerDestructor"))
        {
            ++_towerDestructed;

            Color newColor = Color.Lerp(_standartColor, FireColor, _towerDestructed * (1f / RequiredTowerDestruct));
            _meshRenderer.material.color = newColor;
            _trailRenderer.startColor = newColor;

            if (_towerDestructed == RequiredTowerDestruct)
                SetFireMode(true);
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

        if (mode && !GetComponent<ShieldModificatorBehaviour>())
            _ballController.SetImmortal(true);
    }
    //--------------------------------------------------------------------------
}
