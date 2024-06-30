using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Goldmetal.UndeadSurvivor
{
    // The Gear class manages the behavior and properties of gear items used by the player.
    public class Gear : MonoBehaviour
    {
        // Public fields for gear attributes.
        public ItemData.ItemType type;
        public float rate;

        // Method to initialize the gear with item data.
        public void Init(ItemData data)
        {
            // Basic setup.
            name = "Gear " + data.itemId;
            transform.parent = GameManager.instance.player.transform;
            transform.localPosition = Vector3.zero;

            // Set gear properties.
            type = data.itemType;
            rate = data.damages[0];

            // Apply gear effects.
            ApplyGear();
        }

        // Method to level up the gear, increasing its rate.
        public void LevelUp(float rate)
        {
            this.rate = rate;

            // Re-apply gear effects after leveling up.
            ApplyGear();
        }

        // Method to apply the gear effects based on its type.
        void ApplyGear()
        {
            switch (type)
            {
                case ItemData.ItemType.Glove:
                    // Apply the rate-up effect for gloves.
                    RateUp();
                    break;
                case ItemData.ItemType.Shoe:
                    // Apply the speed-up effect for shoes.
                    SpeedUp();
                    break;
            }
        }

        // Method to apply the rate-up effect for gloves.
        void RateUp()
        {
            // Get all weapons attached to the player.
            Weapon[] weapons = transform.parent.GetComponentsInChildren<Weapon>();

            // Adjust weapon speed based on the gear rate.
            foreach (Weapon weapon in weapons)
            {
                switch (weapon.id)
                {
                    case 0:
                        // For melee weapons (ID 0), increase the rotation speed.
                        float speed = 150 * Character.WeaponSpeed;
                        weapon.speed = speed + (speed * rate);
                        break;
                    default:
                        // For ranged weapons, decrease the firing interval.
                        speed = 0.5f * Character.WeaponRate;
                        weapon.speed = speed * (1f - rate);
                        break;
                }
            }
        }

        // Method to apply the speed-up effect for shoes.
        void SpeedUp()
        {
            // Adjust player movement speed based on the gear rate.
            float speed = 3 * Character.Speed;
            GameManager.instance.player.speed = speed + speed * rate;
        }
    }
}
