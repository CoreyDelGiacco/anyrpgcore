using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Triarch
{
    public static class TriarchDataLoader
    {
        private const string DataFolderName = "TriarchData";
        private const string SchemaFolderName = "schemas";

        public static TriarchDataRepository LoadFromStreamingAssets()
        {
            var rootPath = Path.Combine(Application.streamingAssetsPath, DataFolderName);
            return LoadFromPath(rootPath);
        }

        public static TriarchDataRepository LoadFromPath(string rootPath)
        {
            if (string.IsNullOrWhiteSpace(rootPath))
            {
                throw new ArgumentException("Root path is required.", nameof(rootPath));
            }

            var schemasPath = Path.Combine(rootPath, SchemaFolderName);
            var factionsJson = ReadJson(Path.Combine(rootPath, "factions.json"));
            var zonesJson = ReadJson(Path.Combine(rootPath, "zones.json"));
            var deathRulesJson = ReadJson(Path.Combine(rootPath, "deathRules.json"));

            ValidateAgainstSchema(factionsJson, Path.Combine(schemasPath, "factions.schema.json"), "factions.json");
            ValidateAgainstSchema(zonesJson, Path.Combine(schemasPath, "zones.schema.json"), "zones.json");
            ValidateAgainstSchema(deathRulesJson, Path.Combine(schemasPath, "deathRules.schema.json"), "deathRules.json");

            var factions = JsonConvert.DeserializeObject<FactionData>(factionsJson);
            var zones = JsonConvert.DeserializeObject<ZoneData>(zonesJson);
            var rules = JsonConvert.DeserializeObject<DeathRulesData>(deathRulesJson);

            return BuildRepository(factions, zones, rules);
        }

        private static string ReadJson(string path)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"Triarch data file not found: {path}");
            }

            return File.ReadAllText(path);
        }

        private static void ValidateAgainstSchema(string json, string schemaPath, string label)
        {
            if (!File.Exists(schemaPath))
            {
                throw new FileNotFoundException($"Triarch schema file not found: {schemaPath}");
            }

            var dataToken = JToken.Parse(json);
            var schemaToken = JToken.Parse(File.ReadAllText(schemaPath));
            var result = TriarchSchemaValidator.Validate(dataToken, schemaToken);

            if (!result.IsValid)
            {
                var joined = string.Join("\n", result.Errors);
                throw new InvalidOperationException($"Triarch schema validation failed for {label}:\n{joined}");
            }
        }

        private static TriarchDataRepository BuildRepository(FactionData factions, ZoneData zones, DeathRulesData rules)
        {
            if (factions == null || zones == null || rules == null)
            {
                throw new InvalidOperationException("Triarch data files failed to parse.");
            }

            var factionLookup = new Dictionary<string, FactionDefinition>(StringComparer.OrdinalIgnoreCase);
            foreach (var faction in factions.Factions)
            {
                if (faction == null || string.IsNullOrWhiteSpace(faction.Id))
                {
                    throw new InvalidOperationException("Faction entries must include an id.");
                }

                if (factionLookup.ContainsKey(faction.Id))
                {
                    throw new InvalidOperationException($"Duplicate faction id '{faction.Id}'.");
                }

                factionLookup[faction.Id] = faction;
            }

            var zoneLookup = new Dictionary<string, ZoneDefinition>(StringComparer.OrdinalIgnoreCase);
            foreach (var zone in zones.Zones)
            {
                if (zone == null || string.IsNullOrWhiteSpace(zone.Id))
                {
                    throw new InvalidOperationException("Zone entries must include an id.");
                }

                if (zoneLookup.ContainsKey(zone.Id))
                {
                    throw new InvalidOperationException($"Duplicate zone id '{zone.Id}'.");
                }

                zoneLookup[zone.Id] = zone;
            }

            var deathRuleLookup = new Dictionary<RiskTier, DeathRuleDefinition>();
            foreach (var rule in rules.Rules)
            {
                if (rule == null)
                {
                    continue;
                }

                if (deathRuleLookup.ContainsKey(rule.RiskTier))
                {
                    throw new InvalidOperationException($"Duplicate death rule for risk tier '{rule.RiskTier}'.");
                }

                deathRuleLookup[rule.RiskTier] = rule;
            }

            return new TriarchDataRepository(factionLookup, zoneLookup, deathRuleLookup);
        }
    }
}
