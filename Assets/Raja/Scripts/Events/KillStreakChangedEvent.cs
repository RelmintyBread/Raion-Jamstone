public struct KillStreakChangedEvent
{
    public int CurrentStreak { get; }
    public float Multiplier { get; }

    public KillStreakChangedEvent(
        int currentStreak,
        float multiplier)
    {
        CurrentStreak = currentStreak;
        Multiplier = multiplier;
    }
}