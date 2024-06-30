using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Goldmetal.UndeadSurvivor
{
    // The Item class represents an item in the game, including weapons, gear, and healing items.
    public class Item : MonoBehaviour
    {
        // Public fields for item data, level, weapon, and gear.
        public ItemData data;
        public int level;
        public Weapon weapon;
        public Gear gear;

        // Private fields for UI elements.
        Image icon;
        Text textLevel;
        Text textName;
        Text textDesc;

        // Awake is called when the script instance is being loaded.
        void Awake()
        {
            // Initialize the icon with the item's icon image.
            icon = GetComponentsInChildren<Image>()[1];
            icon.sprite = data.itemIcon;

            // Get all Text components in the children of this GameObject.
            Text[] texts = GetComponentsInChildren<Text>();
            textLevel = texts[0];
            textName = texts[1];
            textDesc = texts[2];
            // Set the item's name in the UI.
            textName.text = data.itemName;
        }

        // OnEnable is called when the object becomes enabled and active.
        void OnEnable()
        {
            // Update the level text.
            textLevel.text = "Lv." + (level + 1);

            // Update the description text based on the item type.
            switch (data.itemType)
            {
                case ItemData.ItemType.Melee:
                case ItemData.ItemType.Range:
                    textDesc.text = string.Format(data.itemDesc, data.damages[level] * 100, data.counts[level]);
                    break;
                case ItemData.ItemType.Glove:
                case ItemData.ItemType.Shoe:
                    textDesc.text = string.Format(data.itemDesc, data.damages[level] * 100);
                    break;
                default:
                    textDesc.text = string.Format(data.itemDesc);
                    break;
            }
        }

        // OnClick is called when the item is clicked.
        public void OnClick()
        {
            // Handle item behavior based on the item type.
            switch (data.itemType)
            {
                case ItemData.ItemType.Melee:
                case ItemData.ItemType.Range:
                    // If the item is a weapon, initialize or upgrade it.
                    if (level == 0)
                    {
                        GameObject newWeapon = new GameObject();
                        weapon = newWeapon.AddComponent<Weapon>();
                        weapon.Init(data);
                    }
                    else
                    {
                        float nextDamage = data.baseDamage;
                        int nextCount = 0;

                        nextDamage += data.baseDamage * data.damages[level];
                        nextCount += data.counts[level];

                        weapon.LevelUp(nextDamage, nextCount);
                    }

                    level++;
                    break;
                case ItemData.ItemType.Glove:
                case ItemData.ItemType.Shoe:
                    // If the item is gear, initialize or upgrade it.
                    if (level == 0)
                    {
                        GameObject newGear = new GameObject();
                        gear = newGear.AddComponent<Gear>();
                        gear.Init(data);
                    }
                    else
                    {
                        float nextRate = data.damages[level];
                        gear.LevelUp(nextRate);
                    }

                    level++;
                    break;
                case ItemData.ItemType.Heal:
                    // If the item is a healing item, restore the player's health.
                    GameManager.instance.health = GameManager.instance.maxHealth;
                    break;
            }

            // If the item has reached its maximum level, disable the button.
            if (level == data.damages.Length)
            {
                GetComponent<Button>().interactable = false;
            }
        }
    }
}
