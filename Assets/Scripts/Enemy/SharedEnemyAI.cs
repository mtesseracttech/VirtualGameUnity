using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

public class SharedEnemyAI : MonoBehaviour
{
    private List<EnemyAgent> _agents;

    public void RegisterAgent(EnemyAgent agent)
    {
        Debug.Log("Registering Agent: " + agent.Parent.gameObject.name);
        _agents.Add(agent);
    }

	// Use this for initialization
    void Awake()
    {
        _agents = new List<EnemyAgent>();
    }
	
	// Update is called once per frame
    void Update()
    {
        
    }

    private void NotifyOthersOfPosition(Vector3 lastSeenLocation)
    {
        foreach (var enemyAgent in _agents) enemyAgent.LastSeenTargetPosition = lastSeenLocation;
    }

    private void NotifyAgentOfPos(Vector3 lastSeenLocation, EnemyAgent agent)
    {
        if (!(agent.GetState() is ChaseState))
        {
            agent.LastSeenTargetPosition = lastSeenLocation;
        }
    }

    public void SearchInRangeAgent(EnemyAgent notifierAgent, int range)
    {
        //NotifyOthersOfPosition(notifierAgent.LastSeenTargetPosition);

        int searchingAgents = 0;
        foreach (var otherAgent in _agents)
        {
            if (otherAgent != notifierAgent)
            {
                if (Vector3.Distance(notifierAgent.Parent.transform.position, otherAgent.Parent.transform.position) < range)
                {
                    NotifyAgentOfPos(notifierAgent.LastSeenTargetPosition, otherAgent);
                    otherAgent.SetState(typeof(ChaseState));
                    searchingAgents++;
                }
            }
        }
        Debug.Log(searchingAgents + " other agents start searching!");
    }

    public void SearchInRangeVec3(Vector3 location, int range)
    {
        //NotifyOthersOfPosition(location);

        int searchingAgents = 0;
        foreach (var otherAgent in _agents)
        {
            if (Vector3.Distance(location, otherAgent.Parent.transform.position) < range)
            {
                NotifyAgentOfPos(location, otherAgent);
                otherAgent.SetState(typeof(ChaseState));
                searchingAgents++;
            }
        }
        Debug.Log(searchingAgents + " other agents start searching!");
    }

    public void UnRegisterAgent(EnemyAgent destroyedAgent)
    {
        _agents.Remove(destroyedAgent);
    }
}
