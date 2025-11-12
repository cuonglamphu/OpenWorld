using UnityEngine;
using UnityEngine.AI;

public class ClickToMove : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator anim;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        // Di chuyển bằng chuột
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                agent.SetDestination(hit.point);
            }
        }

        // Cập nhật animation qua InputMagnitude
        if (anim)
        {
            float moveAmount = agent.velocity.magnitude / agent.speed;
            anim.SetFloat("InputMagnitude", moveAmount); // 👈 Thay vì "Speed"
        }
    }
}
