/*
 * ScenePortal -
 * Created by : Allan N. Murillo
 * Last Edited : 5/8/2020
 */

using UnityEngine;
using System.Linq;
using System.Collections;
using ANM.Framework.Extensions;
using ANM.Input;
using UnityEngine.Experimental.Rendering.Universal;

namespace ANM.Framework.Utils
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class ScenePortal : MonoBehaviour
    {
        [SerializeField] private int sceneToLoad = -1;
        [SerializeField] private Vector3 spawnPoint;
        private const string PlayerTag = "Player";


        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.transform.root.CompareTag(PlayerTag))
            {
                StartCoroutine(Transition());
            }
        }

        private IEnumerator Transition()
        {
            if (sceneToLoad < 0)
            {
                yield break;
            }

            DontDestroyOnLoad(gameObject);

            var buildIndex = SceneExtension.GetCurrentSceneBuildIndex();
            //FindObjectOfType<IsometricMovementMouseClick>().enabled = false;
            yield return SceneExtension.LoadMultiSceneWithBuildIndexSequence(sceneToLoad, true);

            //var player = FindObjectOfType<IsometricMovementMouseClick>();
            //var cam = FindObjectOfType<PixelPerfectCamera>();
            //player.enabled = false;
            //var otherPortal = GetOtherScenePortal(buildIndex);
            //UpdatePlayerSpawnPosition(otherPortal, player.gameObject, cam.gameObject);
            //player.enabled = true;

            Destroy(gameObject);
        }

        private static void UpdatePlayerSpawnPosition(ScenePortal otherPortal, GameObject player, GameObject cam)
        {
            var spawnPosition = otherPortal.spawnPoint;
            player.transform.position = spawnPosition;
            cam.transform.position = spawnPosition.With(z: -5);
            player.transform.rotation = Quaternion.identity;
        }

        private ScenePortal GetOtherScenePortal(int otherBuildIndex = -1)
        {
            if (otherBuildIndex == -1)
                return FindObjectsOfType<ScenePortal>().FirstOrDefault(portal => portal != this);

            otherBuildIndex -= 1;
            return FindObjectsOfType<ScenePortal>().FirstOrDefault(portal =>
                portal.name.Contains(otherBuildIndex.ToString()) && portal != this);
        }
    }
}
