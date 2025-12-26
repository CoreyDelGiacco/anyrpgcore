using System.Collections.Generic;
using UnityEngine;

namespace Triarch
{
    public sealed class TriarchDeathService : MonoBehaviour
    {
        [SerializeField]
        private GameObject lootContainerPrefab;

        public bool IsServerAuthority { get; set; } = true;

        public TriarchLootContainer HandleDeath(TriarchPlayer player)
        {
            if (!IsServerAuthority)
            {
                Debug.LogWarning("Death handling attempted without server authority.");
                return null;
            }

            if (player == null)
            {
                return null;
            }

            var rule = TriarchDataBootstrap.Data?.GetDeathRule(player.CurrentRiskTier);
            if (rule == null)
            {
                Debug.LogWarning($"No death rule defined for risk tier {player.CurrentRiskTier}.");
                return null;
            }

            var inventory = player.Inventory;
            if (inventory == null)
            {
                Debug.LogWarning("Player inventory missing.");
                return null;
            }

            var drops = inventory.CollectDrops(rule.LootDropMode, rule.ProtectEquipped);
            if (drops.Count == 0)
            {
                return null;
            }

            var container = CreateLootContainer(player.transform.position, drops);
            return container;
        }

        private TriarchLootContainer CreateLootContainer(Vector3 position, List<TriarchItemStack> drops)
        {
            var containerObject = lootContainerPrefab != null
                ? Instantiate(lootContainerPrefab, position, Quaternion.identity)
                : new GameObject("LootContainer");

            if (lootContainerPrefab == null)
            {
                containerObject.transform.position = position;
            }

            var container = containerObject.GetComponent<TriarchLootContainer>();
            if (container == null)
            {
                container = containerObject.AddComponent<TriarchLootContainer>();
            }

            container.Initialize(drops);
            return container;
        }
    }
}
