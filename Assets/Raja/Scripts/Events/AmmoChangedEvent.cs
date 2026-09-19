public struct AmmoChangedEvent
{
    public int CurrentAmmo { get; }
    public int CarriedAmmo { get; }

    public AmmoChangedEvent(int currentAmmo, int carriedAmmo)
    {
        CurrentAmmo = currentAmmo;
        CarriedAmmo = carriedAmmo;
    }
}