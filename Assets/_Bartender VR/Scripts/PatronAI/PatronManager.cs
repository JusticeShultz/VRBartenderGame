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
    
    public static float thirstThreshold;

    [Header("Required Fields")]
    [SerializeField] public List<GameObject> spots;
    [SerializeField] GameObject spawnLocation;

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
        p.speed = patronSpeed;
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

        //root
        var c0 = new SequenceNode();
        var c1 = new SequenceNode();
        root.childBehaviors = PopulateBranch(c0, c1);

        //c0
        var stage0 = new Stage(0);
        var a0 = new FindSpot();
        var q0 = new AtBar();
        var a1 = new SitDown();
        c0.childBehaviors = PopulateBranch(stage0, a0, q0, a1);

        //c1
        var stage1 = new Stage(1);

        return root;
    }
}