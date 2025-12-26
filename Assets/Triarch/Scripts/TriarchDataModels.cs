using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Triarch
{
    public enum RiskTier
    {
        None = 0,
        SAFE = 1,
        CONTESTED = 2,
        HIGH_RISK = 3
    }

    public enum LootDropMode
    {
        NONE = 0,
        PARTIAL = 1,
        FULL = 2
    }

    [Serializable]
    public sealed class FactionDefinition
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }

    [Serializable]
    public sealed class ZoneDefinition
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("riskTier")]
        public RiskTier RiskTier { get; set; }
    }

    [Serializable]
    public sealed class DeathRuleDefinition
    {
        [JsonProperty("riskTier")]
        public RiskTier RiskTier { get; set; }

        [JsonProperty("lootDropMode")]
        public LootDropMode LootDropMode { get; set; }

        [JsonProperty("goldDropPct")]
        public float GoldDropPct { get; set; }

        [JsonProperty("protectEquipped")]
        public bool ProtectEquipped { get; set; }
    }

    [Serializable]
    public sealed class FactionData
    {
        [JsonProperty("factions")]
        public List<FactionDefinition> Factions { get; set; } = new List<FactionDefinition>();
    }

    [Serializable]
    public sealed class ZoneData
    {
        [JsonProperty("zones")]
        public List<ZoneDefinition> Zones { get; set; } = new List<ZoneDefinition>();
    }

    [Serializable]
    public sealed class DeathRulesData
    {
        [JsonProperty("rules")]
        public List<DeathRuleDefinition> Rules { get; set; } = new List<DeathRuleDefinition>();
    }

    public sealed class TriarchDataRepository
    {
        public IReadOnlyDictionary<string, FactionDefinition> FactionsById { get; }
        public IReadOnlyDictionary<string, ZoneDefinition> ZonesById { get; }
        public IReadOnlyDictionary<RiskTier, DeathRuleDefinition> DeathRulesByRisk { get; }

        public TriarchDataRepository(
            Dictionary<string, FactionDefinition> factionsById,
            Dictionary<string, ZoneDefinition> zonesById,
            Dictionary<RiskTier, DeathRuleDefinition> deathRulesByRisk)
        {
            FactionsById = factionsById ?? throw new ArgumentNullException(nameof(factionsById));
            ZonesById = zonesById ?? throw new ArgumentNullException(nameof(zonesById));
            DeathRulesByRisk = deathRulesByRisk ?? throw new ArgumentNullException(nameof(deathRulesByRisk));
        }

        public ZoneDefinition GetZone(string zoneId)
        {
            if (string.IsNullOrWhiteSpace(zoneId))
            {
                return null;
            }

            return ZonesById.TryGetValue(zoneId, out var zone) ? zone : null;
        }

        public DeathRuleDefinition GetDeathRule(RiskTier riskTier)
        {
            return DeathRulesByRisk.TryGetValue(riskTier, out var rule) ? rule : null;
        }
    }
}
