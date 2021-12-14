/*
 * ButtonSounds -
 * Created by : Allan N. Murillo
 * Last Edited : 7/8/2020
 */

using UnityEngine;

namespace ANM.Framework.Audio
{
    public class ButtonSounds : MonoBehaviour
    {
        private float _delay;


        private void Start()
        {
            _delay = 0f;
        }

        public void PlayButtonPressedSound()
        {
            if (!(Time.timeSinceLevelLoad > _delay)) return;
            AudioController.instance?.PlayAudio(AudioType.Sfx01);
            _delay = Time.timeSinceLevelLoad + 0.3f;
        }
    }
}
