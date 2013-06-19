using System;
using System.Net;
using Microsoft.Exchange.WebServices.Data;

namespace TaskInterfaceAbstraction.MasterCategoryList
{
	class Program
	{
		static void Main()
		{
			var service = new ExchangeService(ExchangeVersion.Exchange2010_SP1) { Credentials = new NetworkCredential("", "") };
			service.AutodiscoverUrl("", url => true);

			var list = MasterCategoryList.Bind(service);
			list.Categories.Add(new Category("Vacation", CategoryColor.DarkMaroon, CategoryKeyboardShortcut.CtrlF10));
			list.Update();


		}
	}
}
