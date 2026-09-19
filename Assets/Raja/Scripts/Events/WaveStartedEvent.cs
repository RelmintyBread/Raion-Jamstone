public struct WaveStartedEvent
{
    public int WaveNumber { get; }

    public WaveStartedEvent(int waveNumber)
    {
        WaveNumber = waveNumber;
    }
}