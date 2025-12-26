using UnityEngine;

namespace Triarch
{
    public sealed class TriarchPlayer : MonoBehaviour
    {
        [SerializeField]
        private string playerId = "player";

        [SerializeField]
        private TriarchInventory inventory;

        public string PlayerId => playerId;
        public string CurrentZoneId { get; private set; }
        public RiskTier CurrentRiskTier { get; private set; } = RiskTier.None;

        public TriarchInventory Inventory
        {
            get
            {
                if (inventory == null)
                {
                    inventory = GetComponent<TriarchInventory>();
                }

                return inventory;
            }
        }

        public void SetZone(string zoneId, RiskTier riskTier)
        {
            CurrentZoneId = zoneId;
            CurrentRiskTier = riskTier;
        }
    }
}
