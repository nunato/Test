using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Boundary
{
	public float xMin, xMax, zMin, zMax;
}

public class PlayerController : MonoBehaviour
{
	public float speed;
	public float tilt;
	public Boundary boundry;
	public GameObject shot;
	public Transform[] shotSpawns;
	public float fireRate;
	public SimpleTouchPad touchPad;
	public SimpleTouchAreaButton areaButton;

	private float nextFire;
	private Quaternion calibrationQuaternion;

	void Start()
	{
		CalibrateAccellerometer();
	}

	void Update()
	{
		if( areaButton.CanFire() && Time.time > nextFire ){
			nextFire = Time.time + fireRate;
			foreach( var shotSpawn in shotSpawns ){
				Instantiate( shot, shotSpawn.position, shotSpawn.rotation );
			}
			GetComponent<AudioSource>().Play();
		}
	}

	//Used to calibrate the Input.acceleration input
	void CalibrateAccellerometer()
	{
		Vector3 accelerationSnapshot = Input.acceleration;
		Quaternion rotateQuaternion = Quaternion.FromToRotation( new Vector3( 0.0f, 0.0f, -1.0f ), accelerationSnapshot );
		calibrationQuaternion = Quaternion.Inverse( rotateQuaternion );
	}

	//Get the 'calibrated' value from the Input
	Vector3 FixAccelleration( Vector3 acceleration )
	{
		Vector3 fixedAcceleration = calibrationQuaternion * acceleration;
		return fixedAcceleration;
	}

	void FixedUpdate()
	{
//		float moveHorizontal = Input.GetAxis( "Horizontal" );
//		float moveVertical = Input.GetAxis( "Vertical" );

//		Vector3 movement = new Vector3( moveHorizontal, 0.0f, moveVertical );

//		Vector3 accelerationRaw = Input.acceleration;
//		Vector3 acceleration = FixAccelleration( accelerationRaw );
//		Vector3 movement = new Vector3( acceleration.x, 0.0f, acceleration.y );

		Vector2 direction = touchPad.GetDirection();
		Vector3 movement = new Vector3( direction.x, 0.0f, direction.y );
		GetComponent<Rigidbody>().velocity = movement * speed;

		GetComponent<Rigidbody>().position = new Vector3
		(
			Mathf.Clamp( GetComponent<Rigidbody>().position.x, boundry.xMin, boundry.xMax ),
			0.0f,
			Mathf.Clamp( GetComponent<Rigidbody>().position.z, boundry.zMin, boundry.zMax )
		);

		GetComponent<Rigidbody>().rotation = Quaternion.Euler( 0.0f, 0.0f, GetComponent<Rigidbody>().velocity.x * -tilt );
	}
}