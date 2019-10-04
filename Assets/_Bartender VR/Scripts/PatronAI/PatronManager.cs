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
    [SerializeField] public GameObject spawnLocation, exitLocation;

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
        if (patrons.Count < minPatrons) { patrons.Add(GeneratePatron()); }

        if (currTime >= timeBetweenGeneration)
        {
            currTime = 0f;
            if (Random.Range(0f, 1f) <= patronGenerationChance && patrons.Count < maxPatrons)
            {
                patrons.Add(GeneratePatron());
            }
        }

        foreach(PatronAI p in patrons) { root.DoBehaviour(p); }
    }

    PatronAI GeneratePatron()
    {
        GameObject body = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        body.transform.position = spawnLocation.transform.position;
        PatronAI p = body.AddComponent<PatronAI>();
        p.patronManager = this;
        p.agent = body.AddComponent<NavMeshAgent>();
        p.agent.speed = patronSpeed;
        return p;
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
        var seqStates = new SequenceNode[8];
        for(int i = 0; i < seqStates.Length; i++) { seqStates[i] = new SequenceNode(); }
        var stateNodes = new State[8];
        var switchNodes = new SwitchState[8];

        //root
        root.childBehaviors = PopulateBranch(seqStates[0],
                                             seqStates[1],
                                             seqStates[2],
                                             seqStates[3],
                                             seqStates[4],
                                             seqStates[5],
                                             seqStates[6],
                                             seqStates[7]);

        //state 0
        stateNodes[0] = new State(0);
        var findSpot = new FindSpot();
        var atBar = new AtBar();
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
        var dbg1 = new DebugNode("Debug 3");
        var sel0 = new SelectorNode();
        seqStates[3].childBehaviors = PopulateBranch(stateNodes[3], dbg1, sel0);

        //sel0
        var seq0 = new SequenceNode();
        var seq1 = new SequenceNode();
        sel0.childBehaviors = PopulateBranch(seq0, seq1);

        //seq0
        //idle
        var dbg3 = new DebugNode("Debug 3.5 switch to 4");
        var gotDrink = new GotDrink();
        switchNodes[3] = new SwitchState(4);
        seq0.childBehaviors = PopulateBranch(dbg3, idle, gotDrink, switchNodes[3]);

        //seq1
        var dbg2 = new DebugNode("Debug 3.5 switch to 5");
        var wait1 = new WaitedLongEnough(idleTimes[1]);
        switchNodes[4] = new SwitchState(5);
        seq1.childBehaviors = PopulateBranch(dbg2, wait1, switchNodes[4]);

        //state4
        //drink

        //state 5
        stateNodes[5] = new State(5);
        var dbg = new DebugNode("Debug 5");
        var hideOrder = new HideOrder();
        switchNodes[5] = new SwitchState(6);
        seqStates[5].childBehaviors = PopulateBranch(stateNodes[5], dbg, hideOrder, switchNodes[5]);

        //state 6
        stateNodes[6] = new State(6);
        //idle
        var q3 = new WaitedLongEnough(idleTimes[2]);
        switchNodes[6] = new SwitchState(7);
        seqStates[6].childBehaviors = PopulateBranch(stateNodes[6], idle, q3, switchNodes[6]);

        //state 7
        stateNodes[7] = new State(7);
        var leave = new Leave();
        switchNodes[7] = new SwitchState(8);
        seqStates[7].childBehaviors = PopulateBranch(stateNodes[7], leave, switchNodes[7]);

        return root;
    }
}