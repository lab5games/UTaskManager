using System;

namespace Lab5Games.Task
{
    public class UTimer
    {
        public static UTimer CreateTask(Action<UTimer> action, float timer, bool autoStart = true)
        {
            UTimer newTask =  new UTimer(action, timer);

            if(autoStart)
            {
                newTask.Start();
            }

            return newTask;
        }

        private Action<UTimer> _action;
        private float _timer;

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
