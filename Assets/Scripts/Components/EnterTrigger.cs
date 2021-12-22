using UnityEngine;
using UnityEngine.Events;

namespace Components
{
    public class EnterTrigger : MonoBehaviour
    {
        [SerializeField] private string _tag;
        [SerializeField] private UnityEvent _action;
        
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.CompareTag(_tag))
            {
                _action?.Invoke();
            }
        }
    }
}