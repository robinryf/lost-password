// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;
// using Stateless.Reflection;
//
// namespace Stateless
// {
// 	/// <summary>
//     /// 
//     /// </summary>
//     /// <typeparam name="TState"></typeparam>
//     /// <typeparam name="TTrigger"></typeparam>
//     public class StateMachine<TState, TTrigger> : StateMachine<TState, TTrigger, StateMachine<TState, TTrigger>.VoidContext>
//     {
// 	    /// <summary>
// 	    /// 
// 	    /// </summary>
// 	    public class VoidContext : StateMachine<TState,TTrigger,VoidContext>.IStateMachineContext
// 	    {
// 		    private readonly Func<TState> _stateAccessor;
// 		    private readonly Action<TState> _stateMutator;
//
// 		    public VoidContext() : this(null, null)
// 		    {
// 			    
// 		    }
// 		    public VoidContext(Func<TState> stateAccessor, Action<TState> stateMutator)
// 		    {
// 			    _stateAccessor = stateAccessor;
// 			    _stateMutator = stateMutator;
// 		    }
//
// 		    private TState _state;
//
// 		    public TState State
// 		    {
// 			    get => _stateAccessor != null ? _stateAccessor() : _state;
// 			    set
// 			    {
// 				    if (_stateMutator != null)
// 				    {
// 					    _stateMutator(value);
// 				    }
// 				    else
// 				    {
// 					    _state = value;
// 				    }
// 			    }
// 		    }
// 	    }
// 	    
// 	    private readonly TState _initialState;
// 	    
// 	    private readonly VoidContext Context;
// 	    
// 	    
// 	    
// 	            /// <summary>
//         /// Construct a state machine with external state storage.
//         /// </summary>
//         /// <param name="stateAccessor">A function that will be called to read the current state value.</param>
//         /// <param name="stateMutator">An action that will be called to write new state values.</param>
//         public StateMachine(Func<TState> stateAccessor, Action<TState> stateMutator) :this(stateAccessor, stateMutator, FiringMode.Queued)
//         {
//         }
//
//         /// <summary>
//         /// Construct a state machine.
//         /// </summary>
//         /// <param name="initialState">The initial state.</param>
//         public StateMachine(TState initialState) : this(initialState, FiringMode.Queued)
//         {
//         }
//
//         /// <summary>
//         /// Construct a state machine with external state storage.
//         /// </summary>
//         /// <param name="stateAccessor">A function that will be called to read the current state value.</param>
//         /// <param name="stateMutator">An action that will be called to write new state values.</param>
//         /// <param name="firingMode">Optional specification of firing mode.</param>
//         public StateMachine(Func<TState> stateAccessor, Action<TState> stateMutator, FiringMode firingMode) : base()
//         {
//             var _stateAccessor = stateAccessor ?? throw new ArgumentNullException(nameof(stateAccessor));
//             var _stateMutator = stateMutator ?? throw new ArgumentNullException(nameof(stateMutator));
//
//             Context = new VoidContext(stateAccessor, stateMutator);
//
//             _initialState = stateAccessor();
//             _firingMode = firingMode;
//         }
//
//         /// <summary>
//         /// Construct a state machine.
//         /// </summary>
//         /// <param name="initialState">The initial state.</param>
//         /// <param name="firingMode">Optional specification of firing mode.</param>
//         public StateMachine(TState initialState, FiringMode firingMode) : base()
//         {
// 	        var context = new VoidContext() { State = initialState };
//
//             _initialState = initialState;
//             _firingMode = firingMode;
//         }
//
//                 /// <summary>
//         /// The current state.
//         /// </summary>
//         public TState State
//         {
//             get => Context.State;
//             private set => Context.State = value;
//         }
//                 
//         public new IEnumerable<TTrigger> GetPermittedTriggers(params object[] args)
//         {
// 	        return base.GetPermittedTriggers(State, args);
//         }
//
//         /// <summary>
//         /// Gets the currently-permissible triggers with any configured parameters.
//         /// </summary>
//         public new IEnumerable<TriggerDetails<TState, TTrigger, VoidContext>> GetDetailedPermittedTriggers(params object[] args)
//         {
// 	        return base.GetDetailedPermittedTriggers(State, args);
//         }
//
//         /// <summary>
//         /// Provides an info object which exposes the states, transitions, and actions of this machine.
//         /// </summary>
//         public StateMachineInfo GetInfo()
//         {
// 	        return base.GetInfo(_initialState);
//         }
//
//         /// <summary>
//         /// Determine if the state machine is in the supplied state.
//         /// </summary>
//         /// <param name="state">The state to test for.</param>
//         /// <returns>True if the current state is equal to, or a substate of,
//         /// the supplied state.</returns>
//         public bool IsInState(TState state)
//         {
// 	        return base.IsInState(State, state);
//         }
//
//         /// <summary>
//         /// Returns true if <paramref name="trigger"/> can be fired
//         /// in the current state.
//         /// </summary>
//         /// <param name="trigger">Trigger to test.</param>
//         /// <returns>True if the trigger can be fired, false otherwise.</returns>
//         public bool CanFire(TTrigger trigger)
//         {
// 	        return base.CanFire(State, trigger);
//         }
//
//         /// <summary>
//         /// Returns true if <paramref name="trigger"/> can be fired
//         /// in the current state.
//         /// </summary>
//         /// <param name="trigger">Trigger to test.</param>
//         /// <param name="unmetGuards">Guard descriptions of unmet guards. If given trigger is not configured for current state, this will be null.</param>
//         /// <returns>True if the trigger can be fired, false otherwise.</returns>
//         public bool CanFire(TTrigger trigger, out ICollection<string> unmetGuards)
//         {
// 	        return base.CanFire(State, trigger, out unmetGuards);
//         }
//
//         /// <summary>
//         /// A human-readable representation of the state machine.
//         /// </summary>
//         /// <returns>A description of the current state and permitted triggers.</returns>
//         public override string ToString()
//         {
// 	        return string.Format(
// 		        "StateMachine {{ State = {0}, PermittedTriggers = {{ {1} }}}}",
// 		        State,
// 		        string.Join(", ", GetPermittedTriggers().Select(t => t.ToString()).ToArray()));
//         }
// 	    
// 	    
// 	    /// <summary>
//         /// Transition from the current state via the specified trigger.
//         /// The target state is determined by the configuration of the current state.
//         /// Actions associated with leaving the current state and entering the new one
//         /// will be invoked.
//         /// </summary>
//         /// <param name="trigger">The trigger to fire.</param>
//         /// <param name="context"></param>
//         /// <exception cref="System.InvalidOperationException">The current state does
//         /// not allow the trigger to be fired.</exception>
//         public void Fire(TTrigger trigger)
//         {
//             InternalFire(trigger, Context, new object[0]);
//         }
//
//         /// <summary>
//         /// Transition from the current state via the specified trigger.
//         /// The target state is determined by the configuration of the current state.
//         /// Actions associated with leaving the current state and entering the new one
//         /// will be invoked.
//         /// </summary>
//         /// <param name="trigger">The trigger to fire.</param>
//         /// <param name="context"></param>
//         /// <param name="args">A variable-length parameters list containing arguments. </param>
//         /// <exception cref="System.InvalidOperationException">The current state does
//         /// not allow the trigger to be fired.</exception>
//         public void Fire(TriggerWithParameters trigger, params object[] args)
//         {
//             if (trigger == null) throw new ArgumentNullException(nameof(trigger));
//             InternalFire(trigger.Trigger, Context, args);
//         }
//
//         /// <summary>
//         /// Transition from the current state via the specified trigger.
//         /// The target state is determined by the configuration of the current state.
//         /// Actions associated with leaving the current state and entering the new one
//         /// will be invoked.
//         /// </summary>
//         /// <typeparam name="TArg0">Type of the first trigger argument.</typeparam>
//         /// <param name="trigger">The trigger to fire.</param>
//         /// <param name="context"></param>
//         /// <param name="arg0">The first argument.</param>
//         /// <exception cref="System.InvalidOperationException">The current state does
//         /// not allow the trigger to be fired.</exception>
//         public void Fire<TArg0>(TriggerWithParameters<TArg0> trigger, TArg0 arg0)
//         {
//             if (trigger == null) throw new ArgumentNullException(nameof(trigger));
//             InternalFire(trigger.Trigger, Context, arg0);
//         }
//
//         /// <summary>
//         /// Transition from the current state via the specified trigger.
//         /// The target state is determined by the configuration of the current state.
//         /// Actions associated with leaving the current state and entering the new one
//         /// will be invoked.
//         /// </summary>
//         /// <typeparam name="TArg0">Type of the first trigger argument.</typeparam>
//         /// <typeparam name="TArg1">Type of the second trigger argument.</typeparam>
//         /// <param name="context"></param>
//         /// <param name="arg0">The first argument.</param>
//         /// <param name="arg1">The second argument.</param>
//         /// <param name="trigger">The trigger to fire.</param>
//         /// <exception cref="System.InvalidOperationException">The current state does
//         /// not allow the trigger to be fired.</exception>
//         public void Fire<TArg0, TArg1>(TriggerWithParameters<TArg0, TArg1> trigger, TArg0 arg0, TArg1 arg1)
//         {
//             if (trigger == null) throw new ArgumentNullException(nameof(trigger));
//             InternalFire(trigger.Trigger, Context, arg0, arg1);
//         }
//
//         /// <summary>
//         /// Transition from the current state via the specified trigger.
//         /// The target state is determined by the configuration of the current state.
//         /// Actions associated with leaving the current state and entering the new one
//         /// will be invoked.
//         /// </summary>
//         /// <typeparam name="TArg0">Type of the first trigger argument.</typeparam>
//         /// <typeparam name="TArg1">Type of the second trigger argument.</typeparam>
//         /// <typeparam name="TArg2">Type of the third trigger argument.</typeparam>
//         /// <param name="context"></param>
//         /// <param name="arg0">The first argument.</param>
//         /// <param name="arg1">The second argument.</param>
//         /// <param name="arg2">The third argument.</param>
//         /// <param name="trigger">The trigger to fire.</param>
//         /// <exception cref="System.InvalidOperationException">The current state does
//         /// not allow the trigger to be fired.</exception>
//         public void Fire<TArg0, TArg1, TArg2>(TriggerWithParameters<TArg0, TArg1, TArg2> trigger, TArg0 arg0, TArg1 arg1, TArg2 arg2)
//         {
//             if (trigger == null) throw new ArgumentNullException(nameof(trigger));
//             InternalFire(trigger.Trigger, Context, arg0, arg1, arg2);
//         }
//
//         /// <summary>
//         /// Activates current state. Actions associated with activating the current state
//         /// will be invoked. The activation is idempotent and subsequent activation of the same current state
//         /// will not lead to re-execution of activation callbacks.
//         /// </summary>
//         public void Activate()
//         {
// 	        base.Activate(State);
//         }
//
//         /// <summary>
//         /// Deactivates current state. Actions associated with deactivating the current state
//         /// will be invoked. The deactivation is idempotent and subsequent deactivation of the same current state
//         /// will not lead to re-execution of deactivation callbacks.
//         /// </summary>
//         public void Deactivate()
//         {
//             base.Deactivate(State);
//         }
// 	    
// 	    
// 	    // /// <summary>
// 	    // /// 
// 	    // /// </summary>
// 	    // /// <param name="stateAccessor"></param>
// 	    // /// <param name="stateMutator"></param>
// 	    // public StateMachine(Func<TState> stateAccessor, Action<TState> stateMutator) : base(stateAccessor, stateMutator)
// 	    // {
// 	    // }
//      //
// 	    // /// <summary>
// 	    // /// 
// 	    // /// </summary>
// 	    // /// <param name="initialState"></param>
// 	    // public StateMachine(TState initialState) : base(initialState)
// 	    // {
// 	    // }
//      //
// 	    // /// <summary>
// 	    // /// 
// 	    // /// </summary>
// 	    // /// <param name="stateAccessor"></param>
// 	    // /// <param name="stateMutator"></param>
// 	    // /// <param name="firingMode"></param>
// 	    // public StateMachine(Func<TState> stateAccessor, Action<TState> stateMutator, FiringMode firingMode) : base((state, ) => stateAccessor(), stateMutator, firingMode)
// 	    // {
// 	    // }
//      //
// 	    // /// <summary>
// 	    // /// 
// 	    // /// </summary>
// 	    // /// <param name="initialState"></param>
// 	    // /// <param name="firingMode"></param>
// 	    // public StateMachine(TState initialState, FiringMode firingMode) : base(initialState, firingMode)
// 	    // {
// 	    // }
// 	    //
// 	    // /// <summary>
//      //    /// Construct a state machine with external state storage.
//      //    /// </summary>
//      //    /// <param name="stateAccessor">A function that will be called to read the current state value.</param>
//      //    /// <param name="stateMutator">An action that will be called to write new state values.</param>
//      //    public StateMachine(Func<TContext, TState> stateAccessor, Action<TState, TContext> stateMutator) :this(stateAccessor, stateMutator, FiringMode.Queued)
//      //    {
//      //    }
//      //
//      //    /// <summary>
//      //    /// Construct a state machine.
//      //    /// </summary>
//      //    /// <param name="initialState">The initial state.</param>
//      //    public StateMachine(TState initialState) : this(initialState, FiringMode.Queued)
//      //    {
//      //    }
//      //
//      //    /// <summary>
//      //    /// Construct a state machine with external state storage.
//      //    /// </summary>
//      //    /// <param name="stateAccessor">A function that will be called to read the current state value.</param>
//      //    /// <param name="stateMutator">An action that will be called to write new state values.</param>
//      //    /// <param name="firingMode">Optional specification of firing mode.</param>
//      //    public StateMachine(Func<TContext, TState> stateAccessor, Action<TState, TContext> stateMutator, FiringMode firingMode) : this()
//      //    {
//      //        _stateAccessor = stateAccessor ?? throw new ArgumentNullException(nameof(stateAccessor));
//      //        _stateMutator = stateMutator ?? throw new ArgumentNullException(nameof(stateMutator));
//      //
//      //        _initialState = stateAccessor();
//      //        _firingMode = firingMode;
//      //    }
//      //
//      //    /// <summary>
//      //    /// Construct a state machine.
//      //    /// </summary>
//      //    /// <param name="initialState">The initial state.</param>
//      //    /// <param name="firingMode">Optional specification of firing mode.</param>
//      //    public StateMachine(TState initialState, FiringMode firingMode) : this()
//      //    {
//      //        var reference = new StateReference { State = initialState };
//      //        _stateAccessor = (context) => reference.State;
//      //        _stateMutator = (s, context) => reference.State = s;
//      //
//      //        _initialState = initialState;
//      //        _firingMode = firingMode;
//      //    }
// 	    //
// 	    // /// <summary>
// 	    // /// The current state.
// 	    // /// </summary>
// 	    // public TState State
// 	    // {
// 		   //  get
// 		   //  {
// 			  //   return _stateAccessor(Context);
// 		   //  }
// 		   //  private set
// 		   //  {
// 			  //   _stateMutator(value, Context);
// 		   //  }
// 	    // }
// 	    
// 	    // /// <summary>
// 	    // /// 
// 	    // /// </summary>
// 	    // /// <param name="trigger"></param>
// 	    // public void Fire(TTrigger trigger)
// 	    // {
// 		   //  InternalFire(trigger, Context, new object[0]);
// 	    // }
// 	    //
// 	    // public Task FireAsync(TTrigger trigger)
// 	    // {
// 		   //  return FireAsync(trigger, Context);
// 	    // }
// 	    //
// 	    // /// <summary>
// 	    // /// 
// 	    // /// </summary>
// 	    // /// <param name="trigger"></param>
// 	    // /// <param name="args"></param>
// 	    // public void Fire(TriggerWithParameters trigger, params object[] args)
// 	    // {
// 		   //  base.Fire(trigger, Context, args);
// 	    // }
//
// 	    public override StateMachine<TState, TTrigger, VoidContext>.Transition CreateTransition(TState source, TState destination, TTrigger trigger, VoidContext context,
// 		    object[] parameters = null)
// 	    {
// 		    return new Transition(source, destination, trigger, Context, parameters);
// 	    }
//
// 	    /// <summary>
// 	    /// 
// 	    /// </summary>
// 	    public new class Transition : StateMachine<TState, TTrigger, VoidContext>.Transition
// 	    {
// 		    /// <summary>
// 		    /// 
// 		    /// </summary>
// 		    /// <param name="source"></param>
// 		    /// <param name="destination"></param>
// 		    /// <param name="trigger"></param>
// 		    /// <param name="parameters"></param>
// 		    public Transition(TState source, TState destination, TTrigger trigger, VoidContext context = null, object[] parameters = null) : base(source, destination, trigger, context, parameters)
// 		    {
// 		    }
// 	    }
//
// 	    /// <summary>
// 	    /// 
// 	    /// </summary>
// 	    protected new partial class StateRepresentation : StateMachine<TState, TTrigger, VoidContext>.StateRepresentation
// 	    {
// 		    /// <summary>
// 		    /// 
// 		    /// </summary>
// 		    /// <param name="state"></param>
// 		    /// <param name="retainSynchronizationContext"></param>
// 		    public StateRepresentation(TState state, bool retainSynchronizationContext = false) : base(state, retainSynchronizationContext)
// 		    {
// 		    }
//
// 		    public void AddEntryAction(Action<Transition, object[]> action, Reflection.InvocationInfo entryActionDescription)
// 		    {
// 			    base.AddEntryAction((transition, objects) => action((Transition)transition, objects), entryActionDescription);
// 		    }
//
// 		    public void AddExitAction(Action<Transition> action, Reflection.InvocationInfo exitActionDescription)
// 		    {
// 			    base.AddExitAction(transition => action((Transition)transition), exitActionDescription);
// 		    }
// 	    }
// 	    
// 	    public new StateConfiguration Configure(TState state)
// 	    {
// 		    return new StateConfiguration(base.Configure(state));
// 		    return new StateConfiguration(this, GetRepresentation(state), GetRepresentation);
// 	    }
//
// 	    /// <summary>
// 	    /// 
// 	    /// </summary>
// 	    public new partial class StateConfiguration : StateMachine<TState, TTrigger, VoidContext>.StateConfiguration
// 	    {
// 		    internal StateConfiguration(StateMachine<TState, TTrigger, VoidContext>.StateConfiguration source) : base(source.Machine, )
// 		    {
// 			    
// 		    }
// 		    internal StateConfiguration(StateMachine<TState, TTrigger> machine, StateRepresentation representation, Func<TState, StateRepresentation> lookup)
// 		    {
// 			    _machine = machine;
// 			    _representation = representation;
// 			    _lookup = lookup;
// 		    }
//
// 		    public new StateMachine<TState, TTrigger> Machine => base._machine;
//
// 		    public new StateConfiguration Permit(TTrigger trigger, TState destinationState)
// 		    {
// 			    return (StateConfiguration)base.Permit(trigger, destinationState);
// 		    }
//
// 		    public new StateConfiguration SubstateOf(TState superstate)
// 		    {
// 			    return (StateConfiguration)base.SubstateOf(superstate);
// 		    }
// 	    }
//
// 	    /// <summary>
// 	    /// 
// 	    /// </summary>
// 	    /// <param name="onTransitionAction"></param>
// 	    /// <exception cref="ArgumentNullException"></exception>
// 	    public void OnTransitioned(Action<Transition> onTransitionAction)
// 	    {
// 		    base.OnTransitioned(transition => onTransitionAction((Transition)transition));
// 	    }
//
// 	    /// <summary>
// 	    /// 
// 	    /// </summary>
// 	    /// <param name="onTransitionAction"></param>
// 	    public void OnTransitionCompleted(Action<Transition> onTransitionAction)
// 	    {
// 		    base.OnTransitionCompleted(transition => onTransitionAction((Transition)transition));
// 	    }
//     }
// }