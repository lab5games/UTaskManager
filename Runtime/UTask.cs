using System;
using System.Collections;

namespace Lab5Games
{
    public enum ETaskState
    {
        Ready,
        Stopped,
        Finished,
        Running,
        Paused
    }

    public class UTask
    {
        public static UTask CreateTask(IEnumerator task, Action<UTask> action = null, bool autoStart = true)
        {
            UTask newTask = new UTask(task, action);

            if(autoStart)
            {
                newTask.Start();
            }

            return newTask;
        }

        private IEnumerator _task;
        private Action<UTask> _action;

        private ETaskState _state = ETaskState.Ready;

        public ETaskState State
        {
            get { return _state; }

            private set
            {
                _state = value;

                if (_state == ETaskState.Finished || _state == ETaskState.Stopped)
                {
                    _action?.Invoke(this);
                }
            }
        }

        public void Start()
        {
            if (State == ETaskState.Ready)
            {
                State = ETaskState.Running;

                UTaskManager.RunTask(this);
            }
        }

        public void Stop()
        {
            if (State == ETaskState.Running)
            {
                State = ETaskState.Stopped;
            }
        }

        public void Pause()
        {
            if (State == ETaskState.Running)
            {
                State = ETaskState.Paused;
            }
        }

        public void UnPause()
        {
            if (State == ETaskState.Paused)
            {
                State = ETaskState.Running;
            }
        }

        private UTask(IEnumerator task, Action<UTask> action)
        {
            _task = task;
            _action = action;
        }

        internal IEnumerator Routine()
        {
            while(State > ETaskState.Finished)
            {
                if(State == ETaskState.Paused)
                {
                    yield return Yielders.EndOfFrame;
                }
                else
                {
                    if(_task != null && _task.MoveNext())
                    {
                        yield return _task.Current;
                    }
                    else
                    {
                        State = ETaskState.Finished;
                    }
                }
            }
        }
    }
}
