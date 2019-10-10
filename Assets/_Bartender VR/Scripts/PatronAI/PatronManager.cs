using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatronManager : MonoBehaviour
{
    [Header("Patron Options")]
    [SerializeField] int minPatrons;
    [SerializeField] int maxPatrons;
    [SerializeField] [Range(0f, 1f)] float patronGenerationChance;
    [SerializeField] float timeBetweenGeneration, patronSpeed;
    [SerializeField] float[] idleTimes;
    
    public static float thirstThreshold;

    [Header("Required Fields")]
    [SerializeField] public List<GameObject> spots;
    [SerializeField] public List<GameObject> coasters;
    [SerializeField] public GameObject spawnLocation, exitLocation, patronPrefab;

    [Header("Generated Fields")]
    [SerializeField] public List<PatronAI> patrons;

    float currTime;
    IBehaviour root;

    void Start()
    {
        if (spots == null) { Debug.LogError("Spots cannot be empty"); }
        if (spawnLocation == null) { Debug.LogError("Spawn Location cannot be empty"); }
        if (patrons == null) { patrons = new List<PatronAI>(); }
        root = PopulateBehaviours();
    }


    void Update()
    {
        currTime += Time.deltaTime;
        while (patrons.Count < minPatrons) { patrons.Add(GeneratePatron()); }

        if (currTime >= timeBetweenGeneration)
        {
            currTime = 0f;
            if (Random.Range(0f, 1f) <= patronGenerationChance)
            {
                if (patrons.Contains(null))
                {
                    for(int i = 0; i < patrons.Count; i++)
                    {
                        if(patrons[i] == null)
                        {
                            patrons[i] = GeneratePatron();
                        }
                    }
                } else if(patrons.Count < maxPatrons)
                {
                    patrons.Add(GeneratePatron());
                }
            }
        }
        for (int i = 0; i < patrons.Count; i++) { if (patrons[i] != null) { root.DoBehaviour(patrons[i]); } }
    }

    PatronAI GeneratePatron()
    {
        var patron = Instantiate(patronPrefab);
        patron.transform.position = spawnLocation.transform.position;
        patron.layer = 11;
        var agent = patron.GetComponent<PatronAI>();
        agent.patronManager = this;
        var nav = patron.GetComponent<NavMeshAgent>();
        agent.navAgent = nav;
        nav.speed = patronSpeed;
        nav.autoRepath = false;
        nav.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
        nav.Warp(patron.transform.position);
        var ani = patron.GetComponent<Animator>();
        agent.animator = ani;
        agent.desiredDrink = (DrinkSystemManager.DrinkNames)Random.Range(0, 11);
        return agent;
    }

    public void SeatMe(PatronAI patron)
    {
        patron.transform.position = spots[patrons.IndexOf(patron)].transform.position;
        patron.transform.rotation = spots[patrons.IndexOf(patron)].transform.rotation;
    }

    private List<IBehaviour> PopulateBranch(params IBehaviour[] children)
    {
        var t = new List<IBehaviour>();
        foreach (IBehaviour p in children) t.Add(p);
        return t;
    }

    IBehaviour PopulateBehaviours()
    {
        var root = new SelectorNode();
        var seqStates = new SequenceNode[10];
        for(int i = 0; i < seqStates.Length; i++) { seqStates[i] = new SequenceNode(); }
        var stateNodes = new State[10];
        var switchNodes = new SwitchState[10];

        //root
        root.childBehaviors = PopulateBranch(seqStates[0],
                                             seqStates[1],
                                             seqStates[2],
                                             seqStates[3],
                                             seqStates[5],
                                             seqStates[6],
                                             seqStates[7],
                                             seqStates[8]);

        //state 0
        stateNodes[0] = new State(0);
        var findSpot = new FindSpot();
        var atBar = new ThereYet();
        var sitDown = new SitDown();
        switchNodes[0] = new SwitchState(1);
        seqStates[0].childBehaviors = PopulateBranch(stateNodes[0], findSpot, atBar, sitDown, switchNodes[0]);

        //state 1
        stateNodes[1] = new State(1);
        var idle = new Idle();
        var wait0 = new WaitedLongEnough(idleTimes[0]);
        switchNodes[1] = new SwitchState(2);
        seqStates[1].childBehaviors = PopulateBranch(stateNodes[1], idle, wait0, switchNodes[1]);

        //state 2
        stateNodes[2] = new State(2);
        var orderDrink = new OrderDrink();
        switchNodes[2] = new SwitchState(3);
        seqStates[2].childBehaviors = PopulateBranch(stateNodes[2], orderDrink, switchNodes[2]);

        //state 3
        stateNodes[3] = new State(3);
        //idle
        var sel0 = new SelectorNode();
        seqStates[3].childBehaviors = PopulateBranch(stateNodes[3], idle, sel0);

        //sel0
        var seq0 = new SequenceNode();
        var seq1 = new SequenceNode();
        sel0.childBehaviors = PopulateBranch(seq0, seq1);

        //seq0
        var gotDrink = new GotDrink();
        switchNodes[3] = new SwitchState(4);
        seq0.childBehaviors = PopulateBranch(gotDrink, switchNodes[3]);

        //seq1
        var wait1 = new WaitedLongEnough(idleTimes[1]);
        switchNodes[4] = new SwitchState(5);
        seq1.childBehaviors = PopulateBranch(wait1, switchNodes[4]);

        //state4
        //drink

        //state 5
        stateNodes[5] = new State(5);
        var hideOrder = new HideOrder();
        switchNodes[5] = new SwitchState(6);
        seqStates[5].childBehaviors = PopulateBranch(stateNodes[5], hideOrder, switchNodes[5]);

        //state 6
        stateNodes[6] = new State(6);
        //idle
        var sel1 = new SelectorNode();
        seqStates[6].childBehaviors = PopulateBranch(stateNodes[6], idle, sel1);

        //sel1
        //seq0
        var seq2 = new SequenceNode();
        sel1.childBehaviors = PopulateBranch(seq0, seq2);

        //seq2
        var wait2 = new WaitedLongEnough(idleTimes[2]);
        switchNodes[6] = new SwitchState(7);
        seq2.childBehaviors = PopulateBranch(wait2, switchNodes[6]);

        //state 7
        stateNodes[7] = new State(7);
        var leave = new Leave();
        switchNodes[7] = new SwitchState(8);
        seqStates[7].childBehaviors = PopulateBranch(stateNodes[7], leave, switchNodes[7]);

        //state 8
        stateNodes[8] = new State(8);
        var outside = new ThereYet();
        var die = new Die();
        switchNodes[8] = new SwitchState(9);
        seqStates[8].childBehaviors = PopulateBranch(stateNodes[8], outside, die, switchNodes[8]);


        return root;
    }
}