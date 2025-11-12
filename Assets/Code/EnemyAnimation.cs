using UnityEngine;
using System.Collections;

public class EnemyAnimation : MonoBehaviour
{
    public float deadZone = 5f;                  // The number of degrees for which the rotation isn't controlled by Mecanim.
    public float speedDampTime = 0.1f;           // Damping time for the Speed parameter.
    public float angularSpeedDampTime = 0.7f;    // Damping time for the AngularSpeed parameter.
    public float angleResponseTime = 0.6f;       // Response time for turning an angle into angularSpeed.

    private UnityEngine.AI.NavMeshAgent nav;     // Reference to the nav mesh agent.
    private Animator anim;                       // Reference to the Animator.
    public Transform target;                     // Destination of the agent.

    void Awake()
    {
        nav = GetComponent<UnityEngine.AI.NavMeshAgent>();
        anim = GetComponent<Animator>();

        // Making sure the rotation is controlled by Mecanim.
        nav.updateRotation = false;

        // Set the weights for the shooting and gun layers to 1.
        anim.SetLayerWeight(1, 1f);
        anim.SetLayerWeight(2, 1f);

        // Convert the deadZone angle from degrees to radians.
        deadZone *= Mathf.Deg2Rad;
    }

    void Update()
    {
        if (target != null)
        {
            // ✅ TODO #1: Set the destination of the NavMesh agent to the position of the target.
            nav.SetDestination(target.position);

            // Compute animation parameters.
            float speed = 0;
            float angularSpeed = 0;
            DetermineAnimParameters(out speed, out angularSpeed);

            // Pass values to Animator (for Blend Tree).
            anim.SetFloat("Speed", speed, speedDampTime, Time.deltaTime);
            anim.SetFloat("AngularSpeed", angularSpeed, angularSpeedDampTime, Time.deltaTime);
        }
    }

    void OnAnimatorMove()
    {
        // Set the NavMeshAgent's velocity to match animation movement.
        nav.velocity = anim.deltaPosition / Time.deltaTime;

        // The object's rotation follows the animation's rotation.
        transform.rotation = anim.rootRotation;
    }

    void DetermineAnimParameters(out float speed, out float angularSpeed)
    {
        // ✅ TODO #2: Set the speed to the magnitude of the projection of nav.desiredVelocity onto the forward vector.
        speed = Vector3.Dot(transform.forward, nav.desiredVelocity);

        // Compute the turning angle.
        float angle = FindAngle(transform.forward, nav.desiredVelocity, transform.up);

        // ✅ TODO #3: If within deadZone, face the desired direction and set angle to 0.
        if (Mathf.Abs(angle) < deadZone)
        {
            // Smoothly rotate toward movement direction.
            transform.LookAt(transform.position + nav.desiredVelocity);
            angle = 0f;
        }

        angularSpeed = angle / angleResponseTime;
    }

    float FindAngle(Vector3 fromVector, Vector3 toVector, Vector3 upVector)
    {
        float angle = 0;

        if (toVector == Vector3.zero)
            return 0f;

        // ✅ TODO #4: Return the signed angle (in radians) between fromVector and toVector.
        angle = Vector3.Angle(fromVector, toVector) * Mathf.Deg2Rad;

        // Determine the sign of the angle using the cross product.
        Vector3 cross = Vector3.Cross(fromVector, toVector);
        if (Vector3.Dot(cross, upVector) < 0)
            angle = -angle;

        return angle;
    }

    public void setTarget(Transform target, float speed = 2.0f)
    {
        this.target = target;
        nav.speed = speed;
    }

    public void Stop()
    {
        nav.speed = 0f;
    }
}
