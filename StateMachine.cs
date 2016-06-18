using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class StateMachine<StateEnum>
{
	public StateEnum currentState
	{
		get 
		{
			return _currentStateEnum;
		}
	}

	protected StateEnum _currentStateEnum;
	protected StateEnum _lastStateEnum;
	protected bool _transitioning = false;
	protected Dictionary<StateEnum, StateData<StateEnum>> _states = new Dictionary<StateEnum, StateData<StateEnum>>();

	//CONSTRUCTOR
	/**
	 * <summary>State Machine Constructor</summary>
	 * <param name="stateList">The List of StateData objecvts defining the states and their callbacks and allowed transitions</param>
	 * <param name="initialStateIndex">The index of the stateList to use as the initial state</param>
	 **/
	public StateMachine(StateData<StateEnum>[] stateList)
	{
		for (int i = 0; i < stateList.Length; i++)
		{
			AddState (stateList[i]);
		}
	}

	//PROTECTED METHODS

	/**
	 * <summary>Method for adding new states to the state machine</summary>
	 * <param name="stateData">The state data defining the state to add</param>
	 **/
	protected void AddState(StateData<StateEnum> stateData)
	{
		//CHECK STATE DOESNT AREADY EXIST
		if(!_states.ContainsKey(stateData.stateName))
		{
			//ADD STATE 
			_states.Add(stateData.stateName, stateData);
		}else{
			Debug.Log("State: " + stateData.stateName.ToString() + " already exists.");
		}
	}

	//PUBLIC METHODS
	public void SetInitialState(StateEnum stateName)
	{
		Debug.Log ("SET INITIAL STATE: " + stateName);

		_transitioning = true;

		//CHECK STATE EXISTS
		if(_states.ContainsKey(stateName))
		{
			//SET NEW STATE
			_currentStateEnum = stateName;

			//CHECK FOR ENTER STATE CALLBACK
			if(_states[stateName].OnEnterStateCallback != null)
			{
				_states[stateName].OnEnterStateCallback();
			}

		}

		_transitioning = false;
	}

	public void ChangeState(StateEnum stateName)
	{
		Debug.Log (_currentStateEnum + " -> " + stateName);

		StateData<StateEnum> current = _states[_currentStateEnum];

		//CHECK IF WE ARE MID TRANSITION
		if (_transitioning)
		{
			throw new Exception ("Cannot change state while currently transitioning");
		}

		//LOCK STATE CHANGE
		_transitioning = true;

		//IF IN THIS STATE ALREADY
		if(_currentStateEnum.Equals(stateName)) return;

		//CHECK STATE EXISTS
		if(_states.ContainsKey(stateName))
		{
			//CHECK NEW STATE IS LEGAL TRANSITION
			if(current.allowedStateTransitions != null)
			{
				bool transitionAllowed = false;
				for(int i = 0; i < current.allowedStateTransitions.Length; i++)
				{
					if(current.allowedStateTransitions[i].Equals(stateName))
					{
						transitionAllowed = true;
						break;
					}
				}
				//NOT A LEGAL TRANSITION
				if(!transitionAllowed)
				{
					Debug.Log("Cannot transition from state " + _currentStateEnum.ToString() + " to state " + stateName.ToString());
					return;
				}
			}
			 
			//SET LAST STATE AS OLD STATE IF IT WAS RESTORABLE
			if(current.canBeRestored)
			{
				_lastStateEnum = _currentStateEnum;
			}


			//CHECK FOR EXIT STATE CALLBACK
			if(current.OnExitStateCallback != null)
			{
				current.OnExitStateCallback();
			}

			//SET NEW STATE
			_currentStateEnum = stateName;

			//CHECK FOR ENTER STATE CALLBACK
			if(_states[stateName].OnEnterStateCallback != null)
			{
				_states[stateName].OnEnterStateCallback();
			}
		}else{
			Debug.Log("State: " + stateName.ToString() + " does not exist.");
		}

		//UNLOCK STATE CHANGE
		_transitioning = false;
	}

	public void ReturnToLastState()
	{
		ChangeState(_lastStateEnum);
	}
}
