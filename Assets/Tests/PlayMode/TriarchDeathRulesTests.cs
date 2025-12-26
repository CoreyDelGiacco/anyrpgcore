using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Triarch;
using UnityEngine;
using UnityEngine.TestTools;

public class TriarchDeathRulesTests
{
    [UnityTest]
    public IEnumerator SafeZone_KillDoesNotSpawnLootContainer()
    {
        var bootstrapObject = new GameObject("TriarchDataBootstrap");
        bootstrapObject.AddComponent<TriarchDataBootstrap>();
        yield return null;

        var zoneService = new GameObject("ZoneService").AddComponent<TriarchZoneService>();
        var deathService = new GameObject("DeathService").AddComponent<TriarchDeathService>();
        var player = CreatePlayer("SafePlayer");

        zoneService.AssignZone(player, "sanctum");
        deathService.HandleDeath(player);
        yield return null;

        Assert.AreEqual(0, Object.FindObjectsOfType<TriarchLootContainer>().Length);

        CleanupScene();
    }

    [UnityTest]
    public IEnumerator HighRiskZone_KillSpawnsLootContainerWithItems()
    {
        var bootstrapObject = new GameObject("TriarchDataBootstrap");
        bootstrapObject.AddComponent<TriarchDataBootstrap>();
        yield return null;

        var zoneService = new GameObject("ZoneService").AddComponent<TriarchZoneService>();
        var deathService = new GameObject("DeathService").AddComponent<TriarchDeathService>();
        var player = CreatePlayer("RiskPlayer");

        zoneService.AssignZone(player, "shatterwilds");
        var container = deathService.HandleDeath(player);
        yield return null;

        Assert.IsNotNull(container);
        Assert.Greater(container.Items.Count, 0);

        CleanupScene();
    }

    private static TriarchPlayer CreatePlayer(string name)
    {
        var playerObject = new GameObject(name);
        var inventory = playerObject.AddComponent<TriarchInventory>();
        inventory.SetInventoryItems(new List<TriarchItemStack>
        {
            new TriarchItemStack { ItemId = "potion", Quantity = 2 }
        });
        inventory.SetEquippedItems(new List<TriarchItemStack>
        {
            new TriarchItemStack { ItemId = "sword", Quantity = 1 }
        });

        var player = playerObject.AddComponent<TriarchPlayer>();
        return player;
    }

    private static void CleanupScene()
    {
        foreach (var obj in Object.FindObjectsOfType<GameObject>())
        {
            Object.DestroyImmediate(obj);
        }
    }
}
