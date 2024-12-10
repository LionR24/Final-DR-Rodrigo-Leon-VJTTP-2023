using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventStateConfigurer<T>
{
	EventStates<T> instance;
	Dictionary<T, EventTransition<T>> transitions = new Dictionary<T, EventTransition<T>>();

	public EventStateConfigurer(EventStates<T> state)
	{
		instance = state;
	}

	public EventStateConfigurer<T> SetTransition(T input, EventStates<T> target)
	{
		transitions.Add(input, new EventTransition<T>(input, target));
		return this;
	}

	public void Done()
	{
		instance.Configure(transitions);
	}
}

public static class EventStateConfigurer
{
	public static EventStateConfigurer<T> Create<T>(EventStates<T> state)
	{
		return new EventStateConfigurer<T>(state);
	}
}
