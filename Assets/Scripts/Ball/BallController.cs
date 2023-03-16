using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class BallController : MonoBehaviour
{
    public UnityEvent DeathEvent;
    public UnityEvent HitEvent;
    public UnityEvent<Collider> TriggerEnterEvent;

    public float RotateSpeed = 1.5f;

    private bool _immortal = false;

    #region Components
    private Camera _camera;
    private Rigidbody _rigidbody;
    private ParticleSystem _particleSystem;
    #endregion

    private Vector3 _velocity;

    private bool _grounded;

    private const float _cameraVerticalOffset = 3;

    private bool _gameOver = false;

    private float _mouseXPreviousPos;
    private bool _mousePress = false;

    private int _groundLayer;
    //--------------------------------------------------------------------------

    private void Awake()
    {
        _camera = Camera.main;
        _camera.transform.position = new Vector3(_camera.transform.position.x, transform.position.y + _cameraVerticalOffset, _camera.transform.position.z);
        _rigidbody = GetComponent<Rigidbody>();

        _particleSystem = GetComponent<ParticleSystem>();

        _groundLayer = LayerMask.NameToLayer("Ground");
    }
    //--------------------------------------------------------------------------

    private void Update()
    {
        if (_gameOver && Input.GetMouseButtonDown(0))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            return;
        }

        _mousePress = Input.GetMouseButton(0);

        if (Input.GetMouseButtonDown(0))
            _mouseXPreviousPos = Input.mousePosition.x;
    }
    //--------------------------------------------------------------------------

    private void FixedUpdate()
    {
        if (_gameOver)
            return;

        MotionControl();

        if (_mousePress)
        {
            float angle = ((_mouseXPreviousPos - Input.mousePosition.x) / Screen.width) * RotateSpeed;
            _mouseXPreviousPos = Input.mousePosition.x;

            transform.RotateAround(Vector3.zero, Vector3.up, -angle);
            _camera.transform.RotateAround(Vector3.zero, Vector3.up, -angle);
        }
    }
    //--------------------------------------------------------------------------

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == _groundLayer)
        {
            _grounded = true;
            _rigidbody.velocity = Vector3.zero;
        }

        if (other.gameObject.CompareTag("Enemy"))
        {
            Hit();
        }

        TriggerEnterEvent?.Invoke(other);
    }
    //--------------------------------------------------------------------------

    private void Hit()
    {
        HitEvent.Invoke();
        if (!_immortal)
            Death();

        SetImmortal(false);
    }
    //--------------------------------------------------------------------------

    private void MotionControl()
    {
        if (_camera.transform.position.y - transform.position.y > _cameraVerticalOffset)
            _camera.transform.position = new Vector3(_camera.transform.position.x, transform.position.y + _cameraVerticalOffset, _camera.transform.position.z);

        if (_grounded)
        {
            _velocity = new Vector3(0, 20, 0);
        }
        else if (_rigidbody.velocity.y > -20)
        {
            _velocity = new Vector3(0, -1, 0);
        }
        else
        {
            _velocity = Vector3.zero;
        }

        _grounded = false;

        _rigidbody.AddForce(_velocity, ForceMode.VelocityChange);
    }
    //--------------------------------------------------------------------------

    private void Death()
    {
        FindObjectOfType<TowersManager>().enabled = false;

        _particleSystem.Play(false);

        _gameOver = true;
        _rigidbody.velocity = Vector3.zero;

        GetComponent<MeshRenderer>().enabled = false;

        DeathEvent?.Invoke();
    }
    //--------------------------------------------------------------------------

    public void SetImmortal(bool mode)
    {
        if (mode)
            _immortal = true;
        else
            StartCoroutine(DisableImmortal());
    }
    //--------------------------------------------------------------------------

    private IEnumerator DisableImmortal()
    {
        // Add some delay to avoid death after end of the immortality
        yield return new WaitForSeconds(0.5f);
        _immortal = false;
    }
    //--------------------------------------------------------------------------
    //--------------------------------------------------------------------------
    //--------------------------------------------------------------------------
}
