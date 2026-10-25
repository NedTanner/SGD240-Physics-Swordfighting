using System;
using UnityEngine;

public class TriggerChecker : MonoBehaviour
{
    [Header("Game Manager")]

    [Header("Trigger Babies")]
    public TriggerSensor[] triggers;
    private bool bodyOccupied;

    [Header("Cooldown")]
    [SerializeField] private float maxDamageCooldown;
    private float damageCooldown;

    private void Awake()
    {
        bodyOccupied = false;

        foreach (TriggerSensor trigger in triggers) // More optimised than checking in update, starts only when one of the array triggers reports a change
        {
            trigger.SensorActivation += CheckSensors;
        }
    }

    private void CheckSensors()
    {
        foreach (TriggerSensor trigger in triggers) // Checks every sensor for trigger so that damage code only runs once even if sword crosses over multiple triggers
        {
            if (trigger.isOccupied)
            {
                bodyOccupied = true;

                if (damageCooldown <= 0) // Reduce health code here
                {
                    damageCooldown = maxDamageCooldown;
                }
            }
        }
    }

    private void Update()
    {
        if (damageCooldown > 0)
        {
            damageCooldown -= Time.deltaTime;
        }

        if (bodyOccupied) // Sets damage cooldown to repeatedly fill until sword has fully left body, can't leave your sword in opponent and farm damage
        {
            foreach (TriggerSensor trigger in triggers)
            {
                if (trigger.isOccupied)
                {
                    bodyOccupied = true;
                }

                else
                {
                    bodyOccupied = false;
                    damageCooldown = maxDamageCooldown;
                }
            }
        }
    }
}
