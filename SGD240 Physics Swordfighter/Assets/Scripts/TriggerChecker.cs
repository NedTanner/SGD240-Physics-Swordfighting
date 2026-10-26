using System;
using UnityEngine;

public class TriggerChecker : MonoBehaviour
{
    [Header("Triggers")]
    public TriggerSensor[] triggers;
    private bool bodyOccupied;

    [Header("Cooldown")]
    [SerializeField] private float maxDamageCooldown;
    private float damageCooldown;

    public static event Action<int> OnPlayerDamage;
    private PlayerBase playerBase;

    private void Awake()
    {
        playerBase = GetComponent<PlayerBase>();

        bodyOccupied = false;

        foreach (TriggerSensor trigger in triggers) // More optimised than checking in update, starts only when one of the array triggers reports a change
        {
            trigger.OnSensorActivation += CheckSensors;
        }
    }

    private void CheckSensors()
    {
        foreach (TriggerSensor trigger in triggers) // Checks every sensor for trigger so that damage code only runs once even if sword crosses over multiple triggers
        {
            if (trigger.isOccupied)
            {
                bodyOccupied = true;

                if (damageCooldown <= 0) // Player successfully damaged
                {
                    OnPlayerDamage?.Invoke(playerBase.playerNumber);
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
            bool triggersOccupied = false;

            foreach (TriggerSensor trigger in triggers)
            {
                if (trigger.isOccupied)
                {
                    triggersOccupied = true; // Set and use temp variable to avoid false negatives on 'bodyOccupied'
                    damageCooldown = maxDamageCooldown;
                    break;
                }
            }

            if (triggersOccupied)
            {
                bodyOccupied = true;
            }

            else
            {
                bodyOccupied = false;
            }

        }
    }

    private void OnDestroy()
    {
        foreach (TriggerSensor trigger in triggers) // More optimised than checking in update, starts only when one of the array triggers reports a change
        {
            trigger.OnSensorActivation -= CheckSensors;
        }
    }
}
