using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class TriggerSensor : MonoBehaviour
{
    [Header("Stuff!")]
    public bool isOccupied;
    private float defaultAngularDamping;
    private float defaultLinearDamping;

    public event Action OnSensorActivation;

    private void Awake()
    {
        isOccupied = false;
    }

    private void OnTriggerEnter2D(Collider2D hit)
    {
        if (hit.CompareTag("Weapon"))
        {
            OnSensorActivation?.Invoke(); // Contact TriggerChecker for a change in the array, and to begin checking if any triggers in array are still occupied
            isOccupied = true;
        }
    }

    private void OnTriggerExit2D(Collider2D hit)
    {
       if (isOccupied)
        {
            isOccupied = false;
        }
    }
}
