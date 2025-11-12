using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public enum Transition
{
    NullTransition = 0,
    SawPlayer = 1,
    LostPlayer = 2,
}

public enum StateID
{
    NullStateID = 0,
    PatrollingID = 1,
    ChasingPlayerID = 2,
}

public class EnemyBehaviour : MonoBehaviour
{
    public FSMSystem fsm;
    public GameObject player;
    public Transform[] wp;

    public float fieldOfViewAngle = 55f;
    public float sightRange = 20f;

    void Start()
    {
        MakeFSM();
    }

    private void MakeFSM()
    {
        PatrolState patrol = new PatrolState(gameObject, wp);
        patrol.AddTransition(Transition.SawPlayer, StateID.ChasingPlayerID);

        ChasePlayerState chase = new ChasePlayerState(gameObject, player);
        chase.AddTransition(Transition.LostPlayer, StateID.PatrollingID);

        fsm = new FSMSystem();
        fsm.AddState(patrol);
        fsm.AddState(chase);
    }

    public void SetTransition(Transition t)
    {
        fsm.PerformTransition(t);
    }

    void Update()
    {
        fsm.CurrentState.Reason(player, gameObject);
        fsm.CurrentState.Act(player, gameObject);
    }

    public bool PlayerInSight(GameObject player, GameObject npc)
    {
        Vector3 direction = player.transform.position - npc.transform.position;
        float distance = direction.magnitude;

        if (distance > sightRange)
            return false;

        float angle = Vector3.Angle(npc.transform.forward, direction);
        if (angle < fieldOfViewAngle * 0.5f)
        {
            RaycastHit hit;
            if (Physics.Raycast(npc.transform.position + Vector3.up, direction.normalized, out hit, sightRange))
            {
                if (hit.collider.gameObject == player)
                    return true;
            }
        }

        return false;
    }
}

public class PatrolState : FSMState
{
    private int currentWayPoint;
    private Transform[] waypoints;
    private EnemyAnimation enemyAnimation;
    private float patrolSpeed = 2.5f;

    public PatrolState(GameObject thisObject, Transform[] wp)
    {
        waypoints = wp;
        currentWayPoint = 0;
        stateID = StateID.PatrollingID;
        enemyAnimation = thisObject.GetComponent<EnemyAnimation>();
    }

    public override void Reason(GameObject player, GameObject npc)
    {
        if (npc.GetComponent<EnemyBehaviour>().PlayerInSight(player, npc))
        {
            npc.GetComponent<EnemyBehaviour>().SetTransition(Transition.SawPlayer);
        }
    }

    public override void Act(GameObject player, GameObject npc)
    {
        if (waypoints.Length == 0) return;

        float dist = Vector3.Distance(npc.transform.position, waypoints[currentWayPoint].position);

        if (dist < 1.0f)
        {
            currentWayPoint = (currentWayPoint + 1) % waypoints.Length;
        }

        enemyAnimation.setTarget(waypoints[currentWayPoint], patrolSpeed);
    }
}

public class ChasePlayerState : FSMState
{
    private EnemyAnimation enemyAnimation;
    private float chaseSpeed = 4f;
    private float stopDist = 1.5f;

    public ChasePlayerState(GameObject thisObject, GameObject tgt)
    {
        stateID = StateID.ChasingPlayerID;
        enemyAnimation = thisObject.GetComponent<EnemyAnimation>();
    }

    public override void Reason(GameObject player, GameObject npc)
    {
        if (!npc.GetComponent<EnemyBehaviour>().PlayerInSight(player, npc))
        {
            npc.GetComponent<EnemyBehaviour>().SetTransition(Transition.LostPlayer);
        }
    }

    public override void Act(GameObject player, GameObject npc)
    {
        float distance = Vector3.Distance(player.transform.position, npc.transform.position);

        if (distance > stopDist)
        {
            enemyAnimation.setTarget(player.transform, chaseSpeed);
        }
        else
        {
            enemyAnimation.Stop();
        }
    }
}
