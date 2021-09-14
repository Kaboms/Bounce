using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Contains and generates triangular platforms.
/// </summary>
public class Tower : MonoBehaviour
{
	public GameObject TrianglePrefab;
	public GameObject DestructorPrefab;

	public GameObject EnemyFloorPrefab;
	public GameObject EnemyWallPrefab;

	public List<GameObject> Modificators;

	public UnityEvent DestructorEnter { get; private set; } = new UnityEvent();

	private MeshRenderer _meshRenderer;

	private float _randomModificator = 0;

	private readonly int[] _angles = { 0, 60, 120, 180, 240, 300 };
	//--------------------------------------------------------------------------

	public void Init(Color newColor, float randomModificator)
	{
		_meshRenderer.material.color = newColor;
		_randomModificator = (randomModificator < 0.5f) ? randomModificator : 0.5f;
	}
	//--------------------------------------------------------------------------

	private void Awake()
	{
		_meshRenderer = GetComponent<MeshRenderer>();
	}
	//--------------------------------------------------------------------------

	private void Start()
	{
		SpawnFloors();
		SpawnWall();
		SpawnModificators();

		GameObject towerDestructorObject = Instantiate(DestructorPrefab);
		towerDestructorObject.transform.SetParent(transform);
		towerDestructorObject.transform.localPosition = new Vector3(0, -1.5f, 0);

		TowerDestructor towerDestructor = towerDestructorObject.GetComponent<TowerDestructor>();
		towerDestructor.DestructorEnter.AddListener(OnDestructorTriggered);
	}
	//--------------------------------------------------------------------------

	public void OnDestructorTriggered()
	{
		DestructorEnter.Invoke();

		foreach (Transform child in transform)
		{
			Destructible destructor = child.GetComponent<Destructible>();
			if (destructor)
				destructor.Destruct();
			else
				Destroy(child.gameObject);
		}
	}
	//--------------------------------------------------------------------------

	private void SpawnFloors()
	{
		List<int> freeAngles = _angles.ToList();

		bool first = transform.position.y == 0;

		int maxFloors = Random.Range((int)(2 + _randomModificator * 5), (int)(4 + _randomModificator * 5));
		int enemyCount = 0;

		for (int i = 0; i < maxFloors; ++i)
		{
			bool isEnemy = Random.Range(0.0f, 1.0f) <= (0.3f + _randomModificator) && !first && enemyCount < maxFloors - 1;
			if (isEnemy)
				++enemyCount;

			GameObject triangle = Instantiate((isEnemy) ? EnemyFloorPrefab : TrianglePrefab);

			triangle.transform.SetParent(transform);
			triangle.transform.localPosition = new Vector3(0, -1, -1.5f);

			int choosen = 0;

			if (!first || i > 0)
				choosen = Random.Range(0, freeAngles.Count);

			triangle.transform.eulerAngles = new Vector3
			(
				triangle.transform.eulerAngles.x,
				transform.parent.eulerAngles.y,
				triangle.transform.eulerAngles.z
			);

			triangle.transform.RotateAround(transform.position, Vector3.up, freeAngles[choosen]);

			freeAngles.RemoveAt(choosen);
		}
	}
	//--------------------------------------------------------------------------

	private void SpawnWall()
	{
		if (Random.Range(0.0f, 1.0f) <= (0.30f + _randomModificator) && transform.position.y != 0)
		{
			GameObject wallEnemy = Instantiate(EnemyWallPrefab);
			wallEnemy.transform.SetParent(transform);
			wallEnemy.transform.localPosition = new Vector3(0, 0, -1.5f);
			SetChildAngle(wallEnemy, 30);
		}
	}
	//--------------------------------------------------------------------------

	private void SpawnModificators()
	{
		if (Random.Range(0.00f, 1.00f) > 0.05f)
			return;

		int choosenModificator = Random.Range(0, Modificators.Count);
		GameObject modificator = Instantiate(Modificators[choosenModificator]);
		modificator.transform.SetParent(transform);
		modificator.transform.localPosition = new Vector3(0, 0, -1.5f);

		SetChildAngle(modificator);
	}
	//--------------------------------------------------------------------------

	private void SetChildAngle(GameObject child, int offset = 0)
	{
		child.transform.eulerAngles = new Vector3
		(
			child.transform.eulerAngles.x,
			transform.parent.eulerAngles.y,
			child.transform.eulerAngles.z
		);

		int choosenAngle = Random.Range(0, _angles.Length);
		child.transform.RotateAround(transform.position, new Vector3(0, 1, 0), _angles[choosenAngle] - offset);
	}
	//--------------------------------------------------------------------------
}
