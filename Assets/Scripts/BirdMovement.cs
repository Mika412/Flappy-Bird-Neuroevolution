using UnityEngine;
using System.Collections;

public class BirdMovement : MonoBehaviour {
    public bool stillAlive = true;

    Vector3 birdInitPosition;
	public bool didFlap = false;
	public Vector3 jumpForce;
	Rigidbody rigidBody;
	GameManager mainMan;
    
    private AudioSource audioSource;
    public AudioClip flapSound;
    public AudioClip hitSound;
    public AudioClip scoreSound;

    private bool hitGround = false;

    NeuralNetwork NeuralNetwork;

    public GameObject nextObstacle;

    public GameObject obstaclesParent;

    public double fitness = 0;


    public Material redMaterial;

    private int points = 0;
	void Start(){
        rigidBody = GetComponent<Rigidbody> ();
        audioSource = GetComponent<AudioSource>();
        birdInitPosition = this.transform.localPosition;
		mainMan = GameObject.Find ("GameManager").GetComponent<GameManager> ();
        obstaclesParent = GameObject.Find("Obstacles");
	}

	void FixedUpdate () {
        if(mainMan.manual){
             if (mainMan.startedGame){
                rigidBody.useGravity = true;

                    // Jump
                    if (Input.GetKeyUp("space") || Input.GetMouseButtonUp(0)) {
                        rigidBody.velocity = Vector3.zero;
                        rigidBody.AddForce(jumpForce);
                        //audioSource.PlayOneShot(flapSound);
                    }

                    if (rigidBody.velocity.y > 0) {
                        transform.localRotation = Quaternion.Euler(transform.localRotation.x, transform.localRotation.y, 0);
                    } else {
                        float angle = Mathf.Lerp(0, -90.0f, -rigidBody.velocity.y / 2);
                        transform.localRotation = Quaternion.Euler(transform.localRotation.x, transform.localRotation.y, angle);
                    }

                    nextObstacle = getNextObstacle();  
                    Debug.Log("Distance is " + (nextObstacle.transform.position.x - this.transform.position.x));
                    Renderer[] renderers = nextObstacle.GetComponentsInChildren<Renderer>();
 
                    for(int i = 0; i < renderers.Length; i++) {
                        //The line below "resets" the materials
                        renderers[i].material = redMaterial;
                    
                        //If I want to change to an entirely different material, I do this
                        //renderers[i].materials = anArrayOfMaterials;
                    }
                    //nextObstacle.
            }else {
                rigidBody.useGravity = false;
            }
        }else{
            if (mainMan.startedGame){
                rigidBody.useGravity = true;

                if (stillAlive) {
                    // Jump
                    /*if (Input.GetKeyUp("space") || Input.GetMouseButtonUp(0)) {
                        rigidBody.velocity = Vector3.zero;
                        rigidBody.AddForce(jumpForce);
                        audioSource.PlayOneShot(flapSound);
                    }*/
                    
                    nextObstacle = getNextObstacle();  
                    Transform nextTrigger = nextObstacle.transform.Find("ScoreTrigger").transform;
                    Transform middlePosition = nextObstacle.transform.Find("Medium").transform;

                     Renderer[] renderers = middlePosition.GetComponentsInChildren<Renderer>();
 
                    for(int i = 0; i < renderers.Length; i++) {
                        renderers[i].material = redMaterial;
                    }
                    CalculatePipeHeight calculatePipeHeight = (CalculatePipeHeight)nextObstacle.GetComponent("CalculatePipeHeight");
                    double feature1 = (middlePosition.position.x - this.transform.position.x) / 16.2d; //Distance till the trigger
                    double feature2 = (middlePosition.position.y - 1) / (15-1);
                    double feature3 = (this.transform.position.y -1.3) / (14.7 -1.3 );
                    double feature4 = feature3 -feature2;
                    double[] features = new double[]{feature1, feature4};
                    if (NeuralNetwork.predict(features)[0] > 0.0d) {
                        Debug.Log("Flapped");
                        rigidBody.velocity = Vector3.zero;
                        rigidBody.AddForce(jumpForce);
                        audioSource.PlayOneShot(flapSound);
                    }
                    if (rigidBody.velocity.y > 0) {
                        transform.localRotation = Quaternion.Euler(transform.localRotation.x, transform.localRotation.y, 0);
                    } else {
                        float angle = Mathf.Lerp(0, -90.0f, -rigidBody.velocity.y / 2);
                        transform.localRotation = Quaternion.Euler(transform.localRotation.x, transform.localRotation.y, angle);
                    }
                    updateTimer();
                }
            }else {
                rigidBody.useGravity = false;
            }
        }
        //Debug.Log("Obstacle x " + nextObstacle.transform.position.x);
	}

    public GameObject getNextObstacle(){
        foreach (Transform child in obstaclesParent.transform){
            if(child.Find("Medium").position.x > transform.position.x){
                return child.gameObject;
            }
        }
        return null;
    }

    public void setNeuralNet(NeuralNetwork NeuralNetwork){
        this.NeuralNetwork = NeuralNetwork;
    }

    public double getFitness(){
        return fitness;
    }

    public bool isAlive(){
        return stillAlive;
    }

    public void ResetBird() {
        print(birdInitPosition);
        this.gameObject.transform.localPosition = birdInitPosition;
        transform.rotation = Quaternion.identity;
        rigidBody.useGravity = false;
        rigidBody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionX;
        rigidBody.velocity = Vector3.zero;
    }

	void OnCollisionEnter(Collision collision) {
		if (collision.gameObject.tag == "wall" || collision.gameObject.tag == "floor") {
            //audioSource.PlayOneShot(hitSound);
            stillAlive = false;
            //Debug.Log("Time " + fitness);
        }
        rigidBody.constraints = RigidbodyConstraints.None;
	}

    private void OnTriggerEnter(Collider other){
        if (other.gameObject.tag == "CounterTrigger"){
            //print("Collided with points");
            
            Debug.Log("Points: " + points++);
            //audioSource.PlayOneShot(scoreSound);
            //Destroy(other);
        }
    }

    public void updateTimer(){
         fitness += Time.deltaTime;
     }
}
