using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils {
	
    /**
     * Generates a random value between a range
     *
     * @param min   min range value
     * @param max   max range value
     * @return      return a double
     */
    public static double randDouble(double min, double max) {
        Random r = new Random();
        return Random.Range((float)min, (float)max);
    }

    /**
     * Generate a random integer between a range
     * @param min   min range
     * @param max   max range
     * @return
     */
    public static int randInt(int min, int max) {
        int randomNum = Random.Range(min, max);
        return randomNum;
    }

    /**
     * Executes a sigmoid activation function
     *
     * @param z desired value
     * @return  activation value
     */
    public static double sigmoid(double z){
        return 1.0/(1.0+Mathf.Exp((float)(-z)));
    }

    public static double tanh(double z){
        return System.Math.Tanh(z);
    }
    /**
     * Calculates the derivative of the sigmoid activation function
     *
     * @param z desired value
     * @return  derivative of the value
     */
    public static double sigmoidDerivative(double z){
        return z*(1.0-z);
    }

    /**
     * Executes a ReLu activation function
     *
     *  @param x desired value
     *  @return  activation value
     */
    public static double ReLu(double x){
        return Mathf.Max(0, (float)x);
    }

    /**
     * Executes a Leaky ReLu activation function
     *
     *  @param x desired value
     *  @return  activation value
     */
    public static double LeakyReLu(double x){
        return Mathf.Max(0.01f *(float) x, (float)x);
    }

    /**
     * Calculates the derivative of the ReLu activation function
     *
     *  @param x desired value
     *  @return  activation value
     */
    public static double ReLuDerivative(double x){
        return x > 0 ? 1 : 0;
    }
}
