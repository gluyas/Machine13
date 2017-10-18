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
		var acceleration = Vector3.ClampMagnitude(WishMovement - Movement, 1);
		if (Vector3.Dot(WishMovement, Movement) <= WishMovement.magnitude)
		{
			var force = MaxAcceleration * _rb.mass;
			_rb.AddForce(acceleration * force);
			if (_rb.velocity.magnitude < 0.1) _rb.velocity = Vector3.zero;
		}

		if (AutoFacing)
		{
			var bankSide = Mathf.Sign(Vector3.Dot(Vector3.Cross(Movement, Vector3.up), acceleration));
			
			var bankAngle = Vector3.Angle(Movement, acceleration) * acceleration.magnitude * bankSide * 2;
			if (bankAngle > 90) bankAngle = 90;
			
			var dorsalAxis = Quaternion.AngleAxis(bankAngle, Movement) * Vector3.up;
			_rb.rotation = Quaternion.LookRotation(Movement, dorsalAxis);
			
			Debug.DrawRay(transform.position, WishMovement * 2, Color.red);
			Debug.DrawRay(transform.position, acceleration * 10, Color.magenta);
			Debug.DrawRay(transform.position, Movement * 2, Color.cyan);
			Debug.DrawRay(transform.position, dorsalAxis * 2);
		}

//		if (AutoFacing)
//		{
//			if (WishMovement.magnitude > 0.1) Facing = WishMovement;
//		}
//		var sign = Mathf.Sign(Vector3.Cross(Vector3.down, Facing).z);
//		_rb.rotation = sign * Vector3.Angle(Vector3.down, Facing);
	}
}
