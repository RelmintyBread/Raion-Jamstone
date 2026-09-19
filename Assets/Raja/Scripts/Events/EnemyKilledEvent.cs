public struct EnemyKilledEvent
{
    public Enemy Enemy { get; }

    public EnemyKilledEvent(Enemy enemy)
    {
        Enemy = enemy;
    }
}