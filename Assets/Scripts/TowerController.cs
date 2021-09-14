using System.Collections;
using System.Collections.Generic;
using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Random = UnityEngine.Random;

/// <summary>
/// Generate towers and destruct them.
/// </summary>
public class TowerController : MonoBehaviour
{
	public GameObject CylinderPrefab;

	public float RotateSpeed = 1.5f;

	public Text Score;

	private float _mouseXPreviousPos;

	private Vector3 _spawnPosition;

	private uint _towerCount = 8;

	private float _colorChangeFactor = 0;

	private Tuple<Color, Color> Colors;

	private uint _score = 0;

	private bool _mousePress = false;

	private Vector3 _eulerAngles;
	private Vector3 _newEulerAngles;

	//--------------------------------------------------------------------------

	private void Awake()
	{
		_spawnPosition = Vector3.zero;
		_eulerAngles = transform.eulerAngles;
		_newEulerAngles = _eulerAngles;

		Colors = Tuple.Create<Color, Color>(Random.ColorHSV(0f, 1f), Random.ColorHSV(0f, 1f));
	}
	//--------------------------------------------------------------------------

	private void Start()
	{
		for (int i = 0; i < _towerCount; ++i)
			SpawnTower();
	}
	//--------------------------------------------------------------------------

	private void Update()
	{
		_mousePress = Input.GetMouseButton(0);

		if (Input.GetMouseButtonDown(0))
			_mouseXPreviousPos = Input.mousePosition.x;
	}
	//--------------------------------------------------------------------------

	private void FixedUpdate()
	{
		if (_mousePress)
		{
			_eulerAngles.y += ((_mouseXPreviousPos - Input.mousePosition.x) / Screen.width) * RotateSpeed;
			_mouseXPreviousPos = Input.mousePosition.x;
		}

		transform.eulerAngles = _eulerAngles;
	}
	//--------------------------------------------------------------------------

	private void SpawnTower()
	{
		GameObject cylinder = Instantiate(CylinderPrefab, transform);
		cylinder.transform.localPosition = _spawnPosition;

		_spawnPosition.y -= CylinderPrefab.transform.localScale.y * 2;

		if (_colorChangeFactor >= 1)
		{
			_colorChangeFactor = 0;
			Colors = Tuple.Create<Color, Color>(Colors.Item2, Random.ColorHSV(0f, 1f));
		}

		Color color = Color.Lerp(Colors.Item1, Colors.Item2, _colorChangeFactor);
		Camera.main.backgroundColor = color;
		_colorChangeFactor += 0.02f;

		Tower tower = cylinder.GetComponent<Tower>();
		tower.Init(color, _towerCount * 0.001f);
		tower.DestructorEnter.AddListener(OnTowerDestruct);
	}
	//--------------------------------------------------------------------------

	private void OnTowerDestruct()
	{
		SpawnTower();

		++_towerCount;

		// Destroy old towers
		if (_towerCount > 10)
		{
			Destroy(transform.GetChild(0).gameObject);
		}

		_score += 1;
		Score.text = _score.ToString();
	}
	//--------------------------------------------------------------------------
}