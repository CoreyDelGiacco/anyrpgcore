using System.Collections.Generic;
using UnityEngine;

namespace Triarch
{
    [System.Serializable]
    public struct TriarchItemStack
    {
        public string ItemId;
        public int Quantity;
    }

    public sealed class TriarchInventory : MonoBehaviour
    {
        [SerializeField]
        private List<TriarchItemStack> inventoryItems = new List<TriarchItemStack>();

        [SerializeField]
        private List<TriarchItemStack> equippedItems = new List<TriarchItemStack>();

        public IReadOnlyList<TriarchItemStack> InventoryItems => inventoryItems;
        public IReadOnlyList<TriarchItemStack> EquippedItems => equippedItems;

        public void SetInventoryItems(List<TriarchItemStack> items)
        {
            inventoryItems = items ?? new List<TriarchItemStack>();
        }

        public void SetEquippedItems(List<TriarchItemStack> items)
        {
            equippedItems = items ?? new List<TriarchItemStack>();
        }

        public List<TriarchItemStack> CollectDrops(LootDropMode dropMode, bool protectEquipped)
        {
            var drops = new List<TriarchItemStack>();

            if (dropMode == LootDropMode.NONE)
            {
                return drops;
            }

            if (inventoryItems.Count > 0)
            {
                drops.AddRange(inventoryItems);
                inventoryItems.Clear();
            }

            var dropEquipped = dropMode == LootDropMode.FULL && !protectEquipped;
            if (dropEquipped && equippedItems.Count > 0)
            {
                drops.AddRange(equippedItems);
                equippedItems.Clear();
            }

            return drops;
        }
    }
}
