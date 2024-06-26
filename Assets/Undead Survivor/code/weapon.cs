using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class weapon : MonoBehaviour
{
    public int id;
    public int prefabId;
    public float damage;
    public int count;
    public float speed;

    float timer;
    Player player;

    void Awake()
    {
        player = GameManager.instance.player;
    }


    // Update is called once per frame
    void Update()
    {
        if (!GameManager.instance.isLive)
            return;

         switch (id){
            case 0:
                transform.Rotate(Vector3.back * speed * Time.deltaTime);
                break;
            default:
                timer += Time.deltaTime;

                if (timer > speed){
                    timer = 0f;
                    Fire();
                }
                break;
        }

        //test code
        if (Input.GetButtonDown("Jump")){
            LevelUp(20, 1);
        }
    }
    public void LevelUp(float damage, int count)
    {
        this.damage = damage;
        this.count += count ;

        if (id == 0)
            Batch();

        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
        Debug.Log($"Weapon leveled up: Damage = {this.damage}, Count = {this.count}");

    }

    public void Init(ItemData data)
    {
        //basic set
        name = "weapon " + data.itemId;
        transform.parent = player.transform;
        transform.localPosition = Vector3.zero;
        //proprety set
        id = data.itemId;
        damage = data.baseDamage;
        count = data.baseCount;

        for (int index=0; index < GameManager.instance.pool.prefabs.Length; index++) {
            if (data.projectile == GameManager.instance.pool.prefabs[index]) {
                prefabId = index;
                break;
            }
        }


        switch (id){
            case 0:
                speed = 150;
                Batch();
                break;
            default:
                speed = 0.3f;
                break;
        }
        //hand set
        hand hand = player.hands[(int)data.itemType];
        hand.sprite.sprite = data.hand;
        hand.gameObject.SetActive(true);

        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
    }

    void Batch()
    {
        Debug.Log($"Batching bullets: Count = {count}");

        for (int index =0; index< count; index++){
            Transform bullet;
           
            if (index < transform.childCount){
                bullet = transform.GetChild(index);
            }
            else {
                bullet = GameManager.instance.pool.Get(prefabId).transform;
                bullet.parent = transform;
            }
           

            bullet.localPosition = Vector3.zero;
            bullet.localRotation = Quaternion.identity;

            Vector3 rotVec = Vector3.forward * 360 * index / count;
            bullet.Rotate(rotVec);
            bullet.Translate(bullet.up * 1.5f, Space.World);

            bullet.GetComponent<bullet>().Init(damage, -1, Vector3.zero); // -1 is infinity per.
            Debug.Log($"Bullet batched: Index = {index}, Damage = {damage}");

        }
    }

    void Fire()
    {
        if (!player.scanner.nearestTarget)
            return;

        Vector3 targetPos = player.scanner.nearestTarget.position;
        Vector3 dir = targetPos - transform.position;
        dir = dir.normalized;

        Transform bullet = GameManager.instance.pool.Get(prefabId).transform;
        bullet.position = transform.position;
        bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir);
        bullet.GetComponent<bullet>().Init(damage, count, dir);
        
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Range);

        Debug.Log($"Bullet fired: Damage = {damage}, Count = {count}, Direction = {dir}");


    }
}

