using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour {
    private Rigidbody rb;
    private bool gameFinished;
    private bool isPaused;
    private bool inWater;
    private Vector3 breathScale;
    private Animator animator;

    public Text text;
    public GameObject breath;
    public GameObject breathBar;
    
    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody>();
        gameFinished = false;
        isPaused = false;
        inWater = false;
        breathScale = new Vector3(1, 1);
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.R)) {
            SceneManager.LoadScene(Application.loadedLevel);
        } else if (Input.GetKeyDown(KeyCode.Q)) {
            Application.Quit();
        } else if (Input.GetKeyDown(KeyCode.P)) {
            isPaused = isPaused ? false : true;
            if (isPaused) {
                text.text = "Paused";
            } else {
                text.text = "";
            }
        }

        if (transform.position.y <= 6) {           
            if (inWater && !isPaused && !gameFinished) {
                breathScale = breathScale - new Vector3(0.004f, 0);
                breathBar.transform.localScale = breathScale;
                if (breathBar.transform.localScale.x <= 0) {
                    text.text = "You Lose!";
                    gameFinished = true;
                }
            } else {
                animator.SetBool("InWater", true);
                inWater = true;
                breath.SetActive(true);
            }
        } else if (inWater) {
            animator.SetBool("InWater", false);
            inWater = false;
            breath.SetActive(false);
            breathScale = new Vector3(1, 1);
            breathBar.transform.localScale = breathScale;
        }
    }

    private void FixedUpdate() {
        if (!isPaused) {
            Vector3 rotate = new Vector3(0, Input.GetAxis("Horizontal"), 0);
            float move = Input.GetAxis("Vertical");

            transform.Rotate(rotate * 2);

            if (move != 0) {
                if (move > 0) {
                    rb.velocity = transform.forward * 40;
                } else {
                    rb.velocity = transform.forward * -40;
                }
                animator.SetBool("IsMoving", true);
            } else {
                rb.velocity = new Vector3(0, 0, 0);
                animator.SetBool("IsMoving", false);
            }
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag.Equals("Enemy") && !gameFinished) {
            text.text = "You Lose!";
            gameFinished = true;
        } else if (collision.gameObject.tag.Equals("Finish") && !gameFinished) {
            text.text = "You Win!";
            gameFinished = true;
        }
    }
}
