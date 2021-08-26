using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using MyUtils;
using UnityEngine;

public class TrajectoryMovement : MonoBehaviour
{
	[SerializeField]private Transform _hook;
	[SerializeField]private TrajectoryMovementData _movementData;

	private bool _isPressed, _soundPlayed, _launched;
	private int _waypointIndex;
	private float _initRadius;
	private List<Transform> _waypoints;
	private Transform _parentWaypoint;
	private CircleCollider2D _collider2D;
	private Rigidbody2D _rb2D;
	private LineRenderer _lr;
	private AudioSource _audioSource;

	private Vector2 _startPos,_endPos;
	
	private void Awake()
	{
		_audioSource = GetComponent<AudioSource>();
		_rb2D = GetComponent<Rigidbody2D>();
		_lr = GetComponent<LineRenderer>();
		_collider2D = GetComponent<CircleCollider2D>();
		GetWaypoints();
		_collider2D.enabled = false;
		_startPos = _hook.position;
	}

	private void OnEnable()
	{
		_initRadius = _collider2D.radius;
	}

	private void OnMouseDown()
	{
		_isPressed = true;
	}

	private void OnMouseDrag()
	{
		if (!_audioSource.isPlaying && !_soundPlayed)
		{
			_audioSource.Play();
			_soundPlayed = true;
		}
	}

	private void OnMouseUp()
	{
		
		Launch();
		_audioSource.Stop();
		_soundPlayed = false;
		StopCoroutine(ToggleCollider());
		_collider2D.radius = _initRadius;
	}

	void FixedUpdate ()
	{
		MoveTowardsSling();
		
		if(GameManager.Instance.StopPlaying())
			return;
		
		if (_launched && !_collider2D.enabled)
		{
			_collider2D.enabled = true;
			
			if(gameObject.CompareTag("Unlaunched Box") || gameObject.CompareTag("PowerUp"))
				enabled = false;
		}
		
		DrawTrajectory();
	}
	

	private void Launch()
	{

		if(GameManager.Instance.StopPlaying())
			return;
		
		_launched = true;
		transform.parent = null;
		_isPressed = false;
		_rb2D.isKinematic = false;
		_lr.enabled = false;
		
		Invoke(nameof(BoxLeaveDelay), _movementData.DelayTime);
		
		Vector2 velocity = (_startPos - _rb2D.position) * _movementData.Force;

		_rb2D.velocity = velocity;


	}

	private void BoxLeaveDelay()
	{
		EventManager.TriggerBoxLeaveSling(null);
	}
	private void DrawTrajectory()
	{
		if(_isPressed) 
		{
			_endPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			
			if (Vector3.Distance(_endPos, _startPos) > _movementData.MaxDistance)
			{
				_rb2D.position = _startPos + (_endPos - _startPos).normalized * _movementData.MaxDistance;
			}
			else
			{
				_rb2D.position = _endPos;
			}
			
			Vector2 velocity = (_startPos - _rb2D.position) * _movementData.Force;

			Vector2[] trajectory = CalculateTrajectory(_rb2D, transform.position, velocity, _movementData.Steps);

			_lr.positionCount = trajectory.Length;

			Vector3[] positions = new Vector3[trajectory.Length];

			for(int i = 0; i < trajectory.Length; i++)
			{
				positions[i] = trajectory[i];
			}

			_lr.SetPositions(positions);
		}
	}

	private Vector2[] CalculateTrajectory(Rigidbody2D rigidbody, Vector2 pos, Vector2 velocity, int steps)
	{
		Vector2[] results = new Vector2[steps];

		float timestamp = Time.fixedDeltaTime / Physics2D.velocityIterations;
		Vector2 gravityAccel = Physics2D.gravity * (rigidbody.gravityScale * Mathf.Pow(timestamp,2));

		float drag = 1f - timestamp * rigidbody.drag;
		Vector2 moveStep = velocity * timestamp;

		for (int i = 0; i < steps; i++)
		{
			moveStep += gravityAccel;
			moveStep *= drag;
			pos += moveStep;
			results[i] = pos;
		}

		return results;
	}
	
	private void GetWaypoints()
	{
		_parentWaypoint = GameObject.FindWithTag("Waypoints").transform;
		_hook = _parentWaypoint.GetChild(_parentWaypoint.childCount - 1);
		_waypoints = new List<Transform>();
		
		for (int i = 0; i < _parentWaypoint.childCount; i++)
		{
			Transform child = _parentWaypoint.GetChild(i);
			_waypoints.Add(child);
		}
	}
	private void MoveTowardsSling()
	{
		if (_waypoints[_waypoints.Count - 1].childCount <= 0)
		{
			if (_waypointIndex < _waypoints.Count)
			{
				transform.DOMove(_waypoints[_waypointIndex].transform.position,
					_movementData.MoveSpeed * Time.fixedDeltaTime).SetEase(_movementData.EaseType);
				
				if (Vector2.Distance(transform.position, _waypoints[_waypointIndex].transform.position) <= 0.1f)
				{
					_waypointIndex += 1;
				}
				
				if (Vector2.Distance(transform.position, _waypoints[_waypoints.Count - 1].transform.position) <= 0.1f)
				{
					transform.parent = _waypoints[_waypoints.Count - 1];
					_collider2D.enabled = true;
					_collider2D.radius = 1f;
					StartCoroutine(ToggleCollider());
					EventManager.TriggerBoxReachToSling(transform);
				}
			}
		}
	}

	private IEnumerator ToggleCollider()
	{
		while (!_launched)
		{
			yield return null;
			if (gameObject.CompareTag("Unlaunched Box") || gameObject.CompareTag("PowerUp"))
			{
				yield return BetterWaitForSeconds.Wait(2f);
				_collider2D.enabled = false;
				yield return BetterWaitForSeconds.Wait(0.25f);
				_collider2D.enabled = true;
			}
		}
	}
}
