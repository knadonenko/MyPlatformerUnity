using UnityEngine;

namespace Components.GOBased
{
    public class DestroyComponent : MonoBehaviour
    {
        [SerializeField] private GameObject _objectToDestroy;

        public void DestroyObject() {
            Destroy(_objectToDestroy);
        }
    }
}
