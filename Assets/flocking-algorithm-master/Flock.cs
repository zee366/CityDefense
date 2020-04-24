using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using LightType = UnityEngine.LightType;
using Random = UnityEngine.Random;


public class Flock : MonoBehaviour
{
    public FlockAgent agentPrefab;
    public List<FlockAgent> agents = new List<FlockAgent>();
    List<FlockAgent> newAgents = new List<FlockAgent>();
    public FlockBehavior[] behaviors;
    public float[] weights;

    [Range(1, 100)]
    public int startingCount = 3;
    const float AgentDensity = 2f;

    [Range(1f, 100f)]
    public float driveFactor = 10f;
    [Range(1f, 100f)]
    public float maxSpeed = 5f;
    [Range(1f, 100f)]
    public float neighborRadius = 1.5f;
    [Range(0f, 1f)]
    public float avoidanceRadiusMultiplier = 0.5f;
    Vector3 centre;

    float avgDistance;
    float squareMaxSpeed;
    float squareNeighborRadius;
    float squareAvoidanceRadius;
    public GameObject spawnPoint;
    public GameObject donutSpawnPoint;
    [Header("Light")] 
    [Range(20, 100)] public int range = 70;
    [Range(0, 179)] public int spotAngle = 179;
    [Range(0, 20000)] public int intensity = 2;
    [Range(0, 40)] public int height = 19;
    public Color color = new Color(55,55,55);
    public bool SpotLightOnAgentO = false; 
    public float SquareAvoidanceRadius { get { return squareAvoidanceRadius; } }

    private Light _spotLight;
    // Start is called before the first frame update
    void Start()
    {
        InitLight();

        squareMaxSpeed = maxSpeed * maxSpeed;
        squareNeighborRadius = neighborRadius * neighborRadius;
        squareAvoidanceRadius = squareNeighborRadius * avoidanceRadiusMultiplier * avoidanceRadiusMultiplier;
        for (int i = 0; i < startingCount; i++)
        {  
            Vector3 newPosition=  new Vector3(Random.Range(spawnPoint.transform.position.x-startingCount,spawnPoint.transform.position.x+startingCount),1f, Random.Range(spawnPoint.transform.position.z-startingCount,spawnPoint.transform.position.z+startingCount));
            //newPosition.z = Random.Range(-10,10);
            FlockAgent newAgent = Instantiate(
                agentPrefab,
                newPosition,
                Quaternion.Euler(Vector3.forward),
                transform
                );
            newAgent.name = "Agent " + i;
            if ( SpotLightOnAgentO && i == 0 ) newAgent.GetComponent<PoliceAbilities>().equipLamp();
            newAgent.Initialize(this);
            agents.Add(newAgent);
        }
    }


    private void InitLight() {
        Light newLight = new GameObject().AddComponent<Light>();
        newLight.transform.parent        = transform;
        newLight.name = "Spot Light";
        newLight.bounceIntensity = 0;
        newLight.type                    = LightType.Spot;
        newLight.range                   = range;
        newLight.spotAngle               = spotAngle;
        newLight.intensity               = intensity;
        newLight.transform.localPosition = new Vector3(0, height, 0);
        newLight.color = color;
        if (!SpotLightOnAgentO) newLight.transform.rotation = Quaternion.Euler(new Vector3(90, 0, 0));
        _spotLight = newLight;
    }


    private void UpdateParentPositionOfLight(Transform newOwner) {
        _spotLight.transform.parent = newOwner;
        if ( SpotLightOnAgentO ) {
            _spotLight.transform.localPosition = new Vector3(0.36f, 0.7f, 0.65f);
            
        } else {
            _spotLight.transform.localPosition = new Vector3(0, 19, 0);
        }
    }

    // Update is called once per frame
    void FixedUpdate() {
        // This update the "owner" of the light
        // Not sure what is the best solution? Right now it sets the position of the light with Agent 0
        // It could be with the avg position but then it creates weird behaviour when a squad splits in two
        if ( agents.Count != 0) {
            UpdateParentPositionOfLight(agents[0].transform);
        }
        
        foreach (FlockAgent agent in agents)
        {
            
            List<Transform> context = GetNearbyObjects(agent);
            
            Vector3 avoidancemove = behaviors[0].CalculateMove(agent, context, this);
            Vector3 move = agent.gameObject.transform.position+avoidancemove;

            if(SquadisFormed()){
                if(Time.time > .2f) {
                    if (agent.navMeshAgent)
                    {
                        agent.navMeshAgent.enabled = true;
                        agent.navMeshAgent.speed = 20f;
                        if (move != agent.transform.position)
                        {
                            agent.Move(move);
                        }
                    }
                }
            }
            else if(!SquadisFormed()){
                if(agent.Distance()>avgDistance*1.5&&Time.time > .2f){
                    if (agent.navMeshAgent)
                        agent.navMeshAgent.speed = 40f;
                }

            }
            
        }
        /*
        if (newAgents.Count != 0)
        {
            int size = newAgents.Count;
            for (int i = 0; i < size; i++)
            {
                size=newAgents.Count;
                if (newAgents[i].navMeshAgent)
                {
                    newAgents[i].navMeshAgent.destination = AveragePositionOfFlock();
                    newAgents[i].navMeshAgent.speed = 40f;

                    if ((newAgents[i].transform.position - newAgents[i].navMeshAgent.destination).magnitude <= 1.0f)
                    {
                        agents.Add(newAgents[i]);
                        newAgents[i].Initialize(this);
                        newAgents.Remove(newAgents[i]);
                    }
                }
            }
        }*/
    }

    private bool SquadisFormed()
    {
            int count =0;
            if(agents.Count == 0)
            {
                return false;
            }
        
            Vector3 meanVector = Vector3.zero;
            float meanDitance = 0;
        
            foreach(FlockAgent agent in agents)
            {
                if(agent.partOfFlock){
                    meanVector += agent.gameObject.transform.position;
                    meanDitance+= agent.Distance();
                }
            }
            avgDistance = (meanDitance/agents.Count);
            centre = (meanVector / agents.Count);
            foreach(FlockAgent agent in agents)
            {
                if((agent.gameObject.transform.position-centre).magnitude<agents.Count*2f&&agent.partOfFlock){
                    count++;
                    agent.centre=new Vector3();
                }
                else{
                    agent.centre=centre;
                }
            }
            if(count==agents.Count){
                return true;
            }
            else
                return false;
        
    }

    List<Transform> GetNearbyObjects(FlockAgent agent)
    {
        List<Transform> context = new List<Transform>();
        
        Collider[] contextColliders = Physics.OverlapSphere(agent.transform.position, neighborRadius);
        foreach (Collider c in contextColliders)
        {
            if (c != agent.AgentCollider)
            {
                context.Add(c.transform);
            }
        }
        return context;
    }

    //Function for reinforce squad from police abilities
    public void AddAgent()
    {
        FlockAgent newAgent = Instantiate(
                agentPrefab,
                donutSpawnPoint.transform.position,
                Quaternion.Euler(Vector3.forward),
                transform
                );
        newAgent.name = "Agent " + agents.Count;
        newAgent.isdestroyable=true;
        newAgents.Add(newAgent);
        StartCoroutine(Reinforcement(newAgent));
        //agents.Add(newAgent);

    }

    public void RemoveAgent(FlockAgent agent){
        agents.Remove(agent);
    }

    private Vector3 AveragePositionOfFlock()
    {
        Vector3 avgPosition = new Vector3();
        //foreach (FlockAgent fA in agents)
        for(int i = 0; i < agents.Count; i++)
        {
            FlockAgent fA = agents[i];
            avgPosition += fA.transform.position;
        }
        return avgPosition /= agents.Count;
    }

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(centre, startingCount*2f);
    }
    IEnumerator Reinforcement(FlockAgent agent) {
        while(!agent.navMeshAgent){
            yield return new WaitForFixedUpdate();
        }
        agent.navMeshAgent.destination = AveragePositionOfFlock();
        agent.navMeshAgent.speed = 40f;
        while ((agent.transform.position - agent.navMeshAgent.destination).magnitude <= 2.0f)
        {
            yield return new WaitForFixedUpdate();

        }
        agents.Add(agent);
        agent.Initialize(this);
        newAgents.Remove(agent);
        yield return null;
    }
}
