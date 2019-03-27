using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface FitnessInterface {
    /**
     *  Score function interface
     * @param neuralNetwork
     * @return
     */
    double[] getScore(NeuralNetwork[] neuralNetwork);
}
