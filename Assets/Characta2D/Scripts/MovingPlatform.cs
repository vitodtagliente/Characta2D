using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour {

	// store the sequence of point in wich the platform should move on
	[SerializeField]
	List<Transform> points;
	// take a counter for the platform's position
	int currentPositionIndex = 0;
	// movement speed
	public float speed = 2f;
	// 
	[SerializeField]
	float pointReachedDistance = 0.02f;

	// set true if platform should move back
	bool reverse = false;

	Transform nextPosition {
		get {
			if (currentPositionIndex < points.Count - 1 && !reverse)
				return points [currentPositionIndex + 1];
			if (reverse && currentPositionIndex >= 1)
				return points [currentPositionIndex - 1];
			return null;
		}
	}

	void Start()
	{
		if (points.Count > 0)
			transform.position = points [0].position;
		else
			// disable this component
			this.enabled = false;
	}

	void LateUpdate()
	{
		if (nextPosition != null) {
			var lookat = nextPosition.position - transform.position;
			var movement = lookat.normalized * speed * Time.deltaTime;
			transform.position += movement;

			if (Vector3.Distance (transform.position, nextPosition.position) <= pointReachedDistance) {
				if (reverse)
					currentPositionIndex--;
				else
					currentPositionIndex++;
			}
		} 

		if (nextPosition == null)
			reverse = !reverse;
	}

	void OnCollisionEnter2D(Collision2D other){
		if (other.gameObject.GetComponent<Characta2D.CharactaObject> () != null)
			other.gameObject.transform.parent = transform;
	}

	void OnCollisionExit2D(Collision2D other){
		if (other.gameObject.GetComponent<Characta2D.CharactaObject> () != null)
			other.gameObject.transform.parent = null;
	}
}
