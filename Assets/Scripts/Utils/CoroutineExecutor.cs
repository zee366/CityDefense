using UnityEngine;
using System.Collections;

namespace Utils {
    /// This is a new coroutine interface for Unity.
    ///
    /// The motivation for this is twofold:
    ///
    /// 1. The existing coroutine API provides no means of stopping specific
    ///    coroutines; StopCoroutine only takes a string argument, and it stops
    ///    all coroutines started with that same string; there is no way to stop
    ///    coroutines which were started directly from an enumerator.  This is
    ///    not robust enough and is also probably pretty inefficient.
    ///
    /// 2. StartCoroutine and friends are MonoBehaviour methods.  This means
    ///    that in order to start a coroutine, a user typically must have some
    ///    component reference handy.  There are legitimate cases where such a
    ///    constraint is inconvenient.  This implementation hides that
    ///    constraint from the user.
    ///
    /// Example usage:
    ///
    /// ----------------------------------------------------------------------------
    /// IEnumerator MyAwesomeTask()
    /// {
    ///     while(true) {
    ///         Debug.Log("PRINTING RUBBISH!");
    ///         yield return null;
    ///    }
    /// }
    ///
    /// IEnumerator TaskKiller(float delay, Task t)
    /// {
    ///     yield return new WaitForSeconds(delay);
    ///     t.Stop();
    /// }
    ///
    /// void SomeCodeThatCouldBeAnywhereInTheUniverse()
    /// {
    ///     Task spam = new Task(MyAwesomeTask());
    ///     new Task(TaskKiller(5, spam));
    /// }
    /// ----------------------------------------------------------------------------
    ///
    /// When SomeCodeThatCouldBeAnywhereInTheUniverse is called, the debug console
    /// will be spammed with annoying messages for 5 seconds.
    ///
    /// Simple, really.  There is no need to initialize or even refer to TaskManager.
    /// When the first Task is created in an application, a "TaskManager" GameObject
    /// will automatically be added to the scene root with the TaskManager component
    /// attached.  This component will be responsible for dispatching all coroutines
    /// behind the scenes.
    ///
    /// Task also provides an event that is triggered when the coroutine exits.
    /// A Task object represents a coroutine.  Tasks can be started, paused, and stopped.
    /// It is an error to attempt to start a task that has been stopped or which has
    /// naturally terminated.
    public class CoroutineTask {

        private readonly CoroutineExecutor.TaskState _task;

        /// Returns true if and only if the coroutine is running.  Paused tasks
        /// are considered to be running.
        public bool Running {
            get { return _task.Running; }
        }

        /// Returns true if and only if the coroutine is currently paused.
        public bool Paused {
            get { return _task.Paused; }
        }

        /// Delegate for termination subscribers.  manual is true if and only if
        /// the coroutine was stopped with an explicit call to Stop().
        public delegate void FinishedHandler(bool manual);

        /// Termination event.  Triggered when the coroutine completes execution.
        public event FinishedHandler Finished;


        /// Creates a new Task object for the given coroutine.
        ///
        /// If autoStart is true (default) the task is automatically started
        /// upon construction.
        public CoroutineTask(IEnumerator c, bool autoStart = true) {
            _task          =  CoroutineExecutor.CreateTask(c);
            _task.Finished += TaskFinished;
            if ( autoStart )
                Start();
        }


        /// Begins execution of the coroutine
        public void Start() { _task.Start(); }


        /// Discontinues execution of the coroutine at its next yield.
        public void Stop() { _task.Stop(); }


        public void Pause() { _task.Pause(); }

        public void Unpause() { _task.Unpause(); }


        void TaskFinished(bool manual) {
            FinishedHandler handler = Finished;
            if ( handler != null )
                handler(manual);
        }

    }

    class CoroutineExecutor : MonoBehaviour {

        public class TaskState {

            public bool Running {
                get { return _running; }
            }

            public bool Paused {
                get { return _paused; }
            }

            public delegate void FinishedHandler(bool manual);

            public event FinishedHandler Finished;

            IEnumerator _coroutine;
            bool        _running;
            bool        _paused;
            bool        _stopped;

            public TaskState(IEnumerator c) { _coroutine = c; }

            public void Pause() { _paused = true; }

            public void Unpause() { _paused = false; }


            public void Start() {
                _running = true;
                _singleton.StartCoroutine(CallWrapper());
            }


            public void Stop() {
                _stopped = true;
                _running = false;
            }


            IEnumerator CallWrapper() {
                yield return null;

                IEnumerator e = _coroutine;
                while ( _running ) {
                    if ( _paused )
                        yield return null;
                    else {
                        if ( e != null && e.MoveNext() ) {
                            yield return e.Current;
                        } else {
                            _running = false;
                        }
                    }
                }

                FinishedHandler handler = Finished;
                if ( handler != null )
                    handler(_stopped);
            }

        }

        static CoroutineExecutor _singleton;


        public static TaskState CreateTask(IEnumerator coroutine) {
            if ( _singleton == null ) {
                GameObject go = new GameObject("CoroutineManager");
                _singleton = go.AddComponent<CoroutineExecutor>();
            }

            return new TaskState(coroutine);
        }

    }
}