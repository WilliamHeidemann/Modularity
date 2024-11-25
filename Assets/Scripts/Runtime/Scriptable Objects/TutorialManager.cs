using UnityEngine;
using System;
using System.Collections.Generic;

namespace Runtime.Scriptable_Objects
{
    [CreateAssetMenu]
    public class TutorialManager : ScriptableObject
    {
        // [SerializeField] private List<Quest> _tutorialTasks = new();
        // private Quest _currentQuest;
        //
        // public event Action<string, string> OnTaskCompleted; // Task name, Task description - Event should be assigned within the UI
        //
        // public void CompleteTask(string taskName)
        // {
        //     Quest quest = _tutorialTasks.Find(t => t.Name == taskName);
        //     if (quest != null)
        //     {
        //         quest.IsCompleted = true;
        //     }
        //
        //     if(_tutorialTasks.TrueForAll(t => t.IsCompleted))
        //     {
        //         OnTaskCompleted?.Invoke("Tutorial Completed", "You have completed the tutorial!");
        //     }
        //     else
        //     {
        //         SetNextTask();
        //     }
        // }
        //
        // public void SetNextTask()
        // {
        //     // Find the next task
        //     _currentQuest = _tutorialTasks.Find(t => !t.IsCompleted);
        //     if (_currentQuest != null)
        //     {
        //         OnTaskCompleted?.Invoke(_currentQuest.Name, _currentQuest.Description);
        //     }
        // }
    }
}
