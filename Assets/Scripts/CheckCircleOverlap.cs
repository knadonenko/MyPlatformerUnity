using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Utils;

namespace DefaultNamespace
{
    public class CheckCircleOverlap : MonoBehaviour
    {
        [SerializeField] private float _radius = 1f;
        // [SerializeField] private string _tag;
        [SerializeField] private Collider2D[] _interactionResults = new Collider2D[5];
        
        public GameObject[] GetObjectsInRange()
        {
            var size = Physics2D.OverlapCircleNonAlloc(
                transform.position,
                _radius,
                _interactionResults);

            var overlaps = new List<GameObject>();
            for (var i = 0; i < size; i++)
            {
                overlaps.Add(_interactionResults[i].gameObject);
            }

            return overlaps.ToArray();
        }

        private void OnDrawGizmosSelected()
        {
            Handles.color = HandlesUtils.TransparentRed;
            Handles.DrawSolidDisc(transform.position, Vector3.forward, _radius);
        }
    }
}