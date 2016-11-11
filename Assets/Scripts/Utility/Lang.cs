using System;
using System.Collections;
using System.IO;
using System.Xml;

using UnityEngine;

public class Lang
{
	private Hashtable Strings;

	public Lang ( string path, string language) {
		setLanguage(path, language);
	}

	/*
    path = path to XML resource example:  Path.Combine(Application.dataPath, "lang.xml")
    language = language to use example:  "English"
    */
	public void setLanguage ( string path, string language) {
		var xml = new XmlDocument();
		xml.Load(path);

		Strings = new Hashtable();
		var element = xml.DocumentElement[language];
		if (element != null) {
			var elemEnum = element.GetEnumerator();
			while (elemEnum.MoveNext()) {
				var xmlItem = (XmlElement)elemEnum.Current;
				Strings.Add(xmlItem.GetAttribute("name"), xmlItem.InnerText);
			}
		} else {
			Debug.LogError("The specified language does not exist: " + language);
		}
	}
		
	public string getString (string name) {
		if (!Strings.ContainsKey(name)) {
			Debug.LogError("The specified string does not exist: " + name);

			return "";
		}

		return (string)Strings[name];
	}

}