using System;
using System.Collections;

namespace Lab5Games
{
    public class UTask
    {
        public enum EState
        {
            Ready,
            Stopped,
            Finished,
            Running,
            Paused
        }


        private IEnumerator _task;

        private EState _state = EState.Ready;
        
        public EState state
        {
            get { return _state; }

            set
            {
                _state = value;

                if(state == EState.Finished)
                {
                    onFinish?.Invoke(this);

                    _state = EState.Ready;
                }

                if(state == EState.Stopped)
                {
                    onStop?.Invoke(this);

                    _state = EState.Ready;
                }
            }
        }


        public event Action<UTask> onStop;
        public event Action<UTask> onFinish;


        public static UTask CreateTask(IEnumerator task, bool autoStart = true)
        {
            UTask uTask = new UTask(task);

            if (autoStart)
                uTask.Start();

            return uTask;
        }

        public UTask(IEnumerator task)
        {
            if (_task == null)
                throw new ArgumentNullException(nameof(task));

            _task = task;

            state = EState.Ready;
        }

        public void Start()
        {
            if(state == EState.Ready)
            {
                state = EState.Running;

                UTaskManager.RunTask(this);
            }
        }

        public void Stop()
        {
            if(state == EState.Running)
            {
                state = EState.Stopped;
            }
        }

        public void Pause()
        {
            if(state == EState.Running)
            {
                state = EState.Paused;
            }
        }

        public void UnPause()
        {
            if(state == EState.Paused)
            {
                state = EState.Running;
            }
        }

        internal IEnumerator Routine()
        {
            while(state > EState.Finished)
            {
                if(state == EState.Paused)
                {
                    yield return Yielders.EndOfFrame;
                }
                else
                {
                    if (_task != null && _task.MoveNext())
                    {
                        yield return _task.Current;
                    }
                    else
                    {
                        state = EState.Finished;
                    }
                }
            }
        }
    }
}
