using UnityEngine;

namespace Triarch
{
    public sealed class TriarchZoneService : MonoBehaviour
    {
        public void AssignZone(TriarchPlayer player, string zoneId)
        {
            if (player == null)
            {
                return;
            }

            var zone = TriarchDataBootstrap.Data?.GetZone(zoneId);
            if (zone == null)
            {
                Debug.LogWarning($"Zone '{zoneId}' not found.");
                player.SetZone(null, RiskTier.None);
                return;
            }

            player.SetZone(zone.Id, zone.RiskTier);
        }

        public void ClearZone(TriarchPlayer player, string zoneId)
        {
            if (player == null)
            {
                return;
            }

            if (player.CurrentZoneId != zoneId)
            {
                return;
            }

            player.SetZone(null, RiskTier.None);
        }
    }
}
