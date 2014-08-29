using UnityEngine;
using System.Collections;
using System.IO;
using System;
using System.Globalization;

public class PlanetBuilder : MonoBehaviour {
	public string planetName;
	private MeshBuilder planetConstructor;
	public Mesh planet;
	public Vector2 center;
	private string[] points;
	private Vector2[] points2D;
	public float width = 1f;
	public float scale = 1.0f;
	public bool isInvetertedX = true;
	public bool moveToTransform = false;

	// Use this for initialization
	void Start () {
		planetName = "Planet_1_Maya.txt";
		planetConstructor = new MeshBuilder();
		loadPoints(planetName);
		points2D = new Vector2[points.Length/2];
		buildPlanet ();
		Debug.Log(points[0]);

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void loadPoints (string filename) {
		StreamReader theReader = new StreamReader(planetName); //"/models/planets/" + 
			
		points = theReader.ReadToEnd().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
			
		theReader.Close();
	}

	void buildPlanet () {
		Vector2 planetCenter = new Vector3(0,0,0);//findCenter();
		for (int i=0; i<points.Length-3; i = i+2) {
			float aX;
			float aY;
			float bX;
			float bY;
			if (moveToTransform) {
				aX = (float.Parse(points[i], CultureInfo.InvariantCulture.NumberFormat) - planetCenter.x + transform.position.x) * scale;
				aY = (float.Parse(points[i+1], CultureInfo.InvariantCulture.NumberFormat) - planetCenter.y + transform.position.y) * scale;
				bX = (float.Parse(points[i+2], CultureInfo.InvariantCulture.NumberFormat) - planetCenter.x + transform.position.x) * scale;
				bY = (float.Parse(points[i+3], CultureInfo.InvariantCulture.NumberFormat) - planetCenter.y + transform.position.y) * scale;
			} else {
				aX = float.Parse(points[i], CultureInfo.InvariantCulture.NumberFormat);
				aY = float.Parse(points[i+1], CultureInfo.InvariantCulture.NumberFormat);
				bX = float.Parse(points[i+2], CultureInfo.InvariantCulture.NumberFormat);
				bY = float.Parse(points[i+3], CultureInfo.InvariantCulture.NumberFormat);
			}

			if (isInvetertedX) {
				aX *= -1;
				bX *= -1;
			}
//			float aX = float.Parse(points[i], CultureInfo.InvariantCulture.NumberFormat) - planetCenter.x * scale;
//			float aY = float.Parse(points[i+1], CultureInfo.InvariantCulture.NumberFormat) - planetCenter.y * scale;
//			float bX = float.Parse(points[i+2], CultureInfo.InvariantCulture.NumberFormat) - planetCenter.x * scale;
//			float bY = float.Parse(points[i+3], CultureInfo.InvariantCulture.NumberFormat) - planetCenter.y * scale;
			
			Vector2 pointA = new Vector2(aX, aY);
			Vector2 pointB = new Vector2(bX, bY);
			points2D[i/2] = pointA;

			//planetConstructor.BuildQuad(planetConstructor, pointA, pointB, width);
			planetConstructor.BuildStrip(planetConstructor, pointA, width);
		}
		planet = planetConstructor.CreateMesh();
		//this.GetComponent<MeshFilter>().mesh = planet;
		//this.GetComponent<MeshCollider>().mesh = planet;
		this.GetComponent<PolygonCollider2D>().points = points2D;
		//this.GetComponent<PlanetDeform>().
		//this.GetComponent<EdgeCollider2D>().points = points2D;
	}

	Vector2 findCenter() {
		for (int i=0; i<points.Length-1; i = i+2) {
			float aX = float.Parse(points[i], NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture.NumberFormat);
			float aY = float.Parse(points[i+1], NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture.NumberFormat);
			Vector2 pointA = new Vector2(aX, aY);
			center += pointA;
		}
		center /= points.Length/2;
		return center;
	}
}
