using UnityEngine;

namespace Triarch
{
    public sealed class TriarchDebugUI : MonoBehaviour
    {
        [SerializeField]
        private TriarchPlayer player;

        [SerializeField]
        private TriarchDeathService deathService;

        private void OnGUI()
        {
            if (player == null)
            {
                player = FindObjectOfType<TriarchPlayer>();
            }

            if (deathService == null)
            {
                deathService = FindObjectOfType<TriarchDeathService>();
            }

            if (player == null || deathService == null)
            {
                return;
            }

            var label = $"Zone: {player.CurrentZoneId ?? "None"} | Risk: {player.CurrentRiskTier}";
            GUI.Label(new Rect(12, 12, 400, 24), label);

            if (GUI.Button(new Rect(12, 40, 120, 32), "Kill Me"))
            {
                deathService.HandleDeath(player);
            }
        }
    }
}
