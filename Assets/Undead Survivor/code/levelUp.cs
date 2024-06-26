using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUp : MonoBehaviour
{
    RectTransform rect;
    item[] items;
    // Start is called before the first frame update
    void Awake()
    {
        rect = GetComponent<RectTransform>();
        items =  GetComponentsInChildren<item>(true);
    }

    public void Show()
    {
        Next();
        rect.localScale = Vector3.one;
        GameManager.instance.Stop();
        AudioManager.instance.PlaySfx(AudioManager.Sfx.LevelUp);
        AudioManager.instance.Effectbgm(true);
    }

    public void Hide()
    {
        rect.localScale = Vector3.zero;
        GameManager.instance.Resume();
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Select);
        AudioManager.instance.Effectbgm(false);
    }
   
    public void Select(int index)
    {
        items[index].OnClick();
    }

    void Next()
    {
        //deactivate item
        foreach (item item in items) {
            item.gameObject.SetActive(false);
        }

        //randomize item level
        int[] ran = new int[3];
        while (true) {
            ran[0] = Random.Range(0, items.Length);
            ran[1] = Random.Range(0, items.Length);
            ran[2] = Random.Range(0, items.Length);


            if (ran[0] != ran[1] && ran[1] != ran[2] && ran[0] != ran[2])
                break;
        }

        for (int index=0; index < ran.Length; index++) {
            item ranItem = items[ran[index]];

        //replace max lvl item with consumable like heal
            if (ranItem.level == ranItem.data.damages.Length) {
                items[4].gameObject.SetActive(true);
            }
            else {
                ranItem.gameObject.SetActive(true);
            }
        }

      
    }
}
