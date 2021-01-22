using UnityEngine;
using System.Collections.Generic;

namespace Lab5Games
{
    class UTaskManager : Singleton<UTaskManager>
    {
        private float _dt;

        private List<UTask> _uTasks = new List<UTask>(32);
        private List<UTimer> _uTimers = new List<UTimer>(64);

        public int UTaskCount => _uTasks.Count;
        public int UTimerCount => _uTimers.Count;

        internal static void RunTask(UTask newTask)
        {
            Instance.StartCoroutine(newTask.Routine());

            if (!Instance._uTasks.Contains(newTask))
            {
                Instance._uTasks.Add(newTask);
            }
        }

        internal static void RunTask(UTimer timer)
        {
            if(!Instance._uTimers.Contains(timer))
            {
                Instance._uTimers.Add(timer);
            }
        }
        
        public static void Cleanup()
        {
            Instance.StopAllCoroutines();

            Instance._uTasks.Clear();
            Instance._uTimers.Clear();
        }

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        private void OnEnable()
        {
            Cleanup();
        }

        private void Update()
        {
            _dt = Time.deltaTime;

            for(int i=_uTasks.Count-1; i>=0; i--)
            {
                if(_uTasks[i].State == ETaskState.Finished ||
                    _uTasks[i].State == ETaskState.Stopped)
                {
                    _uTasks.RemoveAt(i);
                }
            }

            for(int i=_uTimers.Count-1; i>=0; i--)
            {
                _uTimers[i].Tick(_dt);

                if(_uTimers[i].State == ETaskState.Finished || 
                    _uTimers[i].State == ETaskState.Stopped)
                {
                    _uTimers.RemoveAt(i);
                }
            }
        }
    }
}
