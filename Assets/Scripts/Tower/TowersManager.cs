using CoreFeatures.MessageBus;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// Manage towers (generate and destruct them)
/// </summary>
public class TowersManager : MonoBehaviour
{
    public GameObject CylinderPrefab;

    private Vector3 _spawnPosition;

    private uint _towerCount = 8;

    private float _colorChangeFactor = 0;

    private Tuple<Color, Color> _colors;
    //--------------------------------------------------------------------------

    private void Awake()
    {
        _spawnPosition = Vector3.zero;

        _colors = Tuple.Create<Color, Color>(Random.ColorHSV(0f, 1f), Random.ColorHSV(0f, 1f));

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
            _colors = Tuple.Create<Color, Color>(_colors.Item2, Random.ColorHSV(0f, 1f));
        }

        Color color = Color.Lerp(_colors.Item1, _colors.Item2, _colorChangeFactor);
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