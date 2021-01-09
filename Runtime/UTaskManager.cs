using UnityEngine;

namespace Lab5Games
{
    class UTaskManager : Singleton<UTaskManager>
    {
        internal static void RunTask(UTask task)
        {
            if(task.state != UTask.EState.Running)
            {
                Debug.LogWarning("UTaskManager: Failed to run the task, state= " + task.state);
                return;
            }

            Instance.CreateTask(task);
        }

        private void CreateTask(UTask task)
        {
            StartCoroutine(task.Routine());
        }

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
