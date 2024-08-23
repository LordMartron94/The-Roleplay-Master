namespace MD.Common.SoftwareStateHandling;

/// <summary>
/// Manages the software state of the application.
/// </summary>
public sealed class SoftwareStateManager
{
    private static volatile SoftwareStateManager? _instance;
    private readonly static object SyncRoot = new object();

    private readonly Dictionary<State, List<Action>> _stateSubscriptions;
    private Action? _exitHandler;

    private bool _shutdownCalled;

    private SoftwareStateManager()
    {
        _stateSubscriptions = new Dictionary<State, List<Action>>();
        _shutdownCalled = false;
        
        foreach (State state in Enum.GetValues(typeof(State)))
            _stateSubscriptions[state] = new List<Action>();
    }
    
    public static SoftwareStateManager Instance
    {
        get
        {
            if (_instance != null)
                return _instance;

            lock (SyncRoot)
            {
                // ReSharper disable once ConvertIfStatementToNullCoalescingAssignment
                if (_instance == null)
                    _instance = new SoftwareStateManager();
            }

            return _instance;
        }
    }
    
    /// <summary>
    /// Subscribes an action to the specified state.
    /// </summary>
    /// <param name="state">The desired state.</param>
    /// <param name="action">The action.</param>
    /// <remarks>Does nothing if the action is already added to this state.</remarks>
    public void SubscribeToState(State state, Action action)
    {
        if (!_stateSubscriptions[state].Contains(action))
            _stateSubscriptions[state].Add(action);
    }

    /// <summary>
    /// Unsubscribes an action from the specified state.
    /// </summary>
    /// <param name="state">The desired state.</param>
    /// <param name="action">The action.</param>
    public void UnsubscribeFromState(State state, Action action)
    {
        if (_stateSubscriptions.TryGetValue(state, out List<Action>? actions) && actions.Contains(action))
            actions.Remove(action);
    }

    public void SetExitHandler(Action exitHandler)
    {
        if (_exitHandler != null)
            throw new InvalidOperationException("Exit handler already set. Something is wrong if you call this method more than once.");

        _exitHandler = exitHandler;
    }

    /// <summary>
    /// Shuts the application down and notifies other components about the shutdown in advance.
    /// </summary>
    /// <param name="skipExitHandler">If true, the exit handler will not be called.</param>
    /// <remarks>If the application is already shut down, this method does nothing.
    /// <br/>This prevents weird bugs where the subscribers are called multiple times.</remarks>
    public void Shutdown(bool skipExitHandler = false)
    {
        if (_exitHandler == null)
            throw new InvalidOperationException("No exit handler set.");
        
        if (_shutdownCalled)
            return;

        _shutdownCalled = true;
        
        List<Action> subscriptions = _stateSubscriptions[State.Shutdown];
        
        foreach (Action action in subscriptions)
            action.Invoke();
        
        if (!skipExitHandler)
            _exitHandler.Invoke();
    }
}