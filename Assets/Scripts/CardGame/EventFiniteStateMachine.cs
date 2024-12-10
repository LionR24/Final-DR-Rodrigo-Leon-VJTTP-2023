using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventFiniteStateMachine<T>
{
	public EventStates<T> Current { get { return current; } }
	private EventStates<T> current;

	public EventFiniteStateMachine(EventStates<T> initial)
	{
		current = initial;
		current.Enter(default(T));
	}

	public void SendInput(T input)
	{
		EventStates<T> newState;

		if (current.CheckInput(input, out newState))
		{
			current.Exit(input);
			current = newState;
			current.Enter(input);
		}
	}


	public void Update()
	{
		current.Update();
	}

	public void LateUpdate()
	{
		current.LateUpdate();
	}

	public void FixedUpdate()
	{
		current.FixedUpdate();
	}
}
