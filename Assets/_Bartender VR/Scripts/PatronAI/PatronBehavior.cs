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

public class AmThirsty : IBehaviour
{
    public BehaviourResult DoBehaviour(PatronAI patron)
    {
        if (patron.thirst < PatronManager.thirstThreshold)
            return BehaviourResult.Success;
        return BehaviourResult.Failure;
    }
}

public class AtBar : IBehaviour
{
    public BehaviourResult DoBehaviour(PatronAI patron)
    {
        if (patron.atBar)
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
        patron.desiredLocation = pm.openSpots[Random.Range(0, pm.openSpots.Count - 1)];
        return BehaviourResult.Success;
    }
}

public class SitDown : IBehaviour
{
    public BehaviourResult DoBehaviour(PatronAI patron)
    {
        //animation
        return BehaviourResult.Success;
    }
}

public class GoToDestination : IBehaviour
{
    public BehaviourResult DoBehaviour(PatronAI patron)
    {
        //path
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