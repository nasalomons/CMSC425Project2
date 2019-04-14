using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyScript : MonoBehaviour {
    private NavMeshAgent agent;
    private bool isPaused;
    private bool isDisabled;

    public GameObject goal;
    public float chaseThreshold;

    // Start is called before the first frame update
    void Start() {
        agent = GetComponent<NavMeshAgent>();
        isPaused = false;
        isDisabled = false;
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.C)) {
            isDisabled = isDisabled ? false : true;
        } else if (Input.GetKeyDown(KeyCode.P)) {
            isPaused = isPaused ? false : true;
        }

        if (isPaused || isDisabled) {
            agent.destination = agent.transform.position;
        }
    }

    private void FixedUpdate() {
        if (!isPaused && !isDisabled) {
            Vector3 vector = goal.transform.position - agent.transform.position;
            if (vector.magnitude <= chaseThreshold) {
                agent.destination = goal.transform.position;
            }
        }
    }
}
