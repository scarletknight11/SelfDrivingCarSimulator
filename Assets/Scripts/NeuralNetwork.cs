using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeuralNetwork {

    public int hiddenLayers = 1;
    public int size_hidden_layers = 5;
    public int outputs = 2;
    public int inputs = 5;
    public float maxInitialValue = 1f;

    private const float EULER_NUMBER = 2.71838f;
    //containts list of neurons in organized layers
    private List<List<float>> neurons;
    //containts list of weights in organized layers
    private List<float[][]> weights;

    private int totalLayers = 0;

    public NeuralNetwork()
    {
        //hidden layers + inputlayers+outputlayers
        totalLayers = hiddenLayers + 2;
        //initailize weights & neurons array
        weights = new List<float[][]>();
        neurons = new List<List<float>>();

        //fill neurons & weight
        for(int i = 0; i < totalLayers; i++)
        {
            float[][] layerWeights;
            List<float> layer = new List<float>();
            int sizeLayer = getSizeLayer(i);
            //if value of i is less then hidden layers + 1
            if(i != 1 + hiddenLayers)
            {
                //change size of layerweights
                layerWeights = new float[sizeLayer][];
                //calculate for next size layer toto input into neurons
                int nextSizeLayer = getSizeLayer(i + 1);
                //calculate multiple layers and combinations of weights and neurons
                for(int j = 0; j <sizeLayer; j++)
                {
                    layerWeights[j] = new float[nextSizeLayer];
                    //make next layer
                    for (int k=0; k < nextSizeLayer; k++) 
                    {
                        //get next random value weight for layer
                        layerWeights[j][k] = getRandomValue();
                    }
                }
                //add weight for layers
                weights.Add(layerWeights);
            }
            for(int j = 0; j<sizeLayer; j++)
            {
                layer.Add(0);
            }
            neurons.Add(layer);
        }

    }
    //contains  weights of data and organized in certain layers
    // weight values will be static when the network is being updated but neuron values will change during gameplay
    public NeuralNetwork(DNA dna)
    {
        List<float[][]> weightsDNA = dna.getDNA();
        //assigning DNA to the weights
        //hidden layers + inputlayers+outputlayers
        totalLayers = hiddenLayers + 2;
        //initailize weights & neurons array
        weights = new List<float[][]>();
        neurons = new List<List<float>>();

        //fill neurons & weight
        for (int i = 0; i < totalLayers; i++)
        {
                 
            float[][] layerWeights;
            float[][] weightsDNALayer;
            List<float> layer = new List<float>();
            int sizeLayer = getSizeLayer(i);
            //if value of i is less then hidden layers + 1
            if (i != 1 + hiddenLayers)
            {
                weightsDNALayer = weightsDNA[i];
                //change size of layerweights
                layerWeights = new float[sizeLayer][];
                //calculate for next size layer toto input into neurons
                int nextSizeLayer = getSizeLayer(i + 1);
                //calculate multiple layers and combinations of weights and neurons
                for (int j = 0; j < sizeLayer; j++)
                {
                    layerWeights[j] = new float[nextSizeLayer];
                    //make next layer
                    for (int k = 0; k < nextSizeLayer; k++)
                    {
                        layerWeights[j][k] = weightsDNALayer[j][k];
                    }
                }
                //add weight for layers
                weights.Add(layerWeights);
            }
            for (int j = 0; j < sizeLayer; j++)
            {
                layer.Add(0);
            }
            neurons.Add(layer);
        }
    }

    public float[] feedForward(float[]inputs)
    {
        List<float> inputLayer = neurons[0];
        for (int i = 0; i < inputs.Length; i++)
        {
            inputLayer[i] = inputs[i];
        }
        //Update neurons from the Input layer to the output layer
        for (int layer = 0; layer < neurons.Count; layer++)
        {

            float[][] weightsLayer = weights[layer];
            int nextLayer = layer + 1;
            List<float> neuronsLayer = neurons[layer];
            List<float> neuronsNextLayer = neurons[nextLayer];
            //Next Layer
            for (int i = 0; i < neuronsNextLayer.Count; i++)
            {
                //add duplications of neurons
                float sum = 0;
                for (int j = 0; j < neuronsLayer.Count; j++)
                {
                    //Feed forward multiplication
                    sum += weightsLayer[j][i] * neuronsLayer[j];
                }
                neuronsNextLayer[i] = sigmold(sum);

            }
        }
        return null;
    }

    public int getSizeLayer(int i)
    {
        //Layer position i
        int sizeLayer = 0;
        //give different size depending on layer
        if (i == 0)
        {
            sizeLayer = inputs;
        } else if (i == hiddenLayers + 1)
        {
            sizeLayer = outputs;
        } else
        {
            sizeLayer = size_hidden_layers;
        }
        return sizeLayer;

    }
    //list of possible outputs within the network
    public List<float> getOutputs()
    {
        //return values of neurons
        return neurons[neurons.Count - 1];
    }

    public float sigmold(float x)
    {
        //return value of neuron weight
        return 1 / (float)(1 * Mathf.Pow(EULER_NUMBER, x));
    }

    //generate random value of neuron
    public float getRandomValue()
    {
        //return random range per neuron
        return Random.Range(-maxInitialValue, maxInitialValue);
    }

    //list of neurons within a list of generated neurons
    public List<List<float>> getNeurons()
    {
        return neurons;
    }
    //list of weight for our data points
    public List<float[][]> getWeight()
    {
        return weights;
    }
}
