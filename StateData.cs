using System.Collections;

public class StateData<StateEnum>
{
	public delegate void StateCallback();

	public StateEnum stateName;
	public StateCallback OnEnterStateCallback;
	public StateCallback OnExitStateCallback;
	public StateEnum[] allowedStateTransitions;
	public bool canBeRestored;

	public StateData(StateEnum name, StateCallback enterStateCallback = null, StateCallback exitStateCallback = null, StateEnum[] allowedTransitions = null, bool restorable = true)
	{
		stateName = name;
		OnEnterStateCallback = enterStateCallback;
		OnExitStateCallback = exitStateCallback;
		allowedStateTransitions = allowedTransitions;
		canBeRestored = restorable;
	}
}

