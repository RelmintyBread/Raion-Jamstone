public struct DefenseChangedEvent
{
    public Defense Defense { get; }
    public bool IsPlaced { get; }
    public int RemainingDefensePoints { get; }

    public DefenseChangedEvent(
        Defense defense,
        bool isPlaced,
        int remainingDefensePoints)
    {
        Defense = defense;
        IsPlaced = isPlaced;
        RemainingDefensePoints = remainingDefensePoints;
    }
}