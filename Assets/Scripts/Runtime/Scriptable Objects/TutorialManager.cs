using UnityEngine;
using System;
using System.Collections.Generic;

namespace Runtime.Scriptable_Objects
{
    [CreateAssetMenu]
    public class TutorialManager : ScriptableObject
    {
        [SerializeField] private List<Task> _tutorialTasks = new();
        private Task _currentTask;

        public event Action<string, string> OnTaskCompleted; // Task name, Task description - Event should be assigned within the UI

        public void CompleteTask(string taskName)
        {
            Task task = _tutorialTasks.Find(t => t.Name == taskName);
            if (task != null)
            {
                task.IsCompleted = true;
            }

            if(_tutorialTasks.TrueForAll(t => t.IsCompleted))
            {
                OnTaskCompleted?.Invoke("Tutorial Completed", "You have completed the tutorial!");
            }
            else
            {
                SetNextTask();
            }
        }

        public void SetNextTask()
        {
            // Find the next task
            _currentTask = _tutorialTasks.Find(t => !t.IsCompleted);
            if (_currentTask != null)
            {
                OnTaskCompleted?.Invoke(_currentTask.Name, _currentTask.Description);
            }
        }
    }
}
