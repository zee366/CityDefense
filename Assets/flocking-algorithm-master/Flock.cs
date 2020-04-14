using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
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

    public float SquareAvoidanceRadius { get { return squareAvoidanceRadius; } }

    // Start is called before the first frame update
    void Start()
    {
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
            newAgent.Initialize(this);
            agents.Add(newAgent);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        foreach (FlockAgent agent in agents)
        {
            
            List<Transform> context = GetNearbyObjects(agent);
            
            //Vector3 move = behavior.CalculateMove(agent, context, this);
            Vector3 move = Vector3.zero;

        //iterate through behaviors
            for (int i = 0; i < behaviors.Length; i++)
            {
                Vector3 partialMove = behaviors[i].CalculateMove(agent, context, this) * weights[i];

                if (partialMove != Vector3.zero)
                {
                    if (partialMove.sqrMagnitude > weights[i] * weights[i])
                    {
                        partialMove.Normalize();
                        partialMove *= weights[i];
                    }

                    move += partialMove;

                }
            }
            move *= driveFactor;
            if (move.sqrMagnitude > squareMaxSpeed)
            {
                move = move.normalized * maxSpeed;
            }
            if(SquadisFormed()){
                if(Time.time > .2f) {
                    agent.navMeshAgent.enabled=true;
                    //agent.navMeshAgent.destination=agent.hit.point;
                    agent.navMeshAgent.speed=20f;
                }
                //print(agent.name+" is idling");
            }
            else if(!SquadisFormed()){
                //print(agent.name + " total distance is " + agent.Distance());
                if(agent.Distance()>avgDistance*1.5&&Time.time > .2f){
                    if (agent.navMeshAgent)
                        agent.navMeshAgent.speed = 40f;
                }
                if(Time.time > .2f) {
                    //agent.Move(move);
                }
            }
            
        }

        if (newAgents.Count != 0)
        {
            int size = newAgents.Count;
            for (int i = 0; i < size; i++)
            {
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
        }
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
                meanVector += agent.gameObject.transform.position;
                meanDitance+= agent.Distance();
            }
            avgDistance = (meanDitance/agents.Count);
            centre = (meanVector / agents.Count);
            foreach(FlockAgent agent in agents)
            {
                if((agent.gameObject.transform.position-centre).magnitude<startingCount*2f){
                    count++;
                    agent.centre=new Vector3();
                }
                else{
                    agent.centre=centre;
                }
            }
            if(count==startingCount){
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
        newAgents.Add(newAgent);
        //agents.Add(newAgent);

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

}
