using System;
using UnityEngine;
using UnityEngine.Events;
using Utils;

namespace Components.ColliderBased
{
    public class EnterTrigger : MonoBehaviour
    {
        [SerializeField] private string _tag;
        [SerializeField] private LayerMask _layer = ~0;
        [SerializeField] private EnterEvent _action;
        
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (!col.gameObject.IsInLayer(_layer)) return;
            if (!string.IsNullOrEmpty(_tag) && !col.gameObject.CompareTag(_tag)) return;

            _action?.Invoke(col.gameObject);
        }
        
        [Serializable]
        public class EnterEvent : UnityEvent<GameObject>
        {
        }
    }
}