using UnityEngine;

namespace Components
{
    public class TeleportComponent : MonoBehaviour
    {
        [SerializeField] private Transform _destTransform;

        public void Teleport(GameObject target)
        {
            Debug.Log("TELEPORT!!!");
            target.transform.position = _destTransform.position;
        }
    }
}