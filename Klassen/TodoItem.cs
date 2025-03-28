using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace MangerTest.todo
{
    public class TodoItem
    {
        public string Title { get; set; }
        public ObservableCollection<SubTask> SubTasks { get; set; }

        public TodoItem()
        {
            SubTasks = new ObservableCollection<SubTask>();
        }
    }

    public class SubTask
    {
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
    }
}
