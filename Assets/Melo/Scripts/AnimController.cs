using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimController : MonoBehaviour
{
    Color color = Color.white*2;

    // Start is called before the first frame update
    void Start()
    {
        updateColor();

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void setColor(Color c)
    {
        this.color = c;
        updateColor();
    }

    public void updateColor()
    {
        Renderer r = GetComponent<Renderer>();
        if (r != null) r.material.SetColor("_Color", color);

        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        foreach (var tr in renderers) tr.material.SetColor("_Color", color);
    }

    public void setValue(float val)
    {
        Renderer r = GetComponent<Renderer>();
        if (r != null) r.material.SetFloat("_Value", val);

        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        foreach (var tr in renderers) tr.material.SetFloat("_Value", val);
    }
}
