/*
 * PlayGenericOneshot - used to play simple quick audio sounds (ie: button click)
 * Created by : Allan N. Murillo
 * Last Edited : 10/26/2022
 */

using FMODUnity;
using UnityEngine;

namespace ANM.Framework.Audio
{
    public class PlayGenericOneshot : MonoBehaviour
    {
        public EventReference soundClip;


        public void PlaySoundEvent()
        {
            RuntimeManager.PlayOneShot(soundClip);
        }
    }
}
