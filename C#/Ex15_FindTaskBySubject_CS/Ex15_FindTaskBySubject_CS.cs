using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Exchange101;
using Microsoft.Exchange.WebServices.Data;

namespace Exchange101
{

    // This sample is for demonstration purposes only. Before you run this sample, make sure that the code meets the coding requirements of your organization.
    public class Ex15_FindTaskBySubject_CS
    {
        static ExchangeService service;
        static private FindItemsResults<Item> taskItems = null;

        public static FindItemsResults<Item> RetrieveTasks(string email, string password)
        {
            service = Service.ConnectToService(UserDataFromConsole.GetUserData(email, password), new TraceListener());
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

    }
}
