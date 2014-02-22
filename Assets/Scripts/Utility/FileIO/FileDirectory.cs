using UnityEngine;
using System.Collections;
using System.IO;

public class FileDirectory{

	private static string pathRoot;
	private static bool initialized;
	private static char delimiter;

	private static void DetermineFilePath(){
		
		if(!initialized){
			
			if(Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor){
				pathRoot = System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData);
			}
			else if(Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.OSXEditor){
				pathRoot = Application.persistentDataPath;
			}

			delimiter = '/';
			if(Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor){
				delimiter = '\\';
			}

			pathRoot += delimiter.ToString();

			initialized = true;
		}
	}

	public static bool DirectoryExists(string path){

		DetermineFilePath();

		if(Directory.Exists(FullPath(path))){
			return true;
		}
		else{
			return false;
		}
	}

	public static void CreateDirectory(string path){
		DetermineFilePath();
		Directory.CreateDirectory(FullPath(path));
	}

	public static void DeleteDirectory(string path){
		DetermineFilePath();
		string fullPath = FullPath(path);
		if(DirectoryExists(fullPath)){
			Directory.Delete(fullPath, true);
		}
	}

	public static void MoveDirectory(string originalPath, string newPath){
		DetermineFilePath();
		originalPath = FullPath(originalPath);
		newPath = FullPath(newPath);
		if(DirectoryExists(originalPath) && !DirectoryExists(newPath)){
			Directory.Move(originalPath, newPath);
		}
	}

	public static bool FileExists(string path){

		DetermineFilePath();

		int split = path.LastIndexOf(delimiter) + 1;
		string file = path.Substring(split);
		string directory = path.Substring(0, split);

		if(DirectoryExists(directory) && File.Exists(FullPath(path))){
			return true;
		}
		else{
			return false;
		}
	}

	private static string FullPath(string path){
		DetermineFilePath();
		return pathRoot + path;
	}

	public static string GetPathRoot(){
		DetermineFilePath();
		return pathRoot;
	}
}
