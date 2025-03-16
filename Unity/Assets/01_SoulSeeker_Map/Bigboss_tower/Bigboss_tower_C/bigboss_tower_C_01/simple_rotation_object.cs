using UnityEngine;
using System.Collections;

public class simple_rotation_object : MonoBehaviour {
	   
	public float myRotationSpeed = 100.0f;     
	// Use this for initialization    

	// Update is called once per frame    
	void Update ()    {        
			transform.Rotate(0, 0, myRotationSpeed * Time.deltaTime);

	}     
}
