using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Goldmetal.UndeadSurvivor
{
    // The Weapon class manages the behavior and properties of weapons used by the player.
    public class Weapon : MonoBehaviour
    {
        // Public fields for weapon attributes.
        public int id;
        public int prefabId;
        public float damage;
        public int count;
        public float speed;

        // Private fields for internal use.
        float timer;
        Player player;

        // Awake is called when the script instance is being loaded.
        void Awake()
        {
            // Initialize the player reference from the GameManager.
            player = GameManager.instance.player;
        }

        // Update is called once per frame.
        void Update()
        {
            // If the game is not live, exit the update.
            if (!GameManager.instance.isLive)
                return;

            // Handle weapon behavior based on its ID.
            switch (id)
            {
                case 0:
                    // For melee weapons (ID 0), rotate them.
                    transform.Rotate(Vector3.back * speed * Time.deltaTime);
                    break;
                default:
                    // For ranged weapons, manage the firing timer.
                    timer += Time.deltaTime;

                    if (timer > speed)
                    {
                        timer = 0f;
                        Fire();
                    }
                    break;
            }

            // Test code for leveling up the weapon when the Jump button is pressed.
            if (Input.GetButtonDown("Jump"))
            {
                LevelUp(10, 1);
            }
        }

        // Method to level up the weapon, increasing its damage and count.
        public void LevelUp(float damage, int count)
        {
            this.damage = damage * Character.Damage;
            this.count += count;

            // If the weapon is a melee weapon, re-batch its projectiles.
            if (id == 0)
                Batch();

            // Apply gear changes to the player.
            player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
        }

        // Method to initialize the weapon with item data.
        public void Init(ItemData data)
        {
            // Basic setup.
            name = "Weapon " + data.itemId;
            transform.parent = player.transform;
            transform.localPosition = Vector3.zero;

            // Set weapon properties.
            id = data.itemId;
            damage = data.baseDamage * Character.Damage;
            count = data.baseCount + Character.Count;

            // Find the appropriate prefab ID for the weapon's projectile.
            for (int index = 0; index < GameManager.instance.pool.prefabs.Length; index++)
            {
                if (data.projectile == GameManager.instance.pool.prefabs[index])
                {
                    prefabId = index;
                    break;
                }
            }

            // Set weapon speed based on its type.
            switch (id)
            {
                case 0:
                    speed = 150 * Character.WeaponSpeed;
                    Batch();
                    break;
                default:
                    speed = 0.5f * Character.WeaponRate;
                    break;
            }

            // Set the weapon's hand sprite and activate it.
            Hand hand = player.hands[(int)data.itemType];
            hand.spriter.sprite = data.hand;
            hand.gameObject.SetActive(true);

            // Apply gear changes to the player.
            player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
        }

        // Method to batch projectiles for melee weapons.
        void Batch()
        {
            for (int index = 0; index < count; index++)
            {
                Transform bullet;

                if (index < transform.childCount)
                {
                    bullet = transform.GetChild(index);
                }
                else
                {
                    bullet = GameManager.instance.pool.Get(prefabId).transform;
                    bullet.parent = transform;
                }

                bullet.localPosition = Vector3.zero;
                bullet.localRotation = Quaternion.identity;

                Vector3 rotVec = Vector3.forward * 360 * index / count;
                bullet.Rotate(rotVec);
                bullet.Translate(bullet.up * 1.5f, Space.World);
                bullet.GetComponent<Bullet>().Init(damage, -100, Vector3.zero); // -100 is Infinity Per.
            }
        }

        // Method to fire projectiles for ranged weapons.
        void Fire()
        {
            // If there is no target, return.
            if (!player.scanner.nearestTarget)
                return;

            // Calculate the direction to the target and fire a projectile.
            Vector3 targetPos = player.scanner.nearestTarget.position;
            Vector3 dir = targetPos - transform.position;
            dir = dir.normalized;

            Transform bullet = GameManager.instance.pool.Get(prefabId).transform;
            bullet.position = transform.position;
            bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir);
            bullet.GetComponent<Bullet>().Init(damage, count, dir);

            // Play the firing sound effect.
            AudioManager.instance.PlaySfx(AudioManager.Sfx.Range);
        }
    }
}
