using UnityEngine;
using UnityEngine.Events;

public class Timer : MonoBehaviour
{
	public enum EmitType
	{
		Periodic,
		OneShot
	}

	public float EmitTime { get; private set; }
	public bool Started { get; private set; }
	public EmitType Type { get; private set; }

	public bool EmitOnStart { get; private set; }

	private UnityEvent _timerFinish = new UnityEvent();

	private float PassedTime = 0;
	//--------------------------------------------------------------------------

	public void Init(float emitTime, bool emitOnStart = false, EmitType type = EmitType.Periodic)
	{
		EmitTime = emitTime;
		Type = type;
		EmitOnStart = emitOnStart;
	}
	//--------------------------------------------------------------------------

	private void Update()
	{
		if (Started)
		{
			PassedTime += Time.deltaTime;

			if (PassedTime >= EmitTime)
			{
				Emit();
			}
		}
	}
	//--------------------------------------------------------------------------

	public void StartTimer()
	{
		PassedTime = 0;
		Started = true;

		if (EmitOnStart)
			Emit();
	}
	//--------------------------------------------------------------------------

	public void StopTimer()
	{
		Started = false;
	}
	//--------------------------------------------------------------------------

	public void AddListener(UnityAction callback)
	{
		_timerFinish.AddListener(callback);
	}
	//--------------------------------------------------------------------------

	private void Emit()
	{
		PassedTime = 0;

		if (Type == EmitType.OneShot)
			StopTimer();

		_timerFinish?.Invoke();
	}
	//--------------------------------------------------------------------------

}