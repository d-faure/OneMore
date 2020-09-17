﻿//************************************************************************************************
// Copyright © 2020 Steven M Cohn.  All rights reserved.
//************************************************************************************************

namespace River.OneMoreAddIn.Helpers.Settings
{
	using System;
	using System.IO;
	using System.Xml.Linq;


	/// <summary>
	/// Loads, save, and manages user settings
	/// </summary>
	internal class SettingsProvider
	{
		private readonly string path;
		private readonly XElement root;


		/// <summary>
		/// Initialize a new provider.
		/// </summary>
		public SettingsProvider()
		{
			path = Path.Combine(
				PathFactory.GetAppDataPath(), Properties.Resources.SettingsFilename);

			if (File.Exists(path))
			{
				try
				{
					root = XElement.Load(path);
				}
				catch (Exception exc)
				{
					Logger.Current.WriteLine($"Error reading {path}", exc);
				}
			}

			if (root == null)
			{
				// file not found so initialize with defaults
				root = new XElement("settings",
					new XElement("images",
						new XAttribute("width", "500")
					)
				);
			}
		}


		/// <summary>
		/// Gets the preset image width for resizing images
		/// </summary>
		/// <returns></returns>
		public int GetImageWidth()
		{
			var width = root.Element("images")?.Attribute("width").Value;
			return width == null ? 500 : int.Parse(width);
		}


		public void SetImageWidth(int width)
		{
			root.Element("images").Attribute("width").Value = width.ToString();
		}


		public SettingCollection GetCollection(string name)
		{
			var settings = new SettingCollection(name);

			var elements = root.Element(name)?.Elements();
			if (elements != null)
			{
				foreach (var element in elements)
				{
					settings.Add(element.Name.LocalName, element.Value);
				}
			}

			return settings;
		}


		public void SetCollection(SettingCollection settings)
		{
			var element = new XElement(settings.Name);
			foreach (var key in settings.Keys)
			{
				element.Add(new XElement(key, settings[key]));
			}

			var e = root.Element(settings.Name);
			if (e != null)
			{
				e.ReplaceWith(element);
			}
			else
			{
				root.Add(element);
			}
		}


		public void Save()
		{
			PathFactory.EnsurePathExists(Path.GetDirectoryName(path));
			root.Save(path, SaveOptions.None);
		}
	}
}