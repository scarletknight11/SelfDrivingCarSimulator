using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelSpeedSensor : MonoBehaviour
{
    public string tag = null;
    public int maxSamples = 4;

    public float selfVel = 1;
    public float selfDist = 1;
    public float carLength = 2;

    private class Sample {
	public Sample(float x, float dt)
	{
	    this.x = x;
	    this.dt = dt;
	}
	public float x;
	public float dt;
    }
    
    private List<Sample> samples = new List<Sample>();
    public float speed
    {
	get
	{
	    if (samples.Count < 1) return 0;
	    float totaldx = 0;
	    float totaldt = 0;
	    for (int i = 0, j = 1; i < samples.Count; i++, j++)
	    {
		totaldx += samples[i].x - samples[j].x;
		totaldt += samples[j].dt;
	    }
	    if (totaldx == Mathf.Infinity || totaldt == 0) return 0;
	    return totaldx / totaldt / samples.Count;
	}
	private set {}
    }

    void Update()
    {
	RaycastHit hit;
	var found = Physics.Raycast(transform.position, transform.forward, out hit);
	if (!found) return;
	if (!hit.collider.gameObject.CompareTag(tag)) return;
	// add distance to buffer
	AddSample(hit.distance, Time.deltaTime);
    }

    void AddSample(float s, float dt)
    {
	samples.Add(new Sample(s, dt));
	if (samples.Count > maxSamples)
	{
	    samples.RemoveAt(0);
	}
    }

    public bool PredictCollision()
    {
	// C= distance from car to collision point
	// P= distance from person to collision point
	// vc= car velocity
	// vp= person velocity
	// D = distance to player at interesection point
	// D = vc(-P/vp)-C
	// if D is positive the person passes infront of the car, if its negative the person is behind
	// therefore if D is negative it must be greater than the car length
	// we will all so require a margin of safety of 1 car length to account for the width of the car.
	// also if the cars velocity is negative, then we know its going away and can ignore it
	var vc = speed;
	var C = samples[samples.Count].x;
	var vp = selfVel;
	var P = selfDist;
	var D = vc * (-P / vp) - C;
	return vc < 0 || D > carLength || D < -2 * carLength;
	
    }
}
