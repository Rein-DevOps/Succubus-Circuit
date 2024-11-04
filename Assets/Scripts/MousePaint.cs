using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePaint : MonoBehaviour {

	public GameObject stamp;
	public float mouseSpeed = 1f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		// Get the mouse's movement information
		float mouseX = Input.GetAxis ("Mouse X");
		float mouseY = Input.GetAxis ("Mouse Y");

		// Scale the mouse input, to get a pleasing amount of movement.
		Vector3 moveBy = new Vector3(mouseX, mouseY, 0.0f);
		moveBy *= mouseSpeed;

		// Move the object
		transform.Translate(moveBy);

		// Listen for click events
		if (Input.GetMouseButtonDown(0)) {
			// Clone the stamp when the user clicks
			Instantiate(stamp, transform.position, transform.rotation);
		}
	}

}