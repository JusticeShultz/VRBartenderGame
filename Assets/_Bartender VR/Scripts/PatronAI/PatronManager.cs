using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatronManager : MonoBehaviour
{
    [Header("Patron Options")]
    [SerializeField] int minPatrons;
    [SerializeField] int maxPatrons;
    [SerializeField] [Range(0f, 1f)] float patronGenerationChance;
    [SerializeField] float timeBetweenGeneration;
    
    public static float thirstThreshold;

    [Header("Required Fields")]
    [SerializeField] List<GameObject> spots;
    [SerializeField] GameObject spawnLocation;
    [SerializeField] DrinkSystemManager drinkSystemManager;

    [Header("Generated Fields")]
    [SerializeField] List<PatronAI> patrons;

    float currTime;
    IBehaviour root;

    public List<GameObject> openSpots;

    void Start()
    {
        if (spots == null) { Debug.LogError("Spots cannot be empty"); }
        if (spawnLocation == null) { Debug.LogError("Spawn Location cannot be empty"); }
        if (patrons == null) { patrons = new List<PatronAI>(); }
        openSpots = new List<GameObject>(spots.Count);
        foreach (GameObject s in spots) openSpots.Add(s);
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
    }

    PatronAI GeneratePatron()
    {
        GameObject body = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        body.transform.position = spawnLocation.transform.position;
        PatronAI p = body.AddComponent<PatronAI>();
        p.patronManager = this;
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


        return root;
    }
}