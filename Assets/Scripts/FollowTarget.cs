using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private float _damping;

    private void LateUpdate() 
    {
        var dest = new Vector3(_target.position.x, _target.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, dest, Time.deltaTime * _damping);
    }
    
}
