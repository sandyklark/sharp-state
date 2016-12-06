# sharp-state

A versatile and extensible state machine class for c#

## Example

1. Define enums for your states

	```csharp
	public enum State {WALK, RUN, CLIMB, DIE};
	```

2. Define a list of state data

  State data is constructed with the following arguments:
  - state name (enum)
  - state enter callback (can be null)
  - state exit callback (can be null)
  - valid transitions (a list of state name enums) if empty can transition to any state
  - a boolean determining if this state can be transitioned back to from the next state (optional)
  
	
```csharp
	StateData<GameState>[] stateList = new StateData<GameState>[]
	{
		new StateData<GameState> (State.WALK, OnStartWalking, OnStopWalking, new GameState[]{State.RUN, State.CLIMB, State.DIE}),
		new StateData<GameState> (State.RUN, OnStartRunning, OnStopRunning, new GameState[]{GameState.WALK, State.CLIMB, State.DIE}),
		new StateData<GameState> (State.CLIMB, OnStartClimbing, OnStopClimbing, new GameState[]{GameState.WALK, State.DIE}),
		new StateData<GameState> (State.DIE, null, null)
	}
	```

3. Initialize state machine with state list (State machine is generically typed with state enum type)

	```csharp
	StateMachine<State> stateMachine = new StateMachine<State>(stateList);
	```
4. Set initial state

	```csharp
	stateMachine.SetInitialState(State.WALK);
	```  
  
5. Transition to another state

	```csharp
	stateMachine.ChangeState(State.RUN);
	```

6. Add a new state

	```csharp
	StateData<State> newState = new StateData<State> (State.DANCE, OnStartDancing, OnStopDancing);
	stateMachine.AddState(newState);
	```

## License
The MIT License (MIT). Please see [License File](https://github.com/sandyklark/sharp-messenger/blob/master/LICENSE.md) for more information.
