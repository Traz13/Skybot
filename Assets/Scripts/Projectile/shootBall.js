#pragma strict

private var focusObj: GameObject;
var player: GameObject;
var slingjoin: GameObject;
var elasticTexture: Texture2D;
private var lineRenderer : LineRenderer;
var force: float;
var maxForce: float;
var maxDistance: float;

private var offsetPos: Vector3 = new Vector3(1, 3, 2);
private var offsetNeg: Vector3 = new Vector3(-1, 3, -2);

function Start () 
{
	lineRenderer = gameObject.AddComponent(LineRenderer);
   // lineRenderer.material = new Material (Shader.Find("Unlit/Texture"));
    lineRenderer.material = Resources.Load("Materials/Sling", typeof(Material)) as Material;
    lineRenderer.material.mainTexture = elasticTexture;
    lineRenderer.SetWidth(0.5f,0.5f);
    lineRenderer.SetVertexCount(3);
    lineRenderer.SetPosition(0, player.transform.position + offsetPos);
    lineRenderer.SetPosition(1, slingjoin.transform.position);
    lineRenderer.SetPosition(2, player.transform.position +  offsetNeg);
}

function Update () 
{
	/*if(Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
	{
		var hit : RaycastHit;
		var ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		if (!Physics.Raycast (ray, hit, 10000))
			return;

		print(hit.transform.gameObject.tag);

		if(hit.transform.gameObject.tag.Equals("Projectile"))
		{
			focusObj = 	hit.transform.gameObject;
		}
	}*/
	if(Input.GetMouseButtonDown(0))
	{
		focusObj = GameObject.FindGameObjectWithTag("Projectile");
	}
	else if(Input.GetMouseButtonUp(0) ||  (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended))
	{
		focusObj = null;
		
		//apply force to ball proportional to length of elastic and in direction of sling base
		//this.gameObject.AddComponent(Rigidbody);
		//this.gameObject.rigidbody.constraints = RigidbodyConstraints.FreezePositionZ;
		//this.gameObject.rigidbody.mass = 2;
		this.gameObject.rigidbody.useGravity = true;
		var dist = Vector3.Distance(player.transform.position + offsetPos, this.transform.position);
		//var dist = Vector3.Distance(this.transform.position, slingtie1.transform.position);
		var dir = player.transform.position + offsetPos - this.transform.position;
		var ballForce: Vector3 = dir * dist * force;
		if(ballForce.magnitude > maxForce)
			ballForce *= (maxForce/ballForce.magnitude);
		this.rigidbody.AddForce( ballForce );
		lineRenderer.SetPosition(1, player.transform.position + offsetPos);
	}
	else if(focusObj && ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved) || Input.GetMouseButton(0)))
   	{
   		var depth = 38f;//Vector3.Distance(focusObj.transform.position, this.transform.position); //28f;
		var mPos = Camera.main.ScreenToWorldPoint(Vector3(Input.mousePosition.x, Input.mousePosition.y, depth));
		this.transform.position.x = mPos.x;
		this.transform.position.y = mPos.y;
		
		depth = Vector3.Distance(slingjoin.transform.position, player.transform.position);
		if(depth > maxDistance) {
			this.transform.position = Vector3.MoveTowards(player.transform.position, focusObj.transform.position, maxDistance);
		}
		
		//update slign elastic
		lineRenderer.SetPosition(0, player.transform.position + offsetPos);
    	lineRenderer.SetPosition(1, slingjoin.transform.position);
    	lineRenderer.SetPosition(2, player.transform.position + offsetNeg);		
   	}
   	
   	if(this.transform.position.x < -215)
   	{
   		//final position of the camera
   		var targetPos: Vector3 = new Vector3(-390,-24,10);
   		var speed: float = 2.0;
   		Camera.main.transform.position = Vector3.Lerp (
        Camera.main.transform.position, targetPos,
        Time.deltaTime * speed);	
   	}
   	
}