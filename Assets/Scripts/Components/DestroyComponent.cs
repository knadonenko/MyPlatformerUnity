using UnityEngine;

namespace Components
{
    public class DestroyComponent : MonoBehaviour
    {
        [SerializeField] private GameObject _objectToDestroy;

        public void DestroyObject() {
            Destroy(_objectToDestroy);
        }
    }
}
