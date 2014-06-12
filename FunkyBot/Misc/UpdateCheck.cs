﻿using System;
using System.Xml;
using FunkyBot.Misc;

namespace FunkyBot
{
	 public class Update
	 {

		  public static void CheckUpdate()
		  {
				// in newVersion variable we will store the
				// version info from xml file
				Version newVersion=null;
				// and in this variable we will put the url we
				// would like to open so that the user can
				// download the new version
				// it can be a homepage or a direct
				// link to zip/exe file
				string url="";

				try
				{
					 string xmlURL="https://raw.github.com/herbfunk/Funky/master/FunkyBot/Version.xml";
					 using (XmlTextReader reader=new XmlTextReader(xmlURL))
					 {
						  // simply (and easily) skip the junk at the beginning
						  reader.MoveToContent();
						  // internal - as the XmlTextReader moves only
						  // forward, we save current xml element name
						  // in elementName variable. When we parse a
						  // text node, we refer to elementName to check
						  // what was the node name
						  string elementName="";
						  // we check if the xml starts with a proper
						  // "ourfancyapp" element node
						  if ((reader.NodeType==XmlNodeType.Element)&&
								(reader.Name=="FunkyTrinity"))
						  {
								while (reader.Read())
								{
									 // when we find an element node,
									 // we remember its name
									 if (reader.NodeType==XmlNodeType.Element)
										  elementName=reader.Name;
									 else
									 {
										  // for text nodes...
										  if ((reader.NodeType==XmlNodeType.Text)&&
												(reader.HasValue))
										  {
												// we check what the name of the node was
												switch (elementName)
												{
													 case "version":
														  // thats why we keep the version info
														  // in xxx.xxx.xxx.xxx format
														  // the Version class does the
														  // parsing for us
														  newVersion=new Version(reader.Value);
														  break;
													 case "url":
														  url=reader.Value;
														  break;
												}
										  }
									 }
								}
						  }
					 }


				} catch (Exception)
				{
				}
				finally
				{

				}

				// compare the versions
				if (Funky.Instance.Version.CompareTo(newVersion)<0)
				{
					 Logger.DBLog.InfoFormat("New Version Available!");
					 Logger.DBLog.InfoFormat("https://github.com/herbfunk/Funky/archive/master.zip");
				}
		  }

	 }
}
