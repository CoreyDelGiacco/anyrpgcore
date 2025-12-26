using System.Collections.Generic;
using UnityEngine;

namespace Triarch
{
    public sealed class TriarchLootContainer : MonoBehaviour
    {
        [SerializeField]
        private List<TriarchItemStack> items = new List<TriarchItemStack>();

        public IReadOnlyList<TriarchItemStack> Items => items;

        public void Initialize(List<TriarchItemStack> droppedItems)
        {
            items = droppedItems ?? new List<TriarchItemStack>();
        }
    }
}
