using UnityEngine;

public class WeaponReactionManager : MonoBehaviour
{
    [Header("Weapon")]
    [SerializeField] private float modifiedAngularDamping; // Slow weapon when stabbing to simulate resistance
    [SerializeField] private float modifiedLinearDamping;
    private float defaultLinearDamping;
    private float defaultAngularDamping;

    private Rigidbody2D rb;
    private Collider2D weaponCollider;

    [Header("Particles")]
    [SerializeField] private GameObject bloodParticle;
    [SerializeField] private GameObject clashParticle;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        defaultLinearDamping = rb.linearDamping; // Dynamic damping detection, dutifully daring!
        defaultAngularDamping = rb.angularDamping;
    }
    private void OnTriggerEnter2D(Collider2D trigger)
    {
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        rb.linearDamping = modifiedLinearDamping;
        rb.angularDamping = modifiedAngularDamping;

        Vector2 contactPoint = GetComponent<Collider2D>().ClosestPoint(trigger.transform.position); // Find exact location of trigger on collider

        Instantiate(bloodParticle, contactPoint, Quaternion.identity, trigger.transform);
    }

    private void OnTriggerExit2D(Collider2D trigger)
    {
        rb.linearDamping = defaultLinearDamping;
        rb.angularDamping = defaultAngularDamping;
        rb.constraints = RigidbodyConstraints2D.None;
    }

    private void OnCollisionEnter2D(Collision2D collider)
    {
        Vector2 contactPoint = GetComponent<Collider2D>().ClosestPoint(collider.transform.position);

        float weaponSpeed = rb.linearVelocity.magnitude;

        if (weaponSpeed >= 10) // Only plays clash effect when moving weapon fast enough
        {
            Instantiate(clashParticle, contactPoint, Quaternion.identity);
        }
    }
}
