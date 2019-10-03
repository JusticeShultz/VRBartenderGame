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
        var c = new SequenceNode[7];
        for(int i = 0; i < c.Length; i++) { c[i] = new SequenceNode(); }
        var s = new Stage[7];
        var ss = new SwitchState[7];

        //root
        root.childBehaviors = PopulateBranch(c[0], c[1], c[2], c[3], c[4], c[5], c[6]);

        //c0
        s[0] = new Stage(0);
        var a0 = new FindSpot();
        var q0 = new AtBar();
        var a1 = new SitDown();
        ss[0] = new SwitchState(1);
        c[0].childBehaviors = PopulateBranch(s[0], a0, q0, a1, ss[0]);

        //c1
        s[1] = new Stage(1);
        var a2 = new Idle();
        var q1 = new WaitedLongEnough(idleTimes[0]);
        ss[1] = new SwitchState(2);
        c[1].childBehaviors = PopulateBranch(s[1], a2, q1, ss[1]);

        //c2
        s[2] = new Stage(2);
        var a3 = new OrderDrink();
        ss[2] = new SwitchState(3);
        c[2].childBehaviors = PopulateBranch(s[2], a3, ss[2]);

        //c3
        s[3] = new Stage(3);
        var a4 = new Idle();
        var q2 = new WaitedLongEnough(idleTimes[1]);
        ss[3] = new SwitchState(4);
        c[3].childBehaviors = PopulateBranch(s[3], a4, q2, ss[3]);

        //c4
        s[4] = new Stage(4);
        var a5 = new HideOrder();
        ss[4] = new SwitchState(5);
        c[4].childBehaviors = PopulateBranch(s[4], a5, ss[4]);

        //c5
        s[5] = new Stage(5);
        var a6 = new Idle();
        var q3 = new WaitedLongEnough(idleTimes[2]);
        ss[5] = new SwitchState(6);
        c[5].childBehaviors = PopulateBranch(s[5], a6, q3, ss[5]);

        //c6
        s[6] = new Stage(6);
        var a7 = new Leave();
        ss[6] = new SwitchState(7);
        c[6].childBehaviors = PopulateBranch(s[6], a7, ss[6]);

        return root;
    }
}