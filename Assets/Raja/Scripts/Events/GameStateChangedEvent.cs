public struct GameStateChangedEvent
{
    public GameState PreviousState { get; }
    public GameState CurrentState { get; }

    public GameStateChangedEvent(
        GameState previousState,
        GameState currentState)
    {
        PreviousState = previousState;
        CurrentState = currentState;
    }
}