using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "TimeTravel/SpherePlace")]
public class SpherePlace : ScriptableObject
{
    public Vector3 position;
    public bool initialized = false;
}
