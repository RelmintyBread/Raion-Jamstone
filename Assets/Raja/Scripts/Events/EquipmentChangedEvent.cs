public struct EquipmentChangedEvent
{
    public int SlotIndex { get; }
    public string WeaponName { get; }

    public EquipmentChangedEvent(int slotIndex, string weaponName)
    {
        SlotIndex = slotIndex;
        WeaponName = weaponName;
    }
}