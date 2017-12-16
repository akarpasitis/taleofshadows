using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetection : MonoBehaviour {

    public GameObject flipModel;

    public float detectionTime;
    float startRun;
    bool firstDetection;

    // movement options

    public float runSpeed;
    public float walkSpeed;
    public bool facingRight = true;

    float moveSpeed;
    bool running;

    Rigidbody myRB;
    Animator myAnim;
    Transform detectedPlayer;

    bool detected;


    // Use this for initialization
    void Start(){
        myRB = GetComponentInParent<Rigidbody>();

        running = false;
        detected = false;
        firstDetection = false;
        moveSpeed = walkSpeed;

        if (Random.Range(0, 10) > 5) Flip();
    }

    // Update is called once per frame
    void FixedUpdate(){
        if (detected){
            if (detectedPlayer.position.x < transform.position.x && facingRight)
                Flip();
            else if (detectedPlayer.position.x > transform.position.x && !facingRight)
                Flip();

            if (!firstDetection){
                startRun = Time.time + detectionTime;
                firstDetection = true;
            }
        }

        if (detected && !facingRight) myRB.velocity = new Vector3((moveSpeed * -1), myRB.velocity.y, 0);
        else if (detected && facingRight) myRB.velocity = new Vector3(moveSpeed, myRB.velocity.y, 0);
        if (!running && detected){
            if (startRun < Time.time){
                moveSpeed = runSpeed;
                running = true;
            }
        }
    }

    void OnTriggerEnter(Collider other){
        if (other.tag == "Player" && !detected){
            detected = true;
            detectedPlayer = other.transform;
            if (detectedPlayer.position.x < transform.position.x && facingRight)
                Flip();
            else if (detectedPlayer.position.x > transform.position.x && !facingRight)
                Flip();
        }
    }
    void OnTriggerExit(Collider other){
        if (other.tag == "Player"){
            firstDetection = false;
            if (running){
                myAnim.SetTrigger("run");
                moveSpeed = walkSpeed;
                running = false;
            }
        }
      }

    void Flip(){
        facingRight = !facingRight;
        Vector3 theScale = flipModel.transform.localScale;
        theScale.z *= -1;
        flipModel.transform.localScale = theScale;
    }
}