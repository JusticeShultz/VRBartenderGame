﻿using System.Collections;
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

public class DebugNode : IBehaviour
{
    string text;

    public DebugNode(string text)
    {
        this.text = text;
    }

    public BehaviourResult DoBehaviour(PatronAI patron)
    {
        Debug.Log(text);
        return BehaviourResult.Success;
    }
}

//End Special Nodes

//Question Nodes

public class State : IBehaviour
{
    int state;

    public State(int state)
    {
        this.state = state;
    }

    public BehaviourResult DoBehaviour(PatronAI patron)
    {
        if (patron.state == state)
            return BehaviourResult.Success;
        return BehaviourResult.Failure;
    }
}

public class ThereYet : IBehaviour
{
    public BehaviourResult DoBehaviour(PatronAI patron)
    {
        if (Vector3.Distance(patron.transform.position, patron.desiredLocation.transform.position) < 1f)
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

public class WaitedLongEnough : IBehaviour
{
    float amount;

    public WaitedLongEnough(float amount)
    {
        this.amount = amount;
    }

    public BehaviourResult DoBehaviour(PatronAI patron)
    {
        if (patron.counter >= amount)
            return BehaviourResult.Success;
        return BehaviourResult.Failure;
    }
}

//End Question Nodes

//Action Nodes

public class SwitchState : IBehaviour
{
    int state;

    public SwitchState(int state)
    {
        this.state = state;
    }

    public BehaviourResult DoBehaviour(PatronAI patron)
    {
        patron.state = state;
        patron.counter = 0f;
        Debug.Log("State change to " + state);
        return BehaviourResult.Success;
    }
}

public class FindSpot : IBehaviour
{
    public BehaviourResult DoBehaviour(PatronAI patron)
    {
        var pm = patron.patronManager;
        patron.desiredLocation = pm.spots[pm.patrons.FindIndex(i => { return i == patron; })];
        patron.navAgent.SetDestination(patron.desiredLocation.transform.position);
        return BehaviourResult.Success;
    }
}

public class SitDown : IBehaviour
{
    public BehaviourResult DoBehaviour(PatronAI patron)
    {
        patron.navAgent.enabled = false;
        patron.animator.SetBool("Seated", true);
        patron.patronManager.SeatMe(patron);
        return BehaviourResult.Success;
    }
}

public class OrderDrink : IBehaviour
{
    public BehaviourResult DoBehaviour(PatronAI patron)
    {
        var pm = patron.patronManager;
        pm.DrinkRequestIcons[pm.patrons.FindIndex(i => { return i == patron; })].material.SetTexture("_MainTex", pm.dsm.DrinkList[patron.desiredDrink].DrinkImg);
        pm.DrinkRequestIcons[pm.patrons.FindIndex(i => { return i == patron; })].gameObject.SetActive(true);
        Debug.Log("Order");
        return BehaviourResult.Success;
    }
}

public class HideOrder : IBehaviour
{
    public BehaviourResult DoBehaviour(PatronAI patron)
    {
        var pm = patron.patronManager;
        pm.DrinkRequestIcons[pm.patrons.FindIndex(i => { return i == patron; })].gameObject.SetActive(false);
        Debug.Log("Hide");
        return BehaviourResult.Success;
    }
}

public class Drink : IBehaviour
{
    public BehaviourResult DoBehaviour(PatronAI patron)
    {
        patron.animator.SetTrigger("DrinkSomething");
        return BehaviourResult.Success;
    }
}

public class Idle : IBehaviour
{
    public BehaviourResult DoBehaviour(PatronAI patron)
    {
        patron.counter += Time.deltaTime;
        return BehaviourResult.Success;
    }
}

public class Leave : IBehaviour
{
    public BehaviourResult DoBehaviour(PatronAI patron)
    {
        patron.animator.SetBool("Seated", false);
        patron.navAgent.enabled = true;
        patron.desiredLocation = patron.patronManager.exitLocation;
        patron.navAgent.destination = patron.desiredLocation.transform.position;
        return BehaviourResult.Success;
    }
}

public class Die : IBehaviour
{
    public BehaviourResult DoBehaviour(PatronAI patron)
    {
        var pm = patron.patronManager;
        pm.patrons[pm.patrons.FindIndex(i => { return i == patron; })] = null;
        GameObject.Destroy(patron.gameObject);
        PatronAI.Destroy(patron);
        return BehaviourResult.Success;
    }
}

//End Action Nodes