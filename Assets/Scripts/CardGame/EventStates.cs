using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventStates<T>
{
	public string Name { get { return _stateName; } }

	public event Action<T> OnEnter = delegate { };
	public event Action OnUpdate = delegate { };
	public event Action OnLateUpdate = delegate { };
	public event Action OnFixedUpdate = delegate { };
	public event Action<T> OnExit = delegate { };

	private string _stateName;
	private Dictionary<T, EventTransition<T>> transitions;

	public EventStates(string name)
	{
		_stateName = name;
	}

	public EventStates<T> Configure(Dictionary<T, EventTransition<T>> transitions)
	{
		this.transitions = transitions;
		return this;
	}

	public EventTransition<T> GetTransition(T input)
	{
		return transitions[input];
	}

	public bool CheckInput(T input, out EventStates<T> next)
	{
		if (transitions.ContainsKey(input))
		{
			var transition = transitions[input];
			transition.OnTransitionExecute(input);
			next = transition.TargetState;
			return true;
		}

		next = this;
		return false;
	}

	public void Enter(T input)
	{
		OnEnter(input);
	}

	public void Update()
	{
		OnUpdate();
	}

	public void LateUpdate()
	{
		OnLateUpdate();
	}

	public void FixedUpdate()
	{
		OnFixedUpdate();
	}

	public void Exit(T input)
	{
		OnExit(input);
	}
}
