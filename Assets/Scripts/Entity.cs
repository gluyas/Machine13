using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Component for objects to move around within the world
/// Provides a simpler movement model than Unity's physics engine.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class Entity : MonoBehaviour
{	
	public float MaxSpeed;
	public float MaxAcceleration;

	public bool AutoFacing = true;
	public bool DestroyOnDeath = false;

	private Vector3 _wishMovement;
	public Vector3 WishMovement
	{
		get { return _wishMovement; } 
		set { _wishMovement = Vector3.ClampMagnitude(value, 1); }
	}

	public Vector3 Movement
	{
		get { return Velocity / MaxSpeed; }
	}
	
	public Vector3 WishVelocity
	{
		get { return WishMovement * MaxSpeed; }
	}

	public Vector3 Velocity
	{
		get { return _rb.velocity; }
	}

	public Vector3 Facing { get; set; }

	public int MaxHp;
	public int Hp { get; private set; }

	public bool Dead { get; private set; }

	private int _platforms = 0;
	public bool HasPlatforms { get { return _platforms > 0; }}
	
	private Rigidbody _rb;

	private AudioSource _audio;
	
	private void Awake()
	{
		Hp = MaxHp;
		_rb = GetComponent<Rigidbody>();
		_audio = GetComponent<AudioSource>();
//		OnDeath.AddListener(() =>
//		{
//			if (DestroyOnDeath) Destroy(this.gameObject);
//			Dead = true;
//		});
//		OnDamage.AddListener();
	}
	
	private void FixedUpdate()
	{	
		if (Vector3.Dot(WishMovement, Movement) <= WishMovement.magnitude)
		{
			var force = MaxAcceleration * _rb.mass;
			_rb.AddForce(Vector3.ClampMagnitude(WishMovement - Movement, 1) * force);
			if (_rb.velocity.magnitude < 0.1) _rb.velocity = Vector3.zero;
		}

//		if (AutoFacing)
//		{
//			if (WishMovement.magnitude > 0.1) Facing = WishMovement;
//		}
//		var sign = Mathf.Sign(Vector3.Cross(Vector3.down, Facing).z);
//		_rb.rotation = sign * Vector3.Angle(Vector3.down, Facing);
	}
}
