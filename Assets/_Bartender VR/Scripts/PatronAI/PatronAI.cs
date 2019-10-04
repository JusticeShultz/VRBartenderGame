﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatronAI : MonoBehaviour
{
    public DrinkSystemManager.DrinkNames desiredDrink;
    public PatronManager patronManager;
    public GameObject desiredLocation;
    public float counter;
    public NavMeshAgent agent;
    public int state = 0;
}
