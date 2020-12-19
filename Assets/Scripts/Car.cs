﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour {

    private DNA dna;
    private NeuralNetwork network;
    private Vector3 initialPoint;
    private float distance;

    private bool initialized = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Initialize()
    {
        network = new NeuralNetwork();
        dna = new DNA(network.getWeight());
        initialPoint = transform.position;
        initialized = true;
    }

    public void Initialize(DNA dna)
    {
        network = new NeuralNetwork(dna);
        this.dna = dna;
        initialPoint = transform.position;
        initialized = true;
    }

        // Update is called once per frame
    void Update()
    {
        if (initialized)
        {
            //Get Inputs of distance layers
            float[] inputs = GetComponent<Lasers>().getDistance();
            
            //Execute feed-forward
            network.feedForward(inputs);

            List<float> outputs = network.getOutputs();
            GetComponent<CarMov>().updateMovement(outputs);
            distance = Vector3.Distance(transform.position, initialPoint);
        }
    }

    void OnTriggerEnter(Collider col)
    {
        //change Camera():

    }
}
