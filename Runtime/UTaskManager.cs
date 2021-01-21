using UnityEngine;
using System.Collections.Generic;

namespace Lab5Games
{
    class UTaskManager : Singleton<UTaskManager>
    {
        private float _dt;

        private List<UTask> _uTasks = new List<UTask>();
        private List<UTimerTask> _uTimers = new List<UTimerTask>();
        private List<UTimerTask> _removedTimers = new List<UTimerTask>();

        internal static void RunTask(UTask newTask)
        {
            Instance.StartCoroutine(newTask.Routine());

            if(!Instance._uTasks.Contains(newTask))
            {
                Instance._uTasks.Add(newTask);
            }
        }

        internal static void RemoveTask(UTask uTask)
        {
            Instance._uTasks.Remove(uTask);
        }

        internal static void RunTask(UTimerTask newTask)
        {
            if(!Instance._uTimers.Contains(newTask))
            {
                Instance._uTimers.Add(newTask);
            }
        }

        internal static void RemoveTask(UTimerTask timerTask)
        {
            Instance._removedTimers.Add(timerTask);
        }

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        private void Update()
        {
            _dt = Time.deltaTime;

            for(int i=0; i<_uTimers.Count; i++)
            {
                _uTimers[i].Tick(_dt);
            }

            while(_removedTimers.Count > 0)
            {
                _uTimers.Remove(_removedTimers[0]);
                _removedTimers.RemoveAt(0);
            }
        }
    }
}
