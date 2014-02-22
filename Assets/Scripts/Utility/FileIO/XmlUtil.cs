using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;

public class XmlUtil{
	
	private string filePath;
	
	private XmlDocument xmlDoc;
	private XmlElement docRoot;
	private XmlElement docLibrary;

	public bool OpenXml(string fileName){
		// false means file not found, true means file existed
		bool result = false;

		fileName += ".xml";

		Debug.Log("Opening");
		Debug.Log(Application.dataPath);
		filePath = FileDirectory.GetPathRoot() + fileName;
		Debug.Log(filePath);
		
		if(!FileDirectory.FileExists(fileName)){
			
			Debug.Log("Xml file not found: " + fileName + " Path: " + filePath);
			Debug.Log("Creating Xml file: " + filePath);

			xmlDoc = new XmlDocument();// Create the XML Declaration, and append it to XML document
			XmlDeclaration dec = xmlDoc.CreateXmlDeclaration("1.0", null, null);
			xmlDoc.AppendChild(dec);// Create the root element
			XmlElement root = xmlDoc.CreateElement("Library");
			docLibrary = root;
			xmlDoc.AppendChild(root);
			xmlDoc.Save(filePath);
			
			docRoot = xmlDoc.DocumentElement;
		}
		else{
			
			result = true;
			
			xmlDoc = new XmlDocument();
			
			xmlDoc.Load(filePath);
			
			docRoot = xmlDoc.DocumentElement;
		}
		
		return result;
	}

	public XmlElement Write(string element){
		if(xmlDoc != null){
			
			Debug.Log("Writing");
			
			XmlElement xmlElement;
			
			XmlNodeList elementList = xmlDoc.GetElementsByTagName(element);
			
			if(elementList.Count == 0){
				Debug.Log("Element: " + element + " not found. Creating.");
				xmlElement = xmlDoc.CreateElement(element);
				docRoot.AppendChild(xmlElement);
			}
			else{
				Debug.Log("Element: " + element + " found.");
				xmlElement = (XmlElement) elementList[0];
			}

			return xmlElement;

		}
		else{
			Debug.Log("No xml file open for writing");	
			return null;
		}
	}
	
	public XmlElement Write(string element, string attribute, string value){
		if(xmlDoc != null){
			Debug.Log("Writing");
			XmlElement xmlElement = Write(element);
			xmlElement.SetAttribute(attribute, value);
			return xmlElement;
		}
		else{
			Debug.Log("No xml file open for writing");	
			return null;
		}
	}

	public XmlElement Write(string element, string[,] attributes){
		if(xmlDoc != null){
			Debug.Log("Writing");
			XmlElement xmlElement = Write(element);
			AddAttributes(xmlElement, attributes);
			return xmlElement;
		}
		else{
			Debug.Log("No xml file open for writing");	
			return null;
		}
	}

	public XmlElement Write(string[] nestedElement){
		if(xmlDoc != null){
			
			Debug.Log("Writing " + nestedElement[0]);
			
			XmlElement root;
			XmlElement xmlElement;
			
			XmlNodeList elementList = xmlDoc.GetElementsByTagName(nestedElement[0]);
			
			if(elementList.Count == 0){
				Debug.Log("Element: " + nestedElement[0] + " not found. Creating.");
				root = xmlDoc.CreateElement(nestedElement[0]);
				xmlElement = CreateElementPath(root, nestedElement);
				docRoot.AppendChild(root);
			}
			else{
				Debug.Log("Element: " + nestedElement[0] + " found.");
				root = (XmlElement) elementList[0];
				xmlElement = TraverseToLeaf(root, nestedElement);
				
				if(xmlElement == null){
					Debug.Log("An element in the path was missing. Creating.");
					xmlElement = CreateElementPath(root, nestedElement);
				}
			}

			return xmlElement;
		}
		else{
			Debug.Log("No xml file open for writing");	
			return null;
		}
	}

	public XmlElement Write(string[] nestedElement, string attribute, string value){
		if(xmlDoc != null){
			XmlElement xmlElement = Write(nestedElement);
			xmlElement.SetAttribute(attribute, value);
			return xmlElement;
		}
		else{
			return null;
		}
	}

	public XmlElement Write(string[] nestedElement, string[,] attributes){
		if(xmlDoc != null){
			XmlElement xmlElement = Write(nestedElement);
			AddAttributes(xmlElement, attributes);
			return xmlElement;
		}
		else{
			return null;
		}
	}
	
	private XmlElement GetNestedElement(XmlElement parentElement, string nestedElement){
		XmlElement xmlElement = (XmlElement) parentElement.GetElementsByTagName(nestedElement)[0];
		if(xmlElement == null){
			xmlElement = xmlDoc.CreateElement(nestedElement);
			parentElement.AppendChild(xmlElement);
		}
		return xmlElement;
	}
	
	public string Read(string element, string attribute){
		
		string result = "";
		
		if(xmlDoc != null){
			
			XmlNodeList elementList = xmlDoc.GetElementsByTagName(element);
			
			if(elementList.Count == 0){
				Debug.Log("Element: " + element + " not found.");
			}
			else{
				Debug.Log("Element: " + element + " found.");
				XmlElement xmlElement = (XmlElement) elementList[0];
				
				if(xmlElement.Attributes[attribute] != null){
					result = xmlElement.Attributes[attribute].Value;
				}
				else{
					Debug.Log("Attribute: " + attribute + " not found.");	
				}
			}
		}
		else{
			Debug.Log("No xml file open for reading");	
		}
		
		return result;
	}
	
	public string[,] ReadElement(string element){
		
		string[,] result = new string[0,0];
		
		if(xmlDoc != null){
			
			XmlNodeList elementList = xmlDoc.GetElementsByTagName(element);
			
			if(elementList.Count == 0){
				Debug.Log("Element: " + element + " not found.");
			}
			else{
				Debug.Log("Element: " + element + " found.");
				XmlElement xmlElement = (XmlElement) elementList[0];
				
				result = new string[xmlElement.Attributes.Count, 2];
				
				for(int i = 0; i < xmlElement.Attributes.Count; i++){
					result[i, 0] = xmlElement.Attributes[i].Name;
					result[i, 1] = xmlElement.Attributes[i].Value;
				}
			}
		}
		else{
			Debug.Log("No xml file open for reading");	
		}
		
		return result;
	}
	
	public string ReadNestedElement(string[] nestedElement, string attribute){
		
		string result = "";
		
		if(xmlDoc != null){
			
			XmlNodeList elementList = xmlDoc.GetElementsByTagName(nestedElement[0]);
			
			if(elementList.Count == 0){
				Debug.Log("Element: " + nestedElement.ToString() + " not found.");
			}
			else{
				Debug.Log("Element: " + nestedElement.ToString() + " found.");
				XmlElement xmlElement = (XmlElement) elementList[0];
				xmlElement = TraverseToLeaf(xmlElement, nestedElement);
				
				if(xmlElement.Attributes[attribute] != null){
					result = xmlElement.Attributes[attribute].Value;
				}
				else{
					Debug.Log("Attribute: " + attribute + " not found.");	
				}
			}
		}
		else{
			Debug.Log("No xml file open for reading");	
		}
		
		return result;
	}
	
	// This assumes root is nestedElement[0]'s element
	// Returns final lead element created
	private XmlElement CreateElementPath(XmlElement root, params string[] nestedElement){
		
		XmlElement temp;
		
		for(int i = 1; i < nestedElement.Length; i++){
			temp = GetNestedElement(root, nestedElement[i]);
			if(temp == null){
				temp = xmlDoc.CreateElement(nestedElement[i]);
				root.AppendChild(temp);
			}
			root = temp;
		}
		
		return root;
	}
	
	// This assumes root is nestedElement[0]'s element
	private XmlElement TraverseToLeaf(XmlElement root, string[] nestedElement){
		
		for(int i = 1; i < nestedElement.Length; i++){
			Debug.Log("Checking Element " + root.Name + " for child element " + nestedElement[i]);
			root = GetNestedElement(root, nestedElement[i]);
			
			// If we ever can't find a node, return null
			if(root == null){
				Debug.Log("Child not found");
				break;
			}
		}
		
		return root;
	}

	public void AddAttributes(XmlElement xmlElement, string[,] attributes){
		for(int i = 0; i < attributes.GetLength(0); i++){
			xmlElement.SetAttribute(attributes[i,0], attributes[i, 1]);
		}
	}
	
	public void Close(){
		if(xmlDoc != null){
			xmlDoc.Save(filePath);
			xmlDoc = null;
		}
	}
	
	public void CloseNoSave(){
		xmlDoc = null;
	}

	#region WriteLibrary

	public void WriteGameObject(GameObject gameObject){
		WriteGameObject(gameObject, docLibrary);
	}

	public void WriteGameObject(GameObject gameObject, params string[] parentElementPath){
		XmlElement leaf = TraverseToLeaf(docLibrary, parentElementPath);
		WriteGameObject(gameObject, leaf);
	}

	public void WriteGameObject(GameObject gameObject, XmlElement parentXmlElement){
		XmlElement xmlElement = GetNestedElement(parentXmlElement, "GameObject");
		AddAttributes(xmlElement, new string[,]{{"Name", gameObject.name}, {"Active", gameObject.activeSelf.ToString()}, {"Tag", gameObject.tag}, {"Layer", LayerMask.LayerToName(gameObject.layer)}, {"Static", gameObject.isStatic.ToString()}});
		WriteTransform(gameObject.transform, xmlElement);
	}

	public void WriteTransform(Transform transform){
		WriteTransform(transform, docLibrary);
	}

	public void WriteTransform(Transform transform, params string[] parentElementPath){
		XmlElement leaf = TraverseToLeaf(docLibrary, parentElementPath);
		WriteTransform(transform, leaf);
	}

	public void WriteTransform(Transform transform, XmlElement parentXmlElement){
		XmlElement xmlElement = GetNestedElement(parentXmlElement, "Transform");
		if(transform != null){
			xmlElement.SetAttribute("ComponentExists", bool.TrueString);
			WriteVector3(GetNestedElement(xmlElement, "Position"), transform.position);
			WriteVector3(GetNestedElement(xmlElement, "Rotation"), transform.rotation.eulerAngles);
			WriteVector3(GetNestedElement(xmlElement, "Scale"), transform.localScale);
		}
		else{
			xmlElement.SetAttribute("ComponentExists", bool.FalseString);
		}
	}

	public void WriteVector3(XmlElement xmlElement, Vector3 vector){
		AddAttributes(xmlElement, Vector3Array(vector));
	}

	public void WriteVector3(string element, Vector3 vector){
		Write(element, Vector3Array(vector));
	}

	public void WriteVector3(string[] nestedElement, Vector3 vector){
		Write(nestedElement, Vector3Array(vector));
	}

	private string[,] Vector3Array(Vector3 vector){
		return new string[,]{{"X", vector.x.ToString()}, {"Y", vector.y.ToString()}, {"Z", vector.z.ToString()}};
	}

	#endregion
}
