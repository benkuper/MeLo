using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using OSCQuery.UnityOSC;
using static UnityEditor.Progress;

[System.Serializable]
public class MeloType
{
    public string type;
    public Color color;
    public GameObject prefab;
}

public class MeloManager : MonoBehaviour
{
    public GameObject itemPrefab;
    public List<MeloItem> items = new List<MeloItem>();
    public List<MeloType> types = new List<MeloType>();

    public float itemLife = 2f;

    [Range(0f, 2f)]
    public float animTime;
    public float gap;

    [Range(0f, 1f)]
    public float opacityMultiplier = 1;

    [Range(0f,10f)]
    public float animHDRMultiplier = 5;


    public Vector3 initRot = Vector3.zero;

    string[] names = new string[4] { "Genial", "Amazing", "Cool", "Super" };


    static MeloManager instance;

    int cleaningAnimCount = 0;


    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        OSCQuery.OSCQuery q = FindObjectOfType<OSCQuery.OSCQuery>();
        q.onMessageReceived += messageReceived;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            int it = Random.Range(0, types.Count);
            int ni = Random.Range(0, names.Length);
            addLog("id " + Random.Range(0, 20), names[ni], types[it].type, Random.Range(0, 100));
        }
    }

    public void addLog(string id, string text, string type, int value)
    {
        MeloItem item = getItemWithId(id);

        if (item == null)
        {
            item = Instantiate(itemPrefab).GetComponent<MeloItem>();
            MeloType t = getTypeForString(type);
            Debug.Log(item);

            Color col = t.color;
            col.a *= opacityMultiplier;

            item.setup(id, text, type, col, t.prefab, animHDRMultiplier, itemLife, value);
            item.itemDestroyed += itemDestroyed;

            item.transform.SetParent(transform);
            items.Add(item);

            item.transform.rotation = Quaternion.Euler(initRot);


            item.transform.localPosition = Vector3.down * gap * (items.IndexOf(item) + 1);
        }
        else
        {
            item.updateValue(value);
        }

        placeItems();
    }

    public void placeItems()
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].isDestroying) continue;
            items[i].transform.DOKill();
            items[i].transform.DOLocalMove(Vector3.down * gap * i, animTime);
            items[i].transform.DORotate(Vector3.zero, animTime);

        }
    }


    //Events
    void itemDestroyed(MeloItem item)
    {
        cleaningAnimCount++;
        

        item.fade(0, .5f);
        item.transform.DOKill();
        item.transform.DORotate(initRot, .5f);
        item.transform.DOMove(item.transform.position + Vector3.right * 10, .5f).OnComplete(() =>
        {
            Destroy(item.gameObject);
            items.Remove(item);
            cleaningAnimCount--;
            if(cleaningAnimCount == 0) placeItems(); 
        });
    }


    //Helpers
    MeloItem getItemWithId(string id)
    {
        foreach (MeloItem item in items)
        {
            if (item.id == id) return item;
        }

        return null;
    }

    MeloType getTypeForString(string type)
    {
        foreach (MeloType t in types)
        {
            if (t.type == type) return t;
        }
        return null;
    }


    //OSC
    void messageReceived(OSCMessage msg)
    {
        if (msg.Address == "/log")
        {
            if (msg.Data.Count >= 4)
            {
                addLog(msg.Data[0].ToString(), msg.Data[1].ToString(), msg.Data[2].ToString(), (int)msg.Data[3]);
            }
        }
    }
}
