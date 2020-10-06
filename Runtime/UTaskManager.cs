using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lab5Games
{
    public class UTaskManager : MonoBehaviour
    {


        private static UTaskManager _instance = null;

        public static UTaskManager instance
        {
            get
            {
                if(_instance == null)
                {
                    _instance = FindObjectOfType<UTaskManager>();
                }

                if(_instance == null)
                {
                    GameObject go = new GameObject("UTaskManager");

                    _instance = go.AddComponent<UTaskManager>();
                }

                return _instance;
            }
        }

        public static void RunTask(UTask task)
        {
            if(task.state != UTask.EState.Running)
            {
                Debug.LogWarning("UTaskManager: Failed to run the task, state= " + task.state);
                return;
            }

            instance.AddTask(task);
        }

        private void AddTask(UTask task)
        {
            
            StartCoroutine(task.Routine());
        }

        private void Awake()
        {
            _instance = this;

            DontDestroyOnLoad(gameObject);
        }

        private void OnDestroy()
        {
            _instance = null;
        }
    }
}
