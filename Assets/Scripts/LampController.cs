using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LampController : MonoBehaviour
{
    public List<Light> reds = new List<Light>();
    public List<Light> greens = new List<Light>();
    public bool startGreen = true;

    void Start()
    {
	green = startGreen;
    }
    
    public bool green
    {
	get
	{
	    return _green;
	}
	set
	{
	    greens.ForEach((r) => r.gameObject.SetActive(value));
	    reds.ForEach((r) => r.gameObject.SetActive(!value));
	    _green = value;
	}
    }
    private bool _green = true;
    
}
