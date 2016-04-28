using UnityEngine;
using System.Collections;
//This script converts our input and calculates the forces that need to be applied on the rigidbody from our animator
public class PlayerMovement : MonoBehaviour
{
    //how much we multiply our move speed, we give it a default value of 1 because otherwise it would be 0.
    private float moveSpeed = 1;

    float stationaryTurnSpeed = 180;    //if the character is not moving, how fast he will turn
    float movingTUrnSpeed = 360;        // same as the above but for when the character is moving
    public bool onGround;               //if true the character is on the 

    Vector3 moveInput;                  //the move vector
    float turnAmount;                   //the calculated turn amount to pass to mecanim
    float forwardAmount;                //the calculated forward amount to pass to mecanim
    Vector3 velocity;                   //the 3d velocity of the character

    float jumpPower = 10;               //We will use this on later videos probably

    IComparer rayHitComparer;           //Reference to our IComparer


}
