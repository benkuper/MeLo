using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimController : MonoBehaviour
{
    Color color = Color.white * 2;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void setColor(Color c)
    {
        this.color = c;
        fade(0, 0);
        fade(1, 1);
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

    public void fade(float value, float time)
    {
        Renderer r = GetComponent<Renderer>();
        Renderer[] renderers = GetComponentsInChildren<Renderer>();

        if (r != null) r.material.DOKill();
        foreach (var tr in renderers) tr.DOKill();


        if (time == 0)
        {
            if (r != null) r.material.DOColor(color * value, "_Color", time);
            foreach (var tr in renderers) tr.material.DOColor(color * value, "_Color", time);
        }
        else
        {
            if (r != null) r.material.DOColor(color * value, "_Color", time);
            foreach (var tr in renderers) tr.material.DOColor(color * value, "_Color", time);
        }
    }
}
