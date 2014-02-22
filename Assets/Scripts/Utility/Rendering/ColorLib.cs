using UnityEngine;
using System.Collections;

public class ColorLib : MonoBehaviour {
	
	public static ColorLib Instance;

	public static Color red;
	public Color Red;
	
	public static Color blue;
	public Color Blue;
	
	public static Color green;
	public Color Green;
	
	public static Color orange;
	public Color Orange;
	
	public static Color yellow;
	public Color Yellow;
	
	public static Color purple;
	public Color Purple;
	
	public static Color teal;
	public Color Teal;
	
	public static Color aqua;
	public Color Aqua;
	
	public static Color pink;
	public Color Pink;
	
	public Color[] allColors;
	
	void Awake(){
		
		Instance = this;
		
		red = Red;	
		blue = Blue;
		green = Green;
		orange = Orange;
		yellow = Yellow;
		purple = Purple;
		teal = Teal;
		aqua = Aqua;
		pink = Pink;
		
		allColors = new Color[9];
		allColors[0] = red;
		allColors[1] = blue;
		allColors[2] = green;
		allColors[3] = orange;
		allColors[4] = yellow;
		allColors[5] = purple;
		allColors[6] = teal;
		allColors[7] = aqua;
		allColors[8] = pink;
	}
	
	public static Color GetRandomColor(){
		return ColorLib.Instance.allColors[Random.Range(0, ColorLib.Instance.allColors.Length)];
	}
}
