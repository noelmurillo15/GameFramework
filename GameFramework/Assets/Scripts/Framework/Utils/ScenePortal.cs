/*
 * ScenePortal - 
 * Created by : Allan N. Murillo
 * Last Edited : 2/26/2020
 */

using UnityEngine;
using System.Linq;
using System.Collections;

namespace ANM.Framework.Utils
{
    [RequireComponent(typeof(BoxCollider))]
    public class ScenePortal : MonoBehaviour
    {
        [SerializeField] private int sceneToLoad = -1;
        private const string PlayerTag = "Player";

        
        private void OnTriggerEnter(Collider other)
        {
            if (other.tag.Equals(PlayerTag))
            {
                StartCoroutine(Transition());
            }
        }

        private IEnumerator Transition()
        {
            if (sceneToLoad < 0)  {  yield break;  }
            DontDestroyOnLoad(gameObject);
            
            //    TODO : Scene Transition - get current scene build index, Transition to new scene
            //    Find new player controller, UpdatePlayerSpawnPosition, enable controller & agent
            
            //UpdatePlayerSpawnPosition(GetOtherScenePortal(otherBuildIndex), player.gameObject);

            yield return null;
            Destroy(gameObject);
        }
        
        private static void UpdatePlayerSpawnPosition(ScenePortal otherPortal, GameObject player)
        {
            var spawnPosition = otherPortal.transform.GetChild(0).position;
            player.transform.localPosition = spawnPosition;
            player.transform.rotation = Quaternion.identity;
        }

        private ScenePortal GetOtherScenePortal(int otherBuildIndex = -1)
        {
            if(otherBuildIndex == -1)
                return FindObjectsOfType<ScenePortal>().FirstOrDefault(portal => portal != this);
            
            return FindObjectsOfType<ScenePortal>().FirstOrDefault(portal => 
                portal.name.Contains(otherBuildIndex.ToString()) && portal != this);
        }
    }
}
