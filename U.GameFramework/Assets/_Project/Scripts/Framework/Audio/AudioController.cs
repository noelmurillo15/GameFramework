/*
 * AudioController -
 * Created by : Allan N. Murillo
 * Last Edited : 6/17/2021
 */

using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace ANM.Framework.Audio
{
    public class AudioController : MonoBehaviour
    {
        #region Data Containers

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

        private enum AudioAction
        {
            Start,
            Stop,
            Restart
        }

        #endregion

        public static AudioController Instance;

        public AudioTrack[] tracks;
        public AudioType currentTrack;
        public bool loopThruTracks;
        public bool debug;

        private Queue<AudioJob> _audioQueue;
        private Hashtable _audioTable;
        private Hashtable _jobTable;


        #region Unity Funcs

        private void Awake()
        {
            if (!Instance) Configure();
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
            Instance = this;
            _jobTable = new Hashtable();
            _audioTable = new Hashtable();
            _audioQueue = new Queue<AudioJob>();
            GenerateAudioTable();
            
            foreach (var audioTrack in GetSoundTracks().OrderBy(n=>Random.value))
                QueueNextTrack(audioTrack.type, true, 1f);
            
            Invoke(nameof(PlayNextTrack), 1f);
        }

        private IEnumerable<AudioObject> GetSoundTracks() => tracks[0].audioObj;
        
        private AudioObject[] GetSoundEffects() => tracks[1].audioObj;
        
        private void QueueNextTrack(AudioType audioType, bool fade = false, float delay = 0f)
        {
            _audioQueue.Enqueue(new AudioJob(AudioAction.Start, audioType, fade, delay));
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
                        //Log("Registering audio in hashtable - " + audioObject.type);
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
            if (job.type != AudioType.Sfx01) SoundTrackActions(job, track);
            else SfxActions(job, track);

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
                    PlayNextTrack();
                }
            }

            _jobTable.Remove(job.type);
            //Log("Job Count - " + _jobTable.Count);
            yield return null;
        }

        private void SoundTrackActions(AudioJob job, AudioTrack track)
        {
            currentTrack = job.type;
            switch (job.action)
            {
                case AudioAction.Start:
                    track.source.Play();
                    Invoke(nameof(PlayNextTrack), track.source.clip.length + job.delay);
                    break;
                case AudioAction.Stop:
                    if (!job.fade)
                    {
                        CancelInvoke();
                        track.source.Stop();
                        PlayNextTrack();
                    }

                    break;
                case AudioAction.Restart:
                    CancelInvoke();
                    track.source.Stop();
                    track.source.Play();
                    Invoke(nameof(PlayNextTrack), track.source.clip.length + job.delay);
                    break;
            }
        }

        private void SfxActions(AudioJob job, AudioTrack track)
        {
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
        }

        private void PlayNextTrack()
        {
            //Log("PlayNextTrack");
            if (loopThruTracks)
            {
                if (_audioQueue.Count > 0)
                {
                    //Log("loading next track in Queue");
                    AddJob(_audioQueue.Dequeue());
                }
                else
                {
                    //Log("picking random background Audio Track");
                    foreach (var key in _audioTable.Keys)
                    {
                        if ((AudioType) key == currentTrack) continue;
                        if ((AudioType) key == AudioType.Sfx01) continue;
                        AddJob(new AudioJob(AudioAction.Start, (AudioType) key, true, 1f));
                        break;
                    }
                }
            }
            else
            {
                if (_audioQueue.Count <= 0) return;
                //Log("loading next track in Queue");
                AddJob(_audioQueue.Dequeue());
            }
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
            CancelInvoke();
            foreach (var job in from DictionaryEntry entry in _jobTable select (IEnumerator) entry.Value)
            {
                StopCoroutine(job);
            }
        }
    }
}