using System.Collections.Generic;
using UnityEngine;

public class DefenseManager : MonoBehaviour
{
    [Header("Defense Points")]
    [SerializeField, Min(0)] private int defensePoints = 5;

    [Header("Placement Points")]
    [SerializeField] private Transform[] defensePointsInScene;

    private readonly List<Defense> placedDefenses =
        new List<Defense>();

    public int CurrentDefensePoints => defensePoints;

    public IReadOnlyList<Defense> PlacedDefenses =>
        placedDefenses;

    public bool CanPlace(DefenseData data)
    {
        if (data == null || data.DefensePrefab == null)
            return false;

        if (defensePoints < data.DefensePointCost)
            return false;

        return true;
    }

    public bool PlaceDefense(
        DefenseData data,
        int placementIndex
    )
    {
        if (!CanPlace(data))
        {
            Debug.LogWarning(
                "Defense tidak dapat dipasang: data tidak valid " +
                "atau Defense Point tidak cukup.",
                this
            );

            return false;
        }

        if (defensePointsInScene == null ||
            placementIndex < 0 ||
            placementIndex >= defensePointsInScene.Length)
        {
            Debug.LogWarning(
                "Placement index tidak valid.",
                this
            );

            return false;
        }

        Transform placementPoint =
            defensePointsInScene[placementIndex];

        if (placementPoint == null)
        {
            Debug.LogWarning(
                "Placement point belum di-assign.",
                this
            );

            return false;
        }

        GameObject defenseObject = Instantiate(
            data.DefensePrefab,
            placementPoint.position,
            placementPoint.rotation
        );

        Defense defense =
            defenseObject.GetComponent<Defense>();

        if (defense == null)
        {
            Debug.LogError(
                "Defense prefab tidak memiliki component Defense.",
                defenseObject
            );

            Destroy(defenseObject);
            return false;
        }

        defense.Initialize(data);

        defensePoints -= data.DefensePointCost;
        placedDefenses.Add(defense);

        return true;
    }

    public void AddDefensePoints(int amount)
    {
        if (amount <= 0)
            return;

        defensePoints += amount;
    }

    public bool SpendDefensePoints(int amount)
    {
        if (amount <= 0 || defensePoints < amount)
            return false;

        defensePoints -= amount;
        return true;
    }

    public void RemoveDefense(Defense defense)
    {
        if (defense == null)
            return;

        placedDefenses.Remove(defense);
        Destroy(defense.gameObject);
    }

    public void RemoveAllDefenses()
    {
        foreach (Defense defense in placedDefenses)
        {
            if (defense != null)
                Destroy(defense.gameObject);
        }

        placedDefenses.Clear();
    }
}