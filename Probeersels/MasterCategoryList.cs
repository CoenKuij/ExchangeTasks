using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.Exchange.WebServices.Data;

namespace MasterCategoryList
{
	/// <remarks/>
	[Serializable]
	[XmlType(AnonymousType = true, Namespace = "CategoryList.xsd")]
	[XmlRoot(ElementName = "categories", Namespace = "CategoryList.xsd", IsNullable = false)]
	public class MasterCategoryList
	{
		private UserConfiguration _UserConfigurationItem;

		/// <remarks/>
		[XmlElement("category")]
		public List<Category> Categories { get; set; }

		/// <remarks/>
		[XmlIgnore]
		public Guid? DefaultCategory { get; set; }

		[XmlAttribute("default")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public string DefaultCategoryText
		{
			get { return DefaultCategory != null ? DefaultCategory.ToString() : string.Empty; }
			set
			{
				Guid result;
				DefaultCategory = (Guid.TryParse(value, out result)) ? result : (Guid?) null;
			}
		}
		

		/// <remarks/>
		[XmlAttribute("lastSavedSession")]
		public byte LastSavedSession { get; set; }

		/// <remarks/>
		[XmlAttribute("lastSavedTime")]
		public DateTime LastSavedTime { get; set; }

		public static MasterCategoryList Bind(ExchangeService service)
		{
			var item = UserConfiguration.Bind(service, "CategoryList", WellKnownFolderName.Calendar,
			                                   UserConfigurationProperties.XmlData);

			var reader = new StreamReader(new MemoryStream(item.XmlData), Encoding.UTF8, true);
			var serializer = new XmlSerializer(typeof(MasterCategoryList));
			var result = (MasterCategoryList) serializer.Deserialize(reader);
			result._UserConfigurationItem = item;
			return result;
		}

		public void Update()
		{
			var stream = new MemoryStream();
			var writer = XmlWriter.Create(stream, new XmlWriterSettings {Encoding = Encoding.UTF8});
			var serializer = new XmlSerializer(typeof(MasterCategoryList));

			serializer.Serialize(writer, this);
			writer.Flush();
			_UserConfigurationItem.XmlData = stream.ToArray();
			_UserConfigurationItem.Update();
		} 
	}
}