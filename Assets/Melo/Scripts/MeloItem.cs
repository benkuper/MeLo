using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class MeloItem : MonoBehaviour
{
    public delegate void ItemDestroyedEvent(MeloItem meloItem);
    public event ItemDestroyedEvent itemDestroyed;

    public string id;
    public string itemName;
    public string type;
    public int value;
    public Color color;


    public float life = 3f;

    float timeAtSpawn;
    public bool isDestroying = false;

    AnimController animObject;
    Material panelMat;

    TextMeshPro labelTM;
    TextMeshPro valueTM;

    void Awake()
    {
        timeAtSpawn = Time.time;
        labelTM = transform.Find("Label").GetComponent<TextMeshPro>();
        valueTM = transform.Find("ValueText").GetComponent<TextMeshPro>();
        panelMat = transform.Find("Panel").GetComponent<Renderer>().material;

    }

    // Update is called once per frame
    void Update()
    {
        if (!isDestroying && Time.time > timeAtSpawn + life)
        {
            Debug.Log("call destroy");
            itemDestroyed?.Invoke(this);
            isDestroying = true;
        }
    }

    public void setup(string id, string name, string type, Color color, GameObject prefab, float animHDRMultiplier, float life, int value)
    {
        this.id = id;
        this.itemName = name;
        this.type = type;
        this.color = color;
        this.life = life;

        if(prefab != null)
        {
            this.animObject = Instantiate(prefab).GetComponent<AnimController>();
            this.animObject.transform.SetParent(transform.Find("Anim"), false);
            this.animObject.setColor(color * animHDRMultiplier);
            this.animObject.setValue(value);
        }
        
        panelMat.SetColor("_Color",color);

        labelTM.text = itemName;
        valueTM.gameObject.SetActive(value >= 0);

        fade(0, 0);
        fade(1, 1);

        updateValue(value);
    }

    public void fade(float value, float time)
    {
        panelMat.DOKill();
        labelTM.DOKill();
        valueTM.DOKill();

        if (time == 0)
        {
            panelMat.SetFloat("_Opacity", value);
            labelTM.color = new Color(labelTM.color.r, labelTM.color.b, labelTM.color.g, value);
            valueTM.color = new Color(valueTM.color.r, valueTM.color.b, valueTM.color.g, value);
        }
        else
        {
            panelMat.DOFloat(value, "_Opacity", time);
            labelTM.DOFade(value, time);
            valueTM.DOFade(value, time);
        }

        if(this.animObject != null)
        {
            this.animObject.fade(value, time);
        }

    }


    public void updateValue(int value)
    {
        timeAtSpawn = Time.time;
        this.value = value;

        if (value >= 0) valueTM.text = (value * 100 / 127).ToString() + "%";
        else valueTM.text = "";

        if(this.animObject != null) this.animObject.setValue(value / 127.0f);
    }
}
