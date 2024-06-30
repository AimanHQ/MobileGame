using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Goldmetal.UndeadSurvivor
{
    // Create a new scriptable object menu entry for creating ItemData assets
    [CreateAssetMenu(fileName = "Item", menuName = "Scriptable Object/ItemData")]
    public class ItemData : ScriptableObject
    {
        // Enumeration for different item types
        public enum ItemType { Melee, Range, Glove, Shoe, Heal }

        // Main information about the item
        [Header("# Main Info")]
        public ItemType itemType; // Type of the item
        public int itemId; // Unique identifier for the item
        public string itemName; // Name of the item
        [TextArea]
        public string itemDesc; // Description of the item
        public Sprite itemIcon; // Icon representing the item

        // Level-specific data for the item
        [Header("# Level Data")]
        public float baseDamage; // Base damage of the item
        public int baseCount; // Base count (e.g., number of uses or projectiles)
        public float[] damages; // Array of damage values for different levels
        public int[] counts; // Array of count values for different levels

        // Weapon-specific data
        [Header("# Weapon")]
        public GameObject projectile; // Prefab for the projectile, if the item is a ranged weapon
        public Sprite hand; // Sprite for the hand holding the item, if applicable
    }
}
