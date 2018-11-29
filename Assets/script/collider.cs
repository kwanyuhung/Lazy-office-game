using UnityEngine;
using System.Collections;


public class collider : MonoBehaviour {

	public bool working = false;

	public int z = 0;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void  OnTriggerEnter2D(Collider2D trigger) {
		if (trigger.gameObject.tag == "staff") {
			if(working == true){
				print ("trigger");
				gameObject.transform.position = new Vector3 (transform.position.x, transform.position.y, z);
			}
		}
	}
	void OnTriggerExit2D(Collider2D trigger){
		if (trigger.gameObject.tag == "staff") {
			if (working != true) {
				print ("Exit");
				gameObject.transform.position = new Vector3 (transform.position.x, transform.position.y, z - 3);
			}
		}
	}
	void workingtest(bool work){
		working = work;
	}
}
