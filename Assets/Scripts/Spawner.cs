using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
	public GameObject Swarmer;
	
	public int SpawnWaveSize;
	public float SpawnWaveInterval;
	public float SpawnWaveFirstInterval;
	public float SpawnRadius;
	public Vector3 SpawnOffset;

	// ANIMATION
	public float Height;
	public float Rotation;
	
	public AnimationCurve AscentionDisplacement = AnimationCurve.EaseInOut(0, 0, 1, 1);
	public AnimationCurve AscentionRotation = AnimationCurve.Linear(0, 0, 1, 1);
	public float AscentionTime;
	
	public AnimationCurve DescentionDisplacement = AnimationCurve.EaseInOut(0, 1, 1, 0);
	public AnimationCurve DescentionRotation = AnimationCurve.Linear(0, 1, 1, 3);
	public float DescentionTime;

	private float _rotation;
	private float _baseHeight;
	
	private HashSet<Func<bool>> _updaters = new HashSet<Func<bool>>();

	public void Init(Vector3 pos)
	{
		this.transform.position = pos;
		_baseHeight = pos.y;

		var time = AscentionTime;
		_updaters.Add(() =>		// ascend animation
		{
			time = Mathf.Clamp(time - Time.deltaTime, 0, time);
			this.transform.position = new Vector3(
				transform.position.x,
				_baseHeight + AscentionDisplacement.Evaluate(1 - time / AscentionTime) * Height,
				transform.position.z
			);
			_rotation = AscentionRotation.Evaluate(1 - time / AscentionTime) * Rotation;

			if (time <= 0)
			{
				time = SpawnWaveFirstInterval;
				_updaters.Add(() =>
				{
					time -= Time.deltaTime;
					if (time <= 0)
					{
						for (var i = 0; i < SpawnWaveSize; i++)
						{
							Debug.Log("poo");
							var x = Random.insideUnitSphere;
							Instantiate(Swarmer, transform.position + SpawnOffset + 
							                     SpawnRadius * Random.insideUnitSphere, Quaternion.identity);
						}
						time += SpawnWaveInterval;
					}
					return false;
				});
				return true;
			}
			else return false;
		});
	}

	private void FixedUpdate()
	{
		_updaters.RemoveWhere(updater => updater.Invoke());	// runs all updaters and removes ones which return true
		
		this.transform.rotation *= Quaternion.AngleAxis(_rotation * Time.deltaTime, Vector3.up);
	}
}
