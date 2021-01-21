using System;

namespace Lab5Games
{
    public class UTimer
    {
        public static UTimer CreateTask(Action<UTimer> action, float timer, bool autoStart = true, bool autoRemove = true)
        {
            UTimer newTask =  new UTimer(action, timer);
            newTask.autoRemove = autoRemove;
            
            if(autoStart)
            {
                newTask.Start();
            }

            return newTask;
        }

        public static void Remove(UTimer timer)
        {
            UTaskManager.RemoveTask(timer);
        }

        private Action<UTimer> _action;
        private float _timer;

        public bool autoRemove = false;

        private ETaskState _state = ETaskState.Ready;

        public ETaskState State
        {
            get { return _state; }

            private set
            {
                _state = value;

                if(_state == ETaskState.Finished || _state == ETaskState.Stopped)
                {
                    _action.Invoke(this);

                    if (autoRemove)
                    {
                        Remove(this);
                    }
                    else
                    {
                        _state = ETaskState.Ready;
                    }
                }
            }
        }
        
        public void Start()
        {
            if(State == ETaskState.Ready)
            {
                State = ETaskState.Running;

                UTaskManager.RunTask(this);
            }
        }

        public void Stop()
        {
            if(State == ETaskState.Running)
            {
                State = ETaskState.Stopped;
            }
        }

        public void Pause()
        {
            if(State == ETaskState.Running)
            {
                State = ETaskState.Paused;
            }
        }

        public void UnPause()
        {
            if(State == ETaskState.Paused)
            {
                State = ETaskState.Running;
            }
        }

        private UTimer(Action<UTimer> action, float timer)
        {
            _action = action;
            _timer = timer;
        }

        internal void Tick(float deltaTime)
        {
            if(State == ETaskState.Running)
            {
                _timer -= deltaTime;

                if(_timer < 0)
                {
                    State = ETaskState.Finished;
                }
            }
        }
    }
}
