//using Microsoft.Win32.TaskScheduler;
//using System;
//using System.IO;
//using System.Linq; 
//using System.Reflection;


//namespace Unach.Codesi.Cgrni.Cooperacion.Presentation.CentralAdmin.Utils.Tasks
//{
//    public class Scheduler
//    {
//        public string TaskName;
//        public Scheduler(string _taskName)
//        {
//            TaskName = _taskName;
//        }

//        public  void SetTask( DateTime startDate, DaysOfTheWeek daysOfWeek,   string autor, string descripcion, string filePath, string aplicacion)
//        {
//            var ts = new TaskService();
//            var td = ts.NewTask();
//            td.RegistrationInfo.Author = autor;
//            td.RegistrationInfo.Description = descripcion;
//            td.Triggers.Add(new WeeklyTrigger { StartBoundary = startDate, DaysOfWeek = daysOfWeek, Enabled = true });
     
//            //run this application or setup path to the file
//            var action = new ExecAction(aplicacion, null, null);
//            if (filePath != string.Empty && File.Exists(filePath))
//            {
//                action = new ExecAction(filePath);
//            }
//            //action.Arguments = "/Scheduler"; //Argumento para pasarle a la aplicacion 
//            td.Actions.Add(action);

//            ts.RootFolder.RegisterTaskDefinition(TaskName, td,TaskCreation.CreateOrUpdate, "Administrator","Codesi2019****");
//        }

//        public  void DeleteTask(string taskName)
//        {
//            var ts = new TaskService();
//            var task = ts.RootFolder.GetTasks().Where(a => a.Name.ToLower() == taskName.ToLower()).FirstOrDefault();
//            if (task != null)
//            {
//                ts.RootFolder.DeleteTask(taskName);
//            }
//        }

//        //public  Task GetTask()
//        //{
//        //    var ts = new TaskService();
//        //    var task = ts.RootFolder.GetTasks().Where(a => a.Name.ToLower() == TaskName.ToLower()).FirstOrDefault();

//        //    return task;
//        //}

//        //public  string GetNextScheduleTaskDate()
//        //{
//        //    try
//        //    {
//        //        var task = Scheduler.GetTask();
//        //        if (task != null)
//        //        {
//        //            var trigger = task.Definition.Triggers.FirstOrDefault();
//        //            if (trigger != null)
//        //            {
//        //                if (trigger.TriggerType == TaskTriggerType.Weekly)
//        //                {
//        //                    if (trigger.Enabled)
//        //                    {
//        //                        var weeklyTrigger = (WeeklyTrigger)trigger;

//        //                        return task.NextRunTime.ToString("yyyy MMM. dd dddd 'at ' HH:mm");
//        //                    }
//        //                }
//        //            }
//        //        }
//        //    }
//        //    catch (Exception)
//        //    { }

//        //    return "no scheduled date!";
//        //}
//    }
//}
