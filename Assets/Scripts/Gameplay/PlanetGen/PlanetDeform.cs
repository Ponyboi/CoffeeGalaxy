using UnityEngine;
using System.Collections;

public class PlanetDeform : MonoBehaviour {

	public bool colliderOn = false;
	public float frequency = 0.01f;
	public float speed = 0.02f;
	public float amplitude = 0.001f;
	public float amplitudeBreathe = 0.002f;
	public float phaseAmp = 0f;
	public float difference = 1;
	public float offset = 0;
	public float staticPos = 3f;
	public float staticPosBreath = 1f;
	public float degree = 1.5f;
	public float randomScaling = 0.5f;
	private Vector3 parentPos;
	public float radius = 5;
	public MeshFilter mf;
	public Mesh mesh;
	public PolygonCollider2D polygonCollider;
	private Vector3[] meshVerts;
	private Vector2[] polygonColliderVerts;

	void Start () {
		parentPos = transform.InverseTransformPoint(transform.parent.position);//transform.position;//transform.parent.position;
		mf = this.GetComponent<MeshFilter>();
		mesh = mf.mesh;
		polygonCollider = this.GetComponent<PolygonCollider2D>();
		meshVerts = (Vector3[])mesh.vertices.Clone();
		polygonColliderVerts = (Vector2[])polygonCollider.points.Clone();
	}

	void Update () {
		
		//transform.Translate(relativeVelocity * Time.deltaTime);

		
		mesh.vertices = SineDeform(mesh.vertices, meshVerts);
		mesh.RecalculateBounds();

		if (colliderOn) {
//			Debug.Log("polyverts: " + polygonColliderVerts.Length);
//			polygonColliderVerts = (Vector2[])polygonCollider.points.Clone();
			polygonCollider.points = SineDeform2D(polygonCollider.points ,polygonColliderVerts);
		}
	}
	
	Vector3[] SineDeform(Vector3[] vertices, Vector3[] verts) {  // takes in vertices to change and verts as hard copy to base deformation on
	
//		Vector3[] vertices = mesh.vertices;
//		Vector3 hitPoint = transform.InverseTransformPoint(collision.contacts[0].point);
//		float hitRadius = relativeVelocity.magnitude;
//		Vector3 hitDir = transform.InverseTransformDirection(-collision.contacts[0].normal);
		
		int i = 0;
		while (i < vertices.Length) {
//			float sine = Mathf.Sin(Time.time * speed) * amplitude;
//			Vector3 newPos = new Vector3(verts[i].x + sine, verts[i].y, verts[i].z);
//			vertices[i] = newPos;

			float distance = Vector3.Distance(verts[i], parentPos);
			Vector3 dir = (verts[i] - parentPos);
			if(dir.magnitude < radius){
				float phase = (Vector3.Angle(Vector3.up, dir)/ 180) * phaseAmp;
				float sine = (Mathf.Sin(Time.time * speed)* amplitude) + staticPos ;
				float sineBreathe = (Mathf.Sin(Time.time * speed + phase)* amplitudeBreathe) + staticPosBreath ;
				//float amount = (1 - dir.magnitude / radius) * difference;
				float amount = (1+Mathf.Pow((radius * sineBreathe) / dir.magnitude, degree)) * difference;
				Vector3 vertMove = dir * amount * sine;
				vertices[i] = vertMove;
			}
			i++;
		}
		return vertices;
	}

	Vector2[] SineDeform2D(Vector2[] vertices, Vector2[] verts) {  // takes in vertices to change and verts as hard copy to base deformation on
		
		//		Vector3[] vertices = mesh.vertices;
		//		Vector3 hitPoint = transform.InverseTransformPoint(collision.contacts[0].point);
		//		float hitRadius = relativeVelocity.magnitude;
		//		Vector3 hitDir = transform.InverseTransformDirection(-collision.contacts[0].normal);
		
		int i = 0;
		while (i < vertices.Length) {
			//			float sine = Mathf.Sin(Time.time * speed) * amplitude;
			//			Vector3 newPos = new Vector3(verts[i].x + sine, verts[i].y, verts[i].z);
			//			vertices[i] = newPos;
			
			float distance = Vector2.Distance(verts[i], parentPos);
			Vector2 dir = (verts[i] - (new Vector2(parentPos.x, parentPos.y)));
			if(dir.magnitude < radius){
				float phase = (Vector3.Angle(Vector3.up, dir)/ 180) * phaseAmp;
				float sine = (Mathf.Sin(Time.time * speed)* amplitude) + staticPos ;
				float sineBreathe = (Mathf.Sin(Time.time * speed + phase)* amplitudeBreathe) + staticPosBreath ;
				//float amount = (1 - dir.magnitude / radius) * difference;
				float amount = (1+Mathf.Pow((radius * sineBreathe) / dir.magnitude, degree)) * difference;
				Vector2 vertMove = dir * amount * sine;
				vertices[i] = vertMove;
			}
			i++;
		}
		return vertices;
	}
}