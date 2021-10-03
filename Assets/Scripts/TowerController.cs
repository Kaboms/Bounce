using System.Collections;
using System.Collections.Generic;
using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using CoreFeatures.MessageBus;
using Random = UnityEngine.Random;

/// <summary>
/// Generate towers and destruct them.
/// </summary>
public class TowerController : MonoBehaviour
{
	public GameObject CylinderPrefab;

	public ScoreController ScoreController;

	private Vector3 _spawnPosition;

	private uint _towerCount = 8;

	private float _colorChangeFactor = 0;

	private Tuple<Color, Color> Colors;
	//--------------------------------------------------------------------------

	private void Awake()
	{
		_spawnPosition = Vector3.zero;

		Colors = Tuple.Create<Color, Color>(Random.ColorHSV(0f, 1f), Random.ColorHSV(0f, 1f));

		MessageBus.GetInstance().Subsribe("TowerDestructed", OnTowerDestructed);
	}
	//--------------------------------------------------------------------------

	private void Start()
	{
		for (int i = 0; i < _towerCount; ++i)
			SpawnTower();
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
	}
	//--------------------------------------------------------------------------

	private void OnTowerDestructed(Message message)
	{
		SpawnTower();

		++_towerCount;

		// Destroy old towers
		if (_towerCount > 10)
		{
			Destroy(transform.GetChild(0).gameObject);
		}
	}
	//--------------------------------------------------------------------------
}