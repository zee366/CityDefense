using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.AI;
using System;

[RequireComponent(typeof(Collider))]
public class FlockAgent : MonoBehaviour
{

    Flock agentFlock;
    Animator animator;
    MeshRenderer[] meshs;
    SkinnedMeshRenderer[] skinmeshes;
    bool isdestroyable=false;
    float lifecycle = 30.0f;
    float timetodestroy = 0.2f;
    float fadeSpeed=0.05f;  
    MaterialPropertyBlock _propBlock; 
    float opacity;
    NavMeshHit closestHit;
    float agentspeed;
    private bool mutexlock;
    private IEnumerator coroutine;
    private float toggletime=0f;
    private float formationtoggletime=0f;
    public bool partOfFlock;
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
        meshs = GetComponentsInChildren<MeshRenderer>();
        skinmeshes = GetComponentsInChildren<SkinnedMeshRenderer>();
        opacity = 1;
        mutexlock=true;
    }

    void Update(){
        lifecycle-=Time.deltaTime;
        toggletime-=Time.deltaTime;
        formationtoggletime-=Time.deltaTime;
        
        if(isdestroyable&&lifecycle<0){
            partOfFlock=false;
            DissolveandDestroy();
        }
        if(this.GetComponent<Rigidbody>().velocity.magnitude>3){
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
            if(((this.transform.position-hit.point).magnitude<10)&&!isdestroyable){
                navMeshAgent.velocity=new Vector3();
            }
        }
        if (Input.GetMouseButtonDown(0) && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject() && partOfFlock) {
                navMeshAgent.isStopped=false;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray,out hit)) {
                    StopAllCoroutines();
                    navMeshAgent.destination=hit.point;
                }
        }
        if(partOfFlock){
            ////////////////////////////////////////////
           /////////////LINE FORMATION//////////////////
          ///////////////////////////////////////////// 
          /*if(gameObject.name=="Agent 0"){
            if(Input.GetKey(KeyCode.LeftShift)||Input.GetKey(KeyCode.RightShift)){
                if(Input.GetKeyDown(KeyCode.LeftArrow)&&formationtoggletime<0){
                    //Line formation facing left
                    //print("Face left and form line");
                    //
                    formationtoggletime=2f;
                    Quaternion rotation = Quaternion.Euler(0,-90,0);
                    StopCoroutine("Rotate");
                    agentFlock.FormVerticalLine(rotation);
                }
                else if(Input.GetKeyDown(KeyCode.RightArrow)&&formationtoggletime<0){
                    //Line formation facing right
                    // print("Face right and form line");
                    //
                    formationtoggletime=2f;
                    Quaternion rotation = Quaternion.Euler(0,90,0);
                    StopCoroutine("Rotate");
                    agentFlock.FormVerticalLine(rotation);
                }
                else if(Input.GetKeyDown(KeyCode.UpArrow)&&formationtoggletime<0){
                    //Line formation facing left
                    //print("Face forward and form line");
                    //
                    formationtoggletime=2f;
                    Quaternion rotation = Quaternion.Euler(0,0,0);
                    StopCoroutine("Rotate");
                    agentFlock.FormHorizontalLine(rotation);
                }
                else if(Input.GetKeyDown(KeyCode.DownArrow)&&formationtoggletime<0){
                    //Line formation facing right
                    //print("Face backward and form line");
                    //
                    formationtoggletime=2f;
                    Quaternion rotation = Quaternion.Euler(0,180,0);
                    StopCoroutine("Rotate");
                    agentFlock.FormHorizontalLine(rotation);
                }

            }
            ////////////////////////////////////////////
           /////////////CIRCLE FORMATION////////////////
          ///////////////////////////////////////////// 
                if(Input.GetKeyDown(KeyCode.C)&&formationtoggletime<0){
                    formationtoggletime=2f;
                    StopCoroutine("Rotate");
                    agentFlock.FormCircle();
                    //FaceAway();
                }
          }*/
            ////////////////////////////////////////////
           /////////////SIMPLE ROTATION////////////////
          ///////////////////////////////////////////// 
        if(Input.GetKey(KeyCode.LeftShift)==false){
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
        //navMeshAgent.destination=gameObject.transform.position;
        agentFlock.CR_running=false;
        yield return null;
    }

    private void DissolveandDestroy()
    {
        //Temp fallback position is set to donut shop
        navMeshAgent.SetDestination(new Vector3(106.9f,4.16f,-157.41f));
        mutexlock=false;
        opacity -= timetodestroy*Time.deltaTime;
        if(opacity<=0){
            //remove from flock
            agentFlock.RemoveAgent(this);
            //destroy reinforcement
            Destroy(gameObject);
        }
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

        //if(centre!=new Vector3()){
            //navMeshAgent.destination=centre;
            //navMeshAgent.speed=15f;
      //  }
      //Debug.Log(velocity);
        gameObject.transform.position = Vector3.Lerp(gameObject.transform.position,velocity,0.2f*Time.deltaTime);
    }

    public float Distance(){
        return (((this.transform.position-hit.point).magnitude)
        +((this.transform.position-centre).magnitude));
    }


    public bool get_isdestroyable
    {
        get { return isdestroyable;} 
    }

    public void set_isdestroyable(bool b)
    {
        isdestroyable=b; 
    }

}
