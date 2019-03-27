using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class GameManager : MonoBehaviour {
    public bool manual = false;
	public bool startedGame = false;
	public bool currentlyTesting = false;
    public int points = 0;

    public int[] neuralNetTopology = new int[]{2,20,16,8,1};

    public GameObject pressText;
    public TextMesh scoreText;
    public TextMesh scoreGeneration;
    public BirdMovement birdMovementScript;
    public GameObject[] movableObjectParents;
    
    public GameObject birdPrefab;
    public GameObject birdStartingPoint;

    public int generations = 50;
    public int genomesPerGeneration = 5;

    int currentGeneration = 0;

    NeuralNetwork[][] genomesNeuralNetw;
    NeuralNetwork[] newOnes;
    
    public GameObject[] currentGenomes;
    public double[] currentFitness;

    private void Start(){
        genomesNeuralNetw = new NeuralNetwork[generations][];

        for(int i = 0; i < generations; i++){
            genomesNeuralNetw[i] = new NeuralNetwork[genomesPerGeneration];
        }

        for(int i = 0; i < genomesPerGeneration; i++){
            NeuralNetwork neuralNet = new NeuralNetwork(neuralNetTopology);
            genomesNeuralNetw[currentGeneration][i] = neuralNet;
        }

        newOnes = new NeuralNetwork[genomesPerGeneration];
    }

    private void Update(){
        if(manual){
            if (!startedGame && Input.GetMouseButtonUp(0)){
                pressText.SetActive(false);
                startedGame = true;
                currentlyTesting = true;
                
                GameObject createdObject = GameObject.Instantiate(birdPrefab, birdStartingPoint.transform.position, Quaternion.identity) as GameObject;
                createdObject.transform.parent = birdStartingPoint.transform;
            }
        }else{
            if (!startedGame && Input.GetMouseButtonUp(0)){
                //pressText.SetActive(false);
                startedGame = true;
                createBirds();
                currentlyTesting = true;
            }
            /*if (startedGame){
                scoreText.text = points+"";
            }*/

            if (startedGame){
                if(currentlyTesting){
                    int countEnded = 0;
                    for(int i = 0; i < genomesPerGeneration; i++){
                        BirdMovement bm = currentGenomes[i].GetComponent("BirdMovement") as BirdMovement;
                        if(!bm.isAlive()){
                            currentFitness[i] = bm.getFitness();
                            countEnded++;
                        }
                    }
                    if(countEnded >= genomesPerGeneration)
                        currentlyTesting = false;
                }else{
                    //if(Input.GetMouseButtonUp(0)){
                        prepareNextGeneration();
                    //}
                }
            }        
        }
    }


    public void createBirds(){
        currentGenomes = new GameObject[genomesPerGeneration];
        currentFitness = new double[genomesPerGeneration];
        scoreGeneration.text = "Generation #" + currentGeneration;
        for(int i = 0; i < genomesPerGeneration; i++){
            GameObject createdObject = GameObject.Instantiate(birdPrefab, birdStartingPoint.transform.position, Quaternion.identity) as GameObject;
            createdObject.transform.parent = birdStartingPoint.transform;
            currentGenomes[i] = createdObject;

            BirdMovement bm = currentGenomes[i].GetComponent("BirdMovement") as BirdMovement;
            bm.setNeuralNet(genomesNeuralNetw[currentGeneration][i]);
        }
    }

    public void createBirds2(){
        currentGenomes = new GameObject[genomesPerGeneration];
        currentFitness = new double[genomesPerGeneration];
        for(int i = 0; i < genomesPerGeneration; i++){
            GameObject createdObject = GameObject.Instantiate(birdPrefab, birdStartingPoint.transform.position, Quaternion.identity) as GameObject;
            createdObject.transform.parent = birdStartingPoint.transform;
            currentGenomes[i] = createdObject;

            BirdMovement bm = currentGenomes[i].GetComponent("BirdMovement") as BirdMovement;
            bm.setNeuralNet(newOnes[i]);
        }
    }
    public void AddPoints(int points) {
        this.points += points;
    }

    public void prepareNextGeneration(){
        for(int i = 0; i < genomesPerGeneration; i++){
            Destroy(currentGenomes[i]);
        }
        GeneticAlgorithm genetic = new GeneticAlgorithm(genomesNeuralNetw[currentGeneration], currentFitness, neuralNetTopology);

        //currentGeneration++;
        genomesNeuralNetw[++currentGeneration] = genetic.newGeneration();
        //newOnes = genetic.newGeneration();
        ResetGame();
        createBirds();
        currentlyTesting = true;
    }

    public void ResetGame(){
        //scoreText.text = "";
        //startedGame = false;
        //stillAlive = true;
        //pressText.SetActive(true);
        foreach (GameObject gb in movableObjectParents) {
            foreach (Transform child in gb.transform) {
                Destroy(child.gameObject);
            }
            gb.GetComponent<ObjectsMoveManager>().SpawnCall();
        }
        //birdMovementScript.ResetBird();
    }
}
