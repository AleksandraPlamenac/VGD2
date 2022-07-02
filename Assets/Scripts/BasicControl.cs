using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
[RequireComponent(typeof(CharacterInputController))]
public class BasicControl : MonoBehaviour
{
    private Rigidbody rbody;
    private CharacterInputController cinput;
    private GameObject holder;
     
    public float forwardMaxSpeed = 1f;
    public float turnMaxSpeed = 1f;
      
    //Useful if you implement jump in the future...
    public float jumpableGroundNormalMaxAngle = 45f;

    public int throwForwardForce = 5;
    public int throwUpForce = 4;
    public int jumpUpForce = 5;

    public bool closeToJumpableGround;

    private int groundContactCount = 0;
    private int jumpCount = 0;


    public bool IsGrounded
    {
        get
        {
            return groundContactCount > 0;
        }
    }


    void Awake()
    {
        rbody = GetComponent<Rigidbody>();
        holder = GameObject.Find("Holder");

        if (rbody == null)
            Debug.Log("Rigid body could not be found");

        cinput = GetComponent<CharacterInputController>();

        if (cinput == null)
            Debug.Log("CharacterInputController could not be found");

    }


    // Use this for initialization
    void Start()
    {
    }


    void Update()
    {

        float inputForward=0f;
        float inputTurn=0f;
      
        if (cinput.enabled)
        {
            inputForward = cinput.Forward;
            inputTurn = cinput.Turn;
        }

        //onCollisionXXX() doesn't always work for checking if the character is grounded from a playability perspective
        //Uneven terrain can cause the player to become technically airborne, but so close the player thinks they're touching ground.
        //Therefore, an additional raycast approach is used to check for close ground

        bool isGrounded = IsGrounded || Common.CheckGroundNear(this.transform.position, jumpableGroundNormalMaxAngle, 0.85f, 0f, out closeToJumpableGround);
        if (isGrounded)
        {
            jumpCount = 0;
        }

        //this.transform.Translate(Vector3.forward * cinput.Forward * Time.deltaTime * forwardMaxSpeed);
        //this.transform.Rotate(Vector3.up, cinput.Turn * Time.deltaTime * turnMaxSpeed);

        //It's supposed to be safe to not scale with Time.deltaTime (e.g. framerate correction) within FixedUpdate()
        //If you want to make that optimization, you can precompute your velocity-based translation using Time.fixedDeltaTime
        //We use rbody.MovePosition() as it's the most efficient and safest way to directly control position in Unity's Physics
        rbody.MovePosition(rbody.position +  this.transform.forward * inputForward * Time.deltaTime * forwardMaxSpeed);
        //Most characters use capsule colliders constrained to not rotate around X or Z axis
        //However, it's also good to freeze rotation around the Y axis too. This is because friction against walls/corners
        //can turn the character. This errant turn is disorienting to players. 
        //Luckily, we can break the frozen Y axis constraint with rbody.MoveRotation()
        //BTW, quaternions multiplied has the effect of adding the rotations together
        rbody.MoveRotation(rbody.rotation * Quaternion.AngleAxis(inputTurn * Time.deltaTime * turnMaxSpeed, Vector3.up));


        var colliders = new Collider[16];
        var size = Physics.OverlapSphereNonAlloc(transform.position, 2, colliders);
        var nearbyBoxes = colliders
            .Where(x => x != null)
            .Where(x => x.CompareTag(Common.RESOURCE_BOX_TAG))
            .ToList();

        if (nearbyBoxes.Any() && holder.transform.childCount == 0)
        {
            var boxList = string.Join(",", nearbyBoxes.Select(x => x.name));
            if (cinput.Action)
            {
                var closestBox = nearbyBoxes[0].gameObject;
                closestBox.tag = Common.RESOURCE_BOX_HELD_TAG;
                closestBox.transform.SetParent(holder.transform);
                closestBox.transform.localPosition = Vector3.zero;

                var boxRb = closestBox.GetComponent<Rigidbody>();
                boxRb.isKinematic = true;
                boxRb.detectCollisions = false;
                boxRb.constraints = RigidbodyConstraints.FreezeAll;
            }
            Debug.Log("Colliders: " + boxList);
            return;
        }

        var nearbyOrderPreps = colliders
            .Where(x => x != null)
            .Where(x => x.CompareTag(Common.ORDER_PREP_TABLE_TAG))
            .ToList();

        //if (nearbyOrderPreps.Any() && holder.transform.childCount == 1)
        if (holder.transform.childCount == 1)
        {
            if (cinput.Action)
            {
                var heldBox = holder.transform.GetChild(0).gameObject;
                heldBox.tag = Common.RESOURCE_BOX_TAG;
                heldBox.transform.SetParent(null);

                var boxRb = heldBox.GetComponent<Rigidbody>();
                boxRb.isKinematic = false;
                boxRb.detectCollisions = true;
                boxRb.constraints = RigidbodyConstraints.None;
                boxRb.velocity = new Vector3(0, 0, 0);
                boxRb.angularVelocity = new Vector3(0, 0, 0);
                boxRb.AddForce(this.transform.forward * throwForwardForce, ForceMode.VelocityChange);
                boxRb.AddForce(this.transform.up * throwUpForce, ForceMode.VelocityChange);
                return;
            }
        }

        if (cinput.Action)
        {
            Debug.Log("[!] Action");
        }

        if (cinput.Jump && jumpCount < 1)
        {
            jumpCount++;
            Debug.Log("[!] Jump");
            rbody.AddForce(this.transform.up * jumpUpForce, ForceMode.VelocityChange);
        }

        // anim.SetFloat("velx", inputTurn); 
        // anim.SetFloat("vely", inputForward);
        // anim.SetBool("isFalling", !isGrounded);
    }

    void FixedUpdate()
    {

    }

    // //This is a physics callback
    // void OnCollisionEnter(Collision collision)
    // {
    //
    //     if (collision.transform.gameObject.tag == "ground")
    //     {
    //         ++groundContactCount;
    //
    //         EventManager.TriggerEvent<MinionLandsEvent, Vector3, float>(collision.contacts[0].point, collision.impulse.magnitude);          
    //     }
				// 		
    // }


    private void OnCollisionStay(Collision collision)
    {
        if (collision.transform.gameObject.tag == "Ground")
        {
            --groundContactCount;
        }
    }
}
