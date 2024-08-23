namespace MD.Common.SoftwareStateHandling;

/// <summary>
/// Debugs the software state.
/// </summary>
public class StateDebugger
{
    public StateDebugger()
    {
        SoftwareStateManager softwareStateManager = SoftwareStateManager.Instance;
        softwareStateManager.SubscribeToState(State.Shutdown, () =>
        {
            Console.WriteLine("Software state changed to Shutdown.");
        });
    }
}