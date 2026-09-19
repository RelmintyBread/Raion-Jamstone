using UnityEngine;

public class EnemyTargetting : MonoBehaviour
{
    [Header("Target Settings")]
    [SerializeField] private Transform target;
    [SerializeField] private string targetTag = "Player";
    [SerializeField] private bool findTargetOnStart = true;

    public Transform Target => target;
    public bool HasTarget => target != null;

    private void Start()
    {
        if (findTargetOnStart && target == null)
            FindTarget();
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    public void FindTarget()
    {
        GameObject targetObject = GameObject.FindGameObjectWithTag(targetTag);

        if (targetObject != null)
            target = targetObject.transform;
    }

    public void ClearTarget()
    {
        target = null;
    }
}