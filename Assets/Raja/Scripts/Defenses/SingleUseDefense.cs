using UnityEngine;

public class SingleUseDefense : Defense
{
    private bool hasBeenUsed;

    public bool HasBeenUsed => hasBeenUsed;

    public bool Activate()
    {
        if (hasBeenUsed || IsDestroyed())
            return false;

        hasBeenUsed = true;

        OnActivated();

        Destroy(gameObject);

        return true;
    }

    protected virtual void OnActivated()
    {
        Debug.Log(
            $"{defenseData.DefenseName} activated!",
            this
        );
    }
}