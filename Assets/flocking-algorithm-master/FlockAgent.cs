using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.AI;

[RequireComponent(typeof(Collider))]
public class FlockAgent : MonoBehaviour
{

    Flock agentFlock;
    public bool partOfFlock;

    Animator animator;

    NavMeshHit closestHit;

    float agentspeed;

    public NavMeshAgent navMeshAgent;
    public RaycastHit hit;

    public Vector3 centre;

    public Flock AgentFlock { get { return agentFlock; } }

    Collider agentCollider;
    public Collider AgentCollider { get { return agentCollider; } }

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        agentCollider = GetComponent<Collider>();
       
    }

    void Update(){
        //print((gameObject.transform.position-closestHit.position).magnitude);
        //print(this.GetComponent<Rigidbody>().velocity.magnitude);
        if(this.GetComponent<Rigidbody>().velocity.magnitude>2){
            animator.SetBool("IsRunning",true);
        }
        else{
            animator.SetBool("IsRunning",false);
        }
        if(Time.time > .1f) {
            GetComponent<NavMeshAgent>().enabled = true; 
            navMeshAgent = GetComponent<NavMeshAgent>();
            if(navMeshAgent.velocity.magnitude>3){
                animator.SetBool("IsRunning",true);
            }
            else{
                animator.SetBool("IsRunning",false);
            }
            if((this.transform.position-hit.point).magnitude<10){
                navMeshAgent.velocity=new Vector3();
            }
        }
        if (Input.GetMouseButtonDown(0) && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject() && partOfFlock) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray,out hit)) {
                navMeshAgent.destination=hit.point;
            }
        }
    }

    public void Initialize(Flock flock)
    {
        partOfFlock = true;
        agentFlock = flock;
    }

    public void Move(Vector3 velocity)
    {

        if(centre!=new Vector3()){
            //navMeshAgent.destination=centre;
            navMeshAgent.speed=15f;
        }
        //transform.forward = velocity;
       //transform.position += (Vector3)velocity * Time.deltaTime;
    }

    public float Distance(){
        return (((this.transform.position-hit.point).magnitude)
        +((this.transform.position-centre).magnitude));
    }
}
