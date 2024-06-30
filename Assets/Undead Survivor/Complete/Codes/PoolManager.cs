using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Goldmetal.UndeadSurvivor
{
    public class PoolManager : MonoBehaviour
    {
        public GameObject[] prefabs; // Array of prefabs to be pooled

        List<GameObject>[] pools; // Array of lists to hold the pooled objects

        void Awake()
        {
            // Initialize the pools array with the same length as the prefabs array
            pools = new List<GameObject>[prefabs.Length];

            // Create a new list for each pool
            for (int index = 0; index < pools.Length; index++) {
                pools[index] = new List<GameObject>();
            }
        }

        public GameObject Get(int index)
        {
            GameObject select = null;

            // Look for an inactive object in the pool
            foreach (GameObject item in pools[index]) {
                if (!item.activeSelf) {
                    select = item; // Select the inactive object
                    select.SetActive(true); // Activate it
                    break; // Exit the loop
                }
            }

            // If no inactive object is found, instantiate a new one
            if (!select) {
                select = Instantiate(prefabs[index], transform); // Instantiate a new object
                pools[index].Add(select); // Add the new object to the pool
            }

            return select; // Return the selected object
        }
    }
}
