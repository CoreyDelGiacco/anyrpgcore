using UnityEngine;

namespace Triarch
{
    [RequireComponent(typeof(Collider))]
    public sealed class TriarchZoneVolume : MonoBehaviour
    {
        [SerializeField]
        private string zoneId;

        [SerializeField]
        private TriarchZoneService zoneService;

        private void Reset()
        {
            var collider = GetComponent<Collider>();
            collider.isTrigger = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            var player = other.GetComponent<TriarchPlayer>();
            if (player == null)
            {
                return;
            }

            ResolveZoneService()?.AssignZone(player, zoneId);
        }

        private void OnTriggerExit(Collider other)
        {
            var player = other.GetComponent<TriarchPlayer>();
            if (player == null)
            {
                return;
            }

            ResolveZoneService()?.ClearZone(player, zoneId);
        }

        private TriarchZoneService ResolveZoneService()
        {
            if (zoneService == null)
            {
                zoneService = FindObjectOfType<TriarchZoneService>();
            }

            return zoneService;
        }
    }
}
