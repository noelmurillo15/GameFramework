/*
 * AudioController -
 * Created by : Allan N. Murillo
 * Last Edited : 7/8/2020
 */

using UnityEngine;
using System.Linq;
using System.Collections;

namespace ANM.Framework.Audio
{
    public class AudioController : MonoBehaviour
    {
        public static AudioController instance;

        [System.Serializable]
        public class AudioTrack
        {
            public AudioSource source;
            public AudioObject[] audioObj;
        }

        [System.Serializable]
        public class AudioObject
        {
            public AudioType type;
            public AudioClip clip;
        }

        public AudioTrack[] tracks;
        public bool debug;

        private enum AudioAction
        {
            Start,
            Stop,
            Restart
        }

        private class AudioJob
        {
            public readonly AudioAction action;
            public readonly AudioType type;
            public readonly bool fade;
            public readonly float delay;

            public AudioJob(AudioAction action, AudioType type, bool fade, float delay)
            {
                this.action = action;
                this.type = type;
                this.fade = fade;
                this.delay = delay;
            }
        }

        private Hashtable _audioTable;
        private Hashtable _jobTable;


        #region Unity Funcs

        private void Awake()
        {
            if (!instance)
                Configure();
        }

        private void OnDisable()
        {
            Dispose();
        }

        #endregion

        #region Public Funcs

        public void PlayAudio(AudioType audioType, bool fade = false, float delay = 0f)
        {
            AddJob(new AudioJob(AudioAction.Start, audioType, fade, delay));
        }

        public void StopAudio(AudioType audioType, bool fade = false, float delay = 0f)
        {
            AddJob(new AudioJob(AudioAction.Stop, audioType, fade, delay));
        }

        public void RestartAudio(AudioType audioType, bool fade = false, float delay = 0f)
        {
            AddJob(new AudioJob(AudioAction.Restart, audioType, fade, delay));
        }

        #endregion

        private void Configure()
        {
            Log("Configure");
            instance = this;
            _jobTable = new Hashtable();
            _audioTable = new Hashtable();
            GenerateAudioTable();
        }

        private void GenerateAudioTable()
        {
            foreach (var audioTrack in tracks)
            {
                foreach (var audioObject in audioTrack.audioObj)
                {
                    if (_audioTable.ContainsKey(audioObject.type))
                    {
                        LogWarning("Audio clip is already registered in hashtable - " + audioObject.type);
                    }
                    else
                    {
                        _audioTable.Add(audioObject.type, audioTrack);
                        Log("Registering audio in hashtable - " + audioObject.type);
                    }
                }
            }
        }

        private void AddJob(AudioJob job)
        {
            RemoveConflictingJobs(job.type);
            IEnumerator jobRunner = RunAudioJob(job);
            if (jobRunner != null)
            {
                _jobTable.Add(job.type, jobRunner);
                StartCoroutine(jobRunner);
            }

            Log("Starting job on - " + job.type + " with operation - " + job.action);
        }

        private void RemoveJob(AudioType audioJobType)
        {
            if (!_jobTable.ContainsKey(audioJobType))
            {
                LogWarning("Trying to stop a job that is not running - " + audioJobType);
                return;
            }

            var runningJob = (IEnumerator) _jobTable[audioJobType];
            StopCoroutine(runningJob);
            _jobTable.Remove(audioJobType);
        }

        private void RemoveConflictingJobs(AudioType audioJobType)
        {
            if (_jobTable.ContainsKey(audioJobType)) RemoveJob(audioJobType);

            var conflictAudio = AudioType.None;
            foreach (DictionaryEntry entry in _jobTable)
            {
                var audioType = (AudioType) entry.Key;
                var audioTrackInUse = (AudioTrack) _audioTable[audioType];
                var audioTrackNeeded = (AudioTrack) _audioTable[audioJobType];
                if (audioTrackNeeded.source == audioTrackInUse.source) conflictAudio = audioType;
            }

            if (conflictAudio == AudioType.None) return;
            RemoveJob(conflictAudio);
        }

        private static AudioClip GetAudioClipFromAudioTrack(AudioType type, AudioTrack track)
        {
            return (from audioObject in track.audioObj where audioObject.type == type select audioObject.clip)
                .FirstOrDefault();
        }

        private IEnumerator RunAudioJob(AudioJob job)
        {
            yield return new WaitForSeconds(job.delay);

            var track = (AudioTrack) _audioTable[job.type];
            track.source.clip = GetAudioClipFromAudioTrack(job.type, track);

            switch (job.action)
            {
                case AudioAction.Start:
                    track.source.Play();
                    break;
                case AudioAction.Stop:
                    if (!job.fade) track.source.Stop();
                    break;
                case AudioAction.Restart:
                    track.source.Stop();
                    track.source.Play();
                    break;
            }

            if (job.fade)
            {
                var initVal = job.action == AudioAction.Start || job.action == AudioAction.Restart ? 0f : 1f;
                var target = initVal == 0f ? 1f : 0f;
                const float duration = 1f;
                var timer = 0f;

                while (timer <= duration)
                {
                    track.source.volume = Mathf.Lerp(initVal, target, timer / duration);
                    timer += Time.deltaTime;
                    yield return null;
                }

                if (job.action == AudioAction.Stop)
                {
                    track.source.Stop();
                }
            }

            _jobTable.Remove(job.type);
            Log("Job Count - " + _jobTable.Count);

            yield return null;
        }

        private void Log(string msg)
        {
            if (!debug) return;
            Debug.Log("[AudioController]: " + msg);
        }

        private void LogWarning(string msg)
        {
            if (!debug) return;
            Debug.LogWarning("[AudioController]: " + msg);
        }

        private void Dispose()
        {
            Log("Dispose");
            foreach (var job in from DictionaryEntry entry in _jobTable select (IEnumerator) entry.Value)
            {
                StopCoroutine(job);
            }
        }
    }
}
