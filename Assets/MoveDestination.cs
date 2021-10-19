    // MoveDestination.cs
    using UnityEngine;
    using UnityEngine.AI;
    
    public class MoveDestination : MonoBehaviour {
       
       private Transform goal;
       
       private void Start() {
          goal = GameObject.FindWithTag("Player").transform;
       }

       void Update () {
          UnityEngine.AI.NavMeshAgent agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
          agent.destination = goal.position; 
       }
    }
