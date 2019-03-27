using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneticAlgorithm {


    private double[] fits;    // [generation_index][genome_index]


    public NeuralNetwork[] newNeuralNets; // [generation_index][genome_index]
    public NeuralNetwork[] oldNeuralNetworks; // [generation_index][genome_index]

    private int genomes_per_generation = 50;     //Genomes per generation
    private double mutation_probability = 0.01d;    //Mutation probability

    private double min_weight = -1d;
    private double max_weight = 1d;  //Max and min weights

    private int parentsNum = 6;             //Max parents to save
    private int randomParents = 0;          //Max random parents to save

    int[] neuralNetworkProp;  //Network topology

    bool doChallenge = true;    //Should evolve with challenge

    public GeneticAlgorithm(NeuralNetwork[] oldNeural, double[] fitness, int[] networkTopology){
        this.fits = fitness;
        oldNeuralNetworks = oldNeural;
        newNeuralNets = new NeuralNetwork[oldNeuralNetworks.Length];
        neuralNetworkProp = networkTopology;
    }
    /**
     * Generates a new generation
     */
    public NeuralNetwork[] newGeneration() {
        sortByFitness();
        crossover();
        return newNeuralNets;
    }

	/**
     * Sort a generation by fitness
     */
    private void sortByFitness(){

        // Sort
        int j_max;
        double fit_temp;
        NeuralNetwork neuralNet_temp;
        for(int i = 0; i < genomes_per_generation - 1; i++) {
            j_max = i;
            for(int j = i + 1; j < genomes_per_generation; j++) {
                if(fits[j] >= fits[j_max]) {
                    j_max = j;
                }
            }
            if(j_max != i) {
                fit_temp = fits[i];
                neuralNet_temp = oldNeuralNetworks[i].clone();
                fits[i] = fits[j_max];
                oldNeuralNetworks[i] = oldNeuralNetworks[j_max].clone();
                fits[j_max] = fit_temp;
                oldNeuralNetworks[j_max] = (NeuralNetwork) neuralNet_temp.clone();
            }
        }
    }

	
    /**
     * Does crossover and mutates the generation
     */
    public void crossover() {
        //Debug.Log("Should be sorted " +  fits[0]);

        for (int l = 0; l < parentsNum; l++) {
            newNeuralNets[l] = oldNeuralNetworks[l].clone();
        }

        for (int i = parentsNum; i < parentsNum + randomParents; i++) {
            newNeuralNets[i] = oldNeuralNetworks[Utils.randInt(parentsNum, genomes_per_generation - 1)].clone();
        }
        //newNeuralNets[0].printWeights();
        for (int l = parentsNum + randomParents; l < genomes_per_generation; l++) {

            double[][][] hiddenWeight = new double[neuralNetworkProp.Length - 1][][];
            for (int i = 0; i < hiddenWeight.Length; i++) {
                hiddenWeight[i] = new double[neuralNetworkProp[i] + 1][];
           	 	for (int j = 0; j < hiddenWeight[i].Length; j++) {
					hiddenWeight[i][j] = new double[neuralNetworkProp[i + 1]];
				}
            }
//2 4 1


            int mixChoice = Utils.randInt(0, parentsNum + randomParents - 1);
            for(int i=0;i<hiddenWeight.Length;i++){
                Debug.Log("New is " + hiddenWeight[i].Length + " Old " + newNeuralNets[0].hiddenWeight[i].Length);
                for(int k=0;k<hiddenWeight[i].Length;k++){
                    for(int j=0;j<hiddenWeight[i][k].Length;j++) {
                        if (Utils.randDouble(0, 1) < mutation_probability) {
                            Debug.Log("Entered here");
//                            hiddenWeight[i][k][j] = randDouble(min_weight, max_weight);
//                            if(randDouble(0,1) < random_mutation_probability)
                            hiddenWeight[i][k][j] = Utils.randDouble(min_weight, max_weight);
                            //hiddenWeight[i][k][j] = 0.5f;
//                            else*/
//                                hiddenWeight[i][k][j] *= randDouble(minChange, maxChange);
                        } else {
                            if (Utils.randDouble(0, 1) < 0.8f){
                                hiddenWeight[i][k][j] = newNeuralNets[0].hiddenWeight[i][k][j];
                                //hiddenWeight[i][k][j] = -0.4d;
                            }else {
                                //hiddenWeight[i][k][j] = 0.4d;
                                hiddenWeight[i][k][j] = newNeuralNets[mixChoice].hiddenWeight[i][k][j];
                            }
                        }
                    }
                }
            }
            newNeuralNets[l] = new NeuralNetwork(hiddenWeight);
        }

        /*for(int i = 0; i < genomes_per_generation; i++){
            NeuralNetwork neuralNet = new NeuralNetwork(neuralNetworkProp);
            newNeuralNets[i] = neuralNet;
        }*/
    }

}
