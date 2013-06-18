using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exchange101;
using Microsoft.Exchange.WebServices.Data;
using System.Collections.ObjectModel;


namespace TaskData101
{
    public class TaskDataList : ObservableCollection<TaskData>
    {
        public TaskDataList()
            : base()
        {

        }
    }

    public class TaskData : IComparable<TaskData>
    { 
        public string Subject { get; set; }
        public string Category { get; set; }
        public string Body { get; set; }
        public Boolean IsComplete { get; set; }

        // Implement the generic CompareTo method with the Task 
        // class as the Type parameter. 
        //
        public int CompareTo(TaskData other)
        {
            // If other is not a valid object reference, this instance is greater.
            if (other == null) return 1;

            // The temperature comparison depends on the comparison of 
            // the underlying Double values. 
            return Subject.CompareTo(other.Subject);
        }
    }

    public class Tasks
    {
        private FindItemsResults<Item> _tasks = null;

        private string _email;
        private string _password;

        public Tasks(string email, string password)
        {
            _email = email;
            _password = password;
            _tasks = Ex15_FindTaskBySubject_CS.RetrieveTasks(email, password);
        }

        public Tasks()
        {
            _tasks = Ex15_FindTaskBySubject_CS.RetrieveTasks(_email, _password);
        }

        public void SyncTasks()
        {
            _tasks = Ex15_FindTaskBySubject_CS.RetrieveTasks(_email, _password);
        }

        public TaskData FindTasksBySubject(string Subject)
        {
            /*
             * Retrieve all tasks
             * Find the task with the proper subject
             * Return task
             */

            TaskData foundTask = null;

            foreach (Task t in _tasks)
            {
                if (t.Subject == Subject)
                {
                    foundTask = CreateTask(t);
                    break;
                }
            }
            return foundTask;
        }

        public TaskDataList GetTasksByCategory(string category)
        {
            /*
             * Create task list
             * Retrieve all tasks
             * for each task in the list
             *      check whether it has the proper category
             *      [YES]
             *          Add task to task list
             *      [NO]
             *          Do nothing
             * return task list
             */

            TaskDataList taskList = new TaskDataList();

            foreach (Task t in _tasks)
            {
                string taskCategory = GetCategory(t);
                if (taskCategory == category)
                {
                    TaskData foundTask = CreateTask(t);
                    taskList.Add(foundTask);
                }
            }

            return taskList;
        }

        public TaskDataList GetNonCompletedTasks()
        {
            /*
             * Create task list
             * Retrieve all tasks
             * for each task in the list
             *      check whether it is completed
             *      [NO]
             *          Add task to task list
             *      [YES]
             *          Do nothing
             * return task list
             */

            TaskDataList taskList = new TaskDataList();

            foreach (Task t in _tasks)
            {
                if (!t.IsComplete)
                {
                    TaskData foundTask = CreateTask(t);
                    taskList.Add(foundTask);
                }
            }

            return taskList;
        }

        public SortedSet<string> RetrieveDistinctTaskCategories()
        {
            /*
             * Create category list
             * Retrieve all tasks
             * For each task in the list do
             *      Check if task category exist in category list, when task doesn't have a category make category "No Category"
             *      <NO>
             *          Add category to list only when there is at least one not completed task for that category
             *      <YES>
             *          Do nothing
             * return category list
             */

            SortedSet<string> categoryList = new SortedSet<string>();

            foreach (Task t in _tasks)
            {
                string category = GetCategory(t);
                if (!t.IsComplete && !categoryList.Contains(category))
                {
                    categoryList.Add(category);
                }
            }
            return categoryList;

        }

        private TaskData CreateTask(Task t)
        {
            TaskData task = new TaskData();
            task.Subject = t.Subject;
            task.Category = GetCategory(t);
            task.Body = t.Body;
            task.IsComplete = t.IsComplete;

            return task;
        }

        private string GetCategory(Task t)
        {
            string category = "";
            if (t.Categories.Count > 0)
            {
                category = t.Categories[0];
            }
            else
            {
                category = "No Category";
            }
            return category;
        }
    }
}
