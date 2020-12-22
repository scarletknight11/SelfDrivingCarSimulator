using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabSource : MonoBehaviour
{
    
    public GameObject parent;
    public GameObject prefab;
    public float seconds = 1;
    public int sourceNumber = 0;

    private float time = 0;
    private int serial = 0;

    void Update()
    {
	if (time <= 0)
	{
	    var p = Instantiate(prefab, transform.position, transform.rotation);
	    p.name = "(" + sourceNumber + ")" + prefab.name + " " + serial;
	    serial++;
	    p.transform.SetParent(parent.transform);
	    time = seconds;
	}
	else
	{
	    time -= Time.deltaTime;
	}
    }
}
