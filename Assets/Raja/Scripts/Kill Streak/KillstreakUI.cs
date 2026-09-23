using UnityEngine;
using TMPro;

public class KillstreakUI : MonoBehaviour
{
    [SerializeField] private KillstreakSystem killstreakSystem;
    [SerializeField] private TMP_Text killstreakText;

    private void OnEnable()
    {
        if (killstreakSystem != null)
        {
            killstreakSystem.OnKillstreakChanged
                += UpdateKillstreakUI;
        }
    }

    private void OnDisable()
    {
        if (killstreakSystem != null)
        {
            killstreakSystem.OnKillstreakChanged
                -= UpdateKillstreakUI;
        }
    }

    private void UpdateKillstreakUI(int killstreak)
    {
        if (killstreakText != null)
        {
            killstreakText.text =
                $"Killstreak: {killstreak}";
        }
    }
}