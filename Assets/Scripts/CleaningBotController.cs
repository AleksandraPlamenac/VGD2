using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class CleaningBotController : MonoBehaviour
{
    public NavMeshAgent agent;
    public Animator animator;

    enum BotState
    {
        Wandering,
        HuntingForBox,
        Dispose,
    }
    
    private BotState state;
    private AStarSearcher searcher;
    private GameObject disposalSite;
    private GameObject targetBox;
    private GameObject heldBox;
    private GameObject holder;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        searcher = GetComponent<AStarSearcher>();
        animator = GetComponent<Animator>();
        disposalSite = GameObject.Find("DisposalSite");
        holder = transform.Find("model/CleanerHolder").gameObject;
        
        ToWanderingState();
    }

    void Update()
    {
        // UpdateStateMachine();
        RunCurrentAction();
    }
    
    void RunCurrentAction()
    {
        switch (state)
        {
            case BotState.Wandering:
                WanderingAction();
                break;
            case BotState.HuntingForBox:
                HuntingAction();
                break;
            case BotState.Dispose:
                DisposalAction();
                break;
            default:
                Debug.LogError("Unknown state");
                break;
        }        
    }

    private void DisposalAction()
    {
        if (agent.remainingDistance < 0.6)
        {
            // throw to disposal
            heldBox.transform.parent = null;
            heldBox.tag = Common.DISPOSED_BOX_TAG;
            Invoke("DestroyBox", 1.0f);
        }

        agent.SetDestination(disposalSite.transform.position);
    }

    private void DestroyBox()
    {
        Destroy(heldBox);
        ToWanderingState();
    }

    private void HuntingAction()
    {
        var isAbleToPickup = agent.remainingDistance < 0.9;
        if (isAbleToPickup)
        {
            ToDisposeState(targetBox);
            return;
        }
        
        if (!targetBox.CompareTag(Common.RESOURCE_BOX_TAG))
        {
            ToWanderingState();
            return;
        }

        agent.SetDestination(targetBox.transform.position);
    }
    
    private void WanderingAction()
    {
        var isAbleToGoHunting = agent.remainingDistance < 0.6; // FIXME: Put a cooldown timer
        if (isAbleToGoHunting)
        {
            var filthyBox = searcher.FindObjectToPickup(Common.RESOURCE_BOX_TAG);
            if (filthyBox != null)
            {
                ToHuntingState(filthyBox);
                return;
            }
        }
        
        // when we reached destination and there is no box to pickup
        if (agent.remainingDistance < 0.6)
        {
            ToWanderingState();
        }
        // animator.SetFloat("vely", agent.velocity.magnitude / agent.speed);
    }

    
    private void ToDisposeState(GameObject filthyBox)
    {
        state = BotState.Dispose;
        heldBox = filthyBox;
        targetBox = null;
        
        heldBox.tag = Common.BOX_HELD_BY_BOT_TAG;
        heldBox.transform.SetParent(holder.transform);
        heldBox.transform.localPosition = Vector3.zero;

        var boxRb = heldBox.GetComponent<Rigidbody>();
        boxRb.isKinematic = true;
        boxRb.detectCollisions = false;
        boxRb.constraints = RigidbodyConstraints.FreezeAll;

        // set target dest
        agent.SetDestination(disposalSite.transform.position);
        Debug.Log("Going to Dispose State");        
    }

    private void ToHuntingState(GameObject filthyBox)
    {
        state = BotState.HuntingForBox;
        targetBox = filthyBox;
        agent.SetDestination(targetBox.transform.position);
        Debug.Log("Going to Hunting State");
    }
    
    private void ToWanderingState()
    {
        state = BotState.Wandering;
        var triangulation = NavMesh.CalculateTriangulation();
        var numVertices = triangulation.vertices.Length;

        var target = triangulation.vertices[Random.Range(0, numVertices)];
        agent.SetDestination(target);
        targetBox = null;
        heldBox = null;
        Debug.Log("Going to Wondering State");
    }
}
