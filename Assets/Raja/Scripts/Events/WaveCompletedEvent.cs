public struct WaveCompletedEvent
{
    public int WaveNumber { get; }

    public WaveCompletedEvent(int waveNumber)
    {
        WaveNumber = waveNumber;
    }
}