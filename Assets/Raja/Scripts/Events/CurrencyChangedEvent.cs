public struct CurrencyChangedEvent
{
    public int CurrentCandy { get; }

    public CurrencyChangedEvent(int currentCandy)
    {
        CurrentCandy = currentCandy;
    }
}