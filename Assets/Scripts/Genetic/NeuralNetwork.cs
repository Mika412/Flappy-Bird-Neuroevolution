using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeuralNetwork {

    /**
     * Network topology
     */
    private int[] inputNodes;   //Network Topology
    public double[][][] hiddenWeight;   //Network weights
	public double[][] hiddenNeuron;     //Neurons
    private double minValue = -1d;    //Min values for weights
    private double maxValue = 1d;     //Max values for weights

	/**
     * Network constructor, creates the network from the topology
     * @param inputNodes network topology
     */
    public NeuralNetwork(int[] inputNodes) {
        this.inputNodes = inputNodes;
        initNeuron();
        initWeight();
    }
/**
     * Network constructor, created from the weights
     * @param hiddenWeight  neural networks weights
     */
    public NeuralNetwork(double[][][] hiddenWeight) {
        inputNodes = new int[hiddenWeight.Length + 1];
        for (int i = 0; i < hiddenWeight.Length; i++) {
            inputNodes[i] = hiddenWeight[i].Length - 1;
        }
        inputNodes[inputNodes.Length - 1] = hiddenWeight[hiddenWeight.Length - 1][0].Length;

        this.hiddenWeight = (double[][][])hiddenWeight.Clone();
        initNeuron();
    }

    /**
     * Initializes the neurons of the network
     */
    private void initNeuron() {
        hiddenNeuron = new double[inputNodes.Length][];
        for (int i = 0; i < inputNodes.Length; i++) {
            hiddenNeuron[i] = new double[inputNodes[i]];
        }
    }

    /**
     * Initializes the weight values of the network
     */
    private void initWeight() {
        hiddenWeight = new double[inputNodes.Length-1][][];
        for (int i = 0; i < hiddenWeight.Length; i++) {
            hiddenWeight[i] = new double[inputNodes[i] + 1][];
            for (int j = 0; j < hiddenWeight[i].Length; j++) {
                hiddenWeight[i][j] = new double[inputNodes[i + 1]];
            }
        }

        for(int i = 0; i < hiddenWeight.Length;i++){
            for(int k = 0; k < hiddenWeight[i].Length; k++) {
                for (int j = 0; j < hiddenWeight[i][k].Length; j++) {
                    hiddenWeight[i][k][j] = Utils.randDouble(minValue, maxValue);
                }
            }
        }
    }
	
    /**
     * Predict function
     * @param input feature input array
     * @return  predicted value
     */
    public double[] predict(double[] input) {
        this.hiddenNeuron[0] = (double[])input.Clone();
        feedforward();
        return this.hiddenNeuron[hiddenNeuron.Length - 1];
    }

    /**
     * Calculates the values for the layer
     * @param weight    weight values between the layers
     * @param prev  previous layer
     * @param next  current layer
     */
    private void forward(double[][] weight, double[] prev, double[] next) {
        for(int i = 0 ; i < next.Length; i++) {
            double sum = 0;
            for(int j = 0 ; j< prev.Length; j++) {
                sum += prev[j] * weight[j][i];
            }
            sum += weight[weight.Length - 1][i];
            next[i]= Utils.tanh(sum);
        }
    }


    /**
     * Propagates the feedforward algorithm through the network.
     */
    private void feedforward() {
        for(int i = 1; i < hiddenNeuron.Length; i++) {
            forward(hiddenWeight[i - 1], hiddenNeuron[i-1], hiddenNeuron[i]);
        }
    }

    /**
     * Return a copy of this network
     * @return a copy of the network
     */
    public NeuralNetwork clone(){
        return new NeuralNetwork(hiddenWeight);
    }

    /**
     * Prints the weight values of the network
     */
    public void printWeights() {
        for (int i = 0; i < hiddenWeight.Length; i++) {
            for (int j = 0; j < hiddenWeight[i].Length; j++) {
                for (int k = 0; k < hiddenWeight[i][j].Length; k++) {
                    Debug.Log("Layer: " + i + " neuron " + j + " weight " + hiddenWeight[i][j][k]);
                }
            }
        }
    }
}
