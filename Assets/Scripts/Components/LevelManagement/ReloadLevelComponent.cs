using DefaultNamespace.Model;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Components.LevelManagement
{
    public class ReloadLevelComponent : MonoBehaviour
    {
        public void Reload()
        {
            var session = FindObjectOfType<GameSession>();
            DestroyImmediate(session);
            
            var scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }
    }
}
