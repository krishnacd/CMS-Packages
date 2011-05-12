﻿using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;
using System.Linq;
using Composite.Tools.PackageCreator.Types;

namespace Composite.Tools.PackageCreator
{
	public static class Extension
	{
		public static void SaveTabbed(this XDocument doc, string fileName)
		{
			XmlTextWriter writer = new XmlTextWriter(fileName, null);
			writer.Formatting = Formatting.Indented;
			writer.Indentation = 1;
			writer.IndentChar = '\t';
			doc.Save(writer);
			writer.Close();
		}

		public static Type Get(this Dictionary<PCCategoryAttribute, Type> categoryTypes, string name)
		{
			return categoryTypes.Where(d => d.Key.Name == name).Select(d => d.Value).First();
		}

		public static PCCategoryAttribute GetCategoryAtribute(this Type type)
		{
			object[] attributes = type.GetCustomAttributes(typeof(PCCategoryAttribute), true);

			if (attributes.Length == 0) return null;

			PCCategoryAttribute category = (PCCategoryAttribute)attributes[0];
			return category;
			
		}

		public static string GetCategoryNameAtribute(this Type type)
		{
			return type.GetCategoryAtribute().Name;
		}


		public static string GetCategoryNameAtribute(this IPackageCreatorItem item)
		{
			return item.GetType().GetCategoryNameAtribute();
		}

		public static string GetProperty(this object o, string propertyName)
		{
			try
			{
				var property = o.GetType().GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				return property.GetValue(o, null).ToString();
			}
			catch { }
			return null;

		}
		public static T GetProperty<T>(this object o, string propertyName)
		{
			try
			{
				var property = o.GetType().GetProperty(propertyName);
				return (T)property.GetValue(o, null);
			}
			catch { }
			return default(T);
		}

		public static void Add(this Dictionary<string, Dictionary<string, XElement>> dictionary, string key, string listKey, XElement listValue)
		{
			if(!dictionary.ContainsKey(key))
				dictionary[key] = new Dictionary<string,XElement>();
			dictionary[key][listKey] = listValue;
		}
	}
}