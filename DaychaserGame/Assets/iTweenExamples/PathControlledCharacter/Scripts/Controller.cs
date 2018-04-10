using UnityEngine;
using System.Collections;

public class Controller : MonoBehaviour {
	public Transform[] controlPath;
	public Transform character;
    public Transform terrain;
	public enum Direction {Forward,Reverse};
	
	private float pathPosition=0;
	private RaycastHit hit;
	private float speed = .2f;
	private float rayLength = 30;
	private Direction characterDirection;
	private Vector3 floorPosition;	
	private float lookAheadAmount = .01f;
	private float ySpeed=0;
	private float gravity=.5f;
	public float jumpForce=.12f;
	private uint jumpState=0; //0=grounded 1=jumping
	
	void OnDrawGizmos(){
		iTween.DrawPath(controlPath,Color.blue);	
	}	
	
	
	void Start(){
		//plop the character pieces in the "Ignore Raycast" layer so we don't have false raycast data:	
		foreach (Transform child in character) {
			child.gameObject.layer=2;
		}

        //This was the most effecient way don't ask questions
        foreach(Transform child in terrain)
        {
            foreach(Transform child2 in child)
            {
                child2.gameObject.layer = 2;

                foreach (Transform child3 in child2)
                {
                    child3.gameObject.layer = 2;

                    foreach(Transform child4 in child3)
                    {
                        child4.gameObject.layer = 2;
                    }
                }
            } 
        }
	}
	
	void Update(){
		DetectKeys();
        FindFloorAndRotation();
	}

    private void FixedUpdate()
    {
        MoveCharacter();
    }


    void DetectKeys(){
		//forward path movement:
		if(Input.GetKeyDown("right")){
			characterDirection=Direction.Forward;	
		}
		if(Input.GetKey("right")) {
            if (pathPosition < 0.99)
            {
                pathPosition += Time.deltaTime * speed;
            }		
		}
		
		//reverse path movement:
		if(Input.GetKeyDown("left")){
			characterDirection=Direction.Reverse;
		}
		if(Input.GetKey("left")) {
			//handle path loop around since we can't interpolate a path percentage that's negative(well duh):
			float temp = pathPosition - (Time.deltaTime * speed);
			if(temp<0){
				pathPosition=0;	
			}else {
				pathPosition -= (Time.deltaTime * speed);
			}
		}	
		
		//jump:
		if (Input.GetKeyDown("space") && jumpState==0) {
			ySpeed-=jumpForce;
			jumpState=1;
            character.GetChild(0).GetComponent<Animator>().Play("Jump", -1, 0.0f);
        }
	}
	
	
	void FindFloorAndRotation(){

		float pathPercent = pathPosition%1;
		Vector3 coordinateOnPath = iTween.PointOnPath(controlPath,pathPercent);
		Vector3 lookTarget;
		
		//calculate look data if we aren't going to be looking beyond the extents of the path:
		if(pathPercent-lookAheadAmount>=0 && pathPercent+lookAheadAmount <=1){
			
			//leading or trailing point so we can have something to look at:
			if(characterDirection==Direction.Forward){
				lookTarget = iTween.PointOnPath(controlPath,pathPercent+lookAheadAmount);
			}else{
				lookTarget = iTween.PointOnPath(controlPath,pathPercent-lookAheadAmount);
			}
			
			//look:
			character.LookAt(lookTarget);
			
			//nullify all rotations but y since we just want to look where we are going:
			float yRot = character.eulerAngles.y;
			character.eulerAngles=new Vector3(0,yRot,0);
		}

		if (Physics.Raycast(coordinateOnPath,-Vector3.up,out hit, rayLength)){
            Debug.DrawRay(coordinateOnPath, -Vector3.up * hit.distance);
            floorPosition = hit.point;
		}
	}
	
	
	void MoveCharacter(){
		//add gravity:
		ySpeed += gravity * Time.deltaTime;
		
		//apply gravity:
		character.position=new Vector3(floorPosition.x,character.position.y-ySpeed,floorPosition.z);
		
		//floor checking:
		if(character.position.y<floorPosition.y){
			ySpeed=0;
			jumpState=0;
			character.position=new Vector3(floorPosition.x,floorPosition.y,floorPosition.z);
		}		
	}
}