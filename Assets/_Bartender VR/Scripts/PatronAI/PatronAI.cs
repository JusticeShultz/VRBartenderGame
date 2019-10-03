using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatronAI : MonoBehaviour
{
    public DrinkSystemManager.DrinkNames desiredDrink;
    public PatronManager patronManager;
    public GameObject desiredLocation;
    public bool orderedDrink, gotDrink;
    public float speed, counter;
    public NavMeshAgent agent;
    public int stage = 0;
}
