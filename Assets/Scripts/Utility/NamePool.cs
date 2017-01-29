using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;

using UnityEngine;

public class NamePool
{
	private List<string> Strings;

	public NamePool ( string path, string language) {
		setLanguage(path, language);
	}

	/*
    path = path to XML resource example:  Path.Combine(Application.dataPath, "lang.xml")
    language = language to use example:  "English"
    */
	public void setLanguage ( string path, string language) {
		var xml = new XmlDocument();
		xml.Load(path);

		Strings = new List<string>();
		var element = xml.DocumentElement[language];
		if (element != null) {
			var elemEnum = element.GetEnumerator();
			while (elemEnum.MoveNext()) {
				var xmlItem = (XmlElement)elemEnum.Current;
				Strings.Add(xmlItem.InnerText);
			}
		} else {
			//Debug.LogError("The specified language does not exist: " + language);
		}
	}

	public string getName (int index) {
		return (string)Strings[index];
	}

	public int getNbNames() {
		return Strings.Count;
	}
}