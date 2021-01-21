using UnityEngine;
using System.Collections.Generic;

namespace Lab5Games
{
    class UTaskManager : Singleton<UTaskManager>
    {
        private float _dt;

        private List<UTask> _uTasks = new List<UTask>();
        private List<UTimer> _uTimers = new List<UTimer>();
        private List<UTimer> _removedTimers = new List<UTimer>();

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

        internal static void RunTask(UTimer timer)
        {
            if(!Instance._uTimers.Contains(timer))
            {
                Instance._uTimers.Add(timer);
            }
        }

        internal static void RemoveTask(UTimer timer)
        {
            Instance._removedTimers.Add(timer);
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
