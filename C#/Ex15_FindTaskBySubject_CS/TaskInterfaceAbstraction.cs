using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using ExchangeAbstraction;
using Microsoft.Exchange.WebServices.Data;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace ExchangeAbstraction
{

    // This sample is for demonstration purposes only. Before you run this sample, make sure that the code meets the coding requirements of your organization.
    public class TaskInterfaceAbstraction
    {
        static private ExchangeService service = null;
        static private FindItemsResults<Item> taskItems = null;

        public static FindItemsResults<Item> RetrieveTasks(string email, string password)
        {
            if (service == null)
            {
                service = Service.ConnectToService(UserDataFromConsole.GetUserData(email, password), new TraceListener());
            }
            // Specify the folder to search, and limit the properties returned in the result.
            TasksFolder tasksfolder = TasksFolder.Bind(service,
                                                        WellKnownFolderName.Tasks,
                                                        new PropertySet(BasePropertySet.FirstClassProperties, FolderSchema.TotalCount));

            // Set the number of items to the smaller of the number of items in the Contacts folder or 1000.
            int numItems = tasksfolder.TotalCount < 1000 ? tasksfolder.TotalCount : 1000;

            // Instantiate the item view with the number of items to retrieve from the contacts folder.
            ItemView view = new ItemView(numItems);

            // To keep the request smaller, send only the display name.
            view.PropertySet = new PropertySet(BasePropertySet.FirstClassProperties, TaskSchema.Subject);

            // Retrieve the items in the Tasks folder
            taskItems = service.FindItems(WellKnownFolderName.Tasks, view);

            // If the subject of the task matches only one item, return that task item.
            if (taskItems.Count() > 1)
            {
                service.LoadPropertiesForItems(taskItems.Items, PropertySet.FirstClassProperties);
                return (FindItemsResults<Item>)taskItems;
            }
            // No tasks, were found.
            else
            {
                return null;
            }
        }

        public static void AddTask(string email, string password, Item task) 
        { 
            if (service == null)
            {
                service = Service.ConnectToService(UserDataFromConsole.GetUserData(email, password), new TraceListener());
            }
            
            // Create the new task in the specified destination folder. 
            task.Save(WellKnownFolderName.Tasks); 

            Console.WriteLine("Recurring task created."); 
        }

        public static ExchangeService GetService (string email, string password)
        {
            if (service == null)
            {
                service = Service.ConnectToService(UserDataFromConsole.GetUserData(email, password), new TraceListener());
            }
            return service;
        }

    } 
}
