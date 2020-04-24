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
    float timetodie;
    float lifecycle=30f;
    private bool mutexlock;
    private float toggletime=0.5f;
    public NavMeshAgent navMeshAgent;
    public RaycastHit hit;

    public Vector3 centre;
    public bool isdestroyable=false;

    public Flock AgentFlock { get { return agentFlock; } }

    Collider agentCollider;
    public Collider AgentCollider { get { return agentCollider; } }

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        agentCollider = GetComponent<Collider>();
        mutexlock=true;
       
    }

    void Update(){
        //print((gameObject.transform.position-closestHit.position).magnitude);
        //print(this.GetComponent<Rigidbody>().velocity.magnitude);
        lifecycle-=Time.deltaTime;
        toggletime-=Time.deltaTime;        
        if(isdestroyable&&lifecycle<0){
            partOfFlock=false;
            navMeshAgent.isStopped=false;
            StartCoroutine(DissolveandDestroy());
        }
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
            if(partOfFlock){
                if (Input.GetMouseButtonDown(1) && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) {
                    StopAllCoroutines();
                    navMeshAgent.isStopped=false;
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray,out hit)) {
                        navMeshAgent.destination=hit.point;
                    }
                }
                if((this.transform.position-hit.point).magnitude<10){
                    navMeshAgent.velocity=new Vector3();
                }
                if(Input.GetKeyDown(KeyCode.LeftArrow)&&toggletime<0){
                    toggletime=0.5f;
                    StopCoroutine("Rotate");
                    FaceLeft();
                }
                else if(Input.GetKeyDown(KeyCode.RightArrow)&&toggletime<0){
                    toggletime=0.5f;
                    StopCoroutine("Rotate");
                    FaceRight();
                }
                else if(Input.GetKeyDown(KeyCode.UpArrow)&&toggletime<0){
                    toggletime=0.5f;
                    StopCoroutine("Rotate");
                    FaceForward();
                }
                else if(Input.GetKeyDown(KeyCode.DownArrow)&&toggletime<0){
                    toggletime=0.5f;
                    StopCoroutine("Rotate");
                    FaceBackward();
                }
            }
        }
        
    }
    public void FaceBackward()
    {
        Quaternion rotation = Quaternion.Euler(0,180,0);
        StartCoroutine(Rotate(rotation));
    }

    public void FaceForward()
    {
        Quaternion rotation = Quaternion.Euler(0,0,0);
        StartCoroutine(Rotate(rotation));
    }

    public void FaceRight()
    {
        Quaternion rotation = Quaternion.Euler(0,90,0);
        StartCoroutine(Rotate(rotation));
    }

    public void FaceLeft()
    {
        Quaternion rotation = Quaternion.Euler(0,-90,0);
        StartCoroutine(Rotate(rotation));
    } 
     
   public IEnumerator Rotate(Quaternion rotation) {
       navMeshAgent.isStopped=true;
       float lerp = 0;
       float lerpspeed= Time.deltaTime/lerp;
            while(Quaternion.Angle(gameObject.transform.rotation, rotation) > 5f) {
                lerp+=Time.deltaTime;
                lerpspeed= lerp/1f;
                gameObject.transform.rotation = Quaternion.Lerp(gameObject.transform.rotation, rotation, lerpspeed);
                yield return new WaitForFixedUpdate();
            }
        yield return null;
    }

    private IEnumerator DissolveandDestroy()
    {
        //Temp fallback position is set to donut shop
        navMeshAgent.SetDestination(new Vector3(106.9f,4.16f,-157.41f));
        while((this.transform.position-navMeshAgent.destination).magnitude>30){
            yield return new WaitForFixedUpdate();
        }
        timetodie-=Time.deltaTime;
        if (timetodie<=0){
            //remove from flock
            agentFlock.RemoveAgent(this);
            //destroy reinforcement
            Destroy(gameObject);
        }
        yield return null;
        //MATERIAL FADER FOR LWRP
        /*
        for (int i=0;i<meshs.Length;i++)
        {
            MeshRenderer temp = meshs[i];
            foreach (Material mat in temp.materials)
            {
                mat.SetFloat("_Mode",2);
                mat.SetInt("_SrcBlend",(int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                mat.SetInt("_DstBlend",(int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                mat.SetInt("_ZWrite",0);
                mat.DisableKeyword("_ALPHATEST_ON");
                mat.EnableKeyword("_ALPHABLEND_ON");
                mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                mat.renderQueue = 3000;
            } 
        }
        for (int i=0;i<skinmeshes.Length;i++)
        {
            SkinnedMeshRenderer temp = skinmeshes[i];
            foreach (Material mat in temp.materials)
            {
                
                mat.SetFloat("_Mode",2);
                mat.SetInt("_SrcBlend",(int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                mat.SetInt("_DstBlend",(int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                mat.SetInt("_ZWrite",0);
                mat.DisableKeyword("_ALPHATEST_ON");
                mat.EnableKeyword("_ALPHABLEND_ON");
                mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                mat.renderQueue = 3000;
                
                
            } 
        }
        */
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
       transform.position = Vector3.Lerp(transform.position,velocity,0.2f*Time.deltaTime);
    }

    public float Distance(){
        return (((this.transform.position-hit.point).magnitude)
        +((this.transform.position-centre).magnitude));
    }
}
