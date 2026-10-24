using UnityEngine;
using UnityEngine.AI;

public class ClockScript : MonoBehaviour
{
    [SerializeField] private Transform _player;
    [SerializeField] private NavMeshAgent _agent;

    private void FixedUpdate()
    {
        _agent.destination = _player.position;
    }
}
