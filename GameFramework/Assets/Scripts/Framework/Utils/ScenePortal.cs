/*
 * ScenePortal - Used to transition between rooms
 * Created by : Allan N. Murillo
 * Last Edited : 2/22/2020
 */

using UnityEngine;
using System.Linq;
using System.Collections;
using ANM.Framework.Extensions;

namespace ANM.Framework.Utils
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class ScenePortal : MonoBehaviour
    {
        [SerializeField] private int sceneToLoad = -1;
        private const string PlayerTag = "Player";

        private void OnTriggerEnter2D(Collider2D other)
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
            var buildIndex = SceneExtension.GetCurrentSceneBuildIndex();
            //    TODO : Disable Player Movement / Input
            
            yield return SceneExtension.LoadMultiSceneWithBuildIndexSequence(sceneToLoad, true);
            
            //    TODO : Disable Player Movement / Input... again (new scene just loaded)
                //    Update player spawn position and re-enable Player Movement / Input

            Destroy(gameObject);
        }
        
        private static void UpdatePlayerSpawnPosition(ScenePortal otherPortal, GameObject player)
        {
            var spawnPosition = otherPortal.transform.GetChild(0).position;
            player.transform.position = spawnPosition;
            player.transform.rotation = Quaternion.identity;
        }

        private ScenePortal GetOtherScenePortal(int otherBuildIndex = -1)
        {
            if(otherBuildIndex == -1)
                return FindObjectsOfType<ScenePortal>().FirstOrDefault(portal => portal != this);
            
            otherBuildIndex -= 1;
            return FindObjectsOfType<ScenePortal>().FirstOrDefault(portal => 
                portal.name.Contains(otherBuildIndex.ToString()) && portal != this);
        }
    }
}
