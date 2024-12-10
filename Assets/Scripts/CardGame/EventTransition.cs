using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventTransition<T>
{
	public event Action<T> OnTransition = delegate { };
	public T Input { get { return input; } }
	public EventStates<T> TargetState { get { return targetState; } }

	T input;
	EventStates<T> targetState;


	public void OnTransitionExecute(T input)
	{
		OnTransition(input);
	}

	public EventTransition(T input, EventStates<T> targetState)
	{
		this.input = input;
		this.targetState = targetState;
	}
}
