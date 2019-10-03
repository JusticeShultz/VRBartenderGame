using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BehaviourResult //Results
{
    Success,
    Failure
}

public interface IBehaviour //Base Interface
{
    BehaviourResult DoBehaviour(PatronAI patron);
}

//Special Nodes

public class SequenceNode : IBehaviour
{
    public List<IBehaviour> childBehaviors = new List<IBehaviour>();

    public BehaviourResult DoBehaviour(PatronAI patron)
    {
        foreach (IBehaviour p in childBehaviors)
            if (p.DoBehaviour(patron) == BehaviourResult.Failure)
                return BehaviourResult.Failure;
        return BehaviourResult.Success;
    }
}

public class SelectorNode : IBehaviour
{
    public List<IBehaviour> childBehaviors = new List<IBehaviour>();

    public BehaviourResult DoBehaviour(PatronAI patron)
    {
        foreach (IBehaviour p in childBehaviors)
            if (p.DoBehaviour(patron) == BehaviourResult.Success)
                return BehaviourResult.Success;
        return BehaviourResult.Failure;
    }
}

//End Special Nodes

//Question Nodes

public class Stage : IBehaviour
{
    int stage;

    public Stage(int stage)
    {
        this.stage = stage;
    }

    public BehaviourResult DoBehaviour(PatronAI patron)
    {
        if (patron.stage == stage)
            return BehaviourResult.Success;
        return BehaviourResult.Failure;
    }
}

public class AtBar : IBehaviour
{
    public BehaviourResult DoBehaviour(PatronAI patron)
    {
        if (Vector3.Distance(patron.transform.position, patron.desiredLocation.transform.position) < 1f)
            return BehaviourResult.Success;
        return BehaviourResult.Failure;
    }
}

public class OrderedDrink : IBehaviour
{
    public BehaviourResult DoBehaviour(PatronAI patron)
    {
        if (patron.orderedDrink)
            return BehaviourResult.Success;
        return BehaviourResult.Failure;
    }
}

public class GotDrink : IBehaviour
{
    public BehaviourResult DoBehaviour(PatronAI patron)
    {
        if (patron.gotDrink)
            return BehaviourResult.Success;
        return BehaviourResult.Failure;
    }
}

//End Question Nodes

//Action Nodes

public class FindSpot : IBehaviour
{
    public BehaviourResult DoBehaviour(PatronAI patron)
    {
        var pm = patron.patronManager;
        patron.desiredLocation = pm.spots[pm.patrons.FindIndex(i => { return i == patron; })];
        patron.agent.SetDestination(patron.desiredLocation.transform.position);
        return BehaviourResult.Success;
    }
}

public class SitDown : IBehaviour
{
    public BehaviourResult DoBehaviour(PatronAI patron)
    {
        patron.stage = 1;
        //animation
        return BehaviourResult.Success;
    }
}

public class OrderDrink : IBehaviour
{
    public BehaviourResult DoBehaviour(PatronAI patron)
    {
        //order func
        return BehaviourResult.Success;
    }
}

//End Action Nodes