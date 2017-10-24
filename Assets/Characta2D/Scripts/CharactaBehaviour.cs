using UnityEngine;

/*
	Generic character behavior
*/

namespace Characta2D
{
	/* 
		We need:
			- a rigidbody to let the native physics to work fine with this object
			- a characta collider, which will manage the collision system
	*/
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(CharactaCollider))]
    public abstract class CharactaBehaviour : MonoBehaviour
    {
		/* Define a reference to the rigid body */
        Rigidbody2D _body;
        public Rigidbody2D body
        {
            get
            {
                if (_body == null)
                {
                    _body = GetComponent<Rigidbody2D>();
                    // Initialize it
					// these settings let we to define a custom physics behavior
                    _body.bodyType = RigidbodyType2D.Kinematic;
                    _body.simulated = true;
                    _body.useFullKinematicContacts = true;
                }
                return _body;
            }
        }

		/* Define a reference to the animator */
		Animator _anim;
		public Animator anim
		{
			get
			{
				if (_anim == null)
				{
					SetupAnimator();
				}
				return _anim;
			}
			private set { _anim = value; }
		}

		void SetupAnimator()
		{

			//  Fix Bug: if object does not contain animator, add it
			if (GetComponent<Animator>() != null)
				anim = GetComponent<Animator>();
			else anim = gameObject.AddComponent<Animator>();

			if (anim.avatar != null)
				return;

			// if not found, search avatar in children objects
			foreach (var childAnimator in GetComponentsInChildren<Animator>())
			{
				if (anim != childAnimator)
				{
					anim.avatar = childAnimator.avatar;
					Destroy(childAnimator);
					break;
				}
			}
		}

		/* Define a reference to the collider */
        CharactaCollider _collider;
        public new CharactaCollider collider
        {
            get
            {
                if (_collider == null)
                    _collider = GetComponent<CharactaCollider>();
                return _collider;
            }
        }

        // store the last collision state
		//[HideInInspector]
		public CollisionStateInfo lastCollision = new CollisionStateInfo();
        // store the current collision state
		//[HideInInspector]
        public CollisionStateInfo collision = new CollisionStateInfo();

        // The velocity of this object, which will let the character to move on the scene
		[HideInInspector]
        public Vector2 velocity;
        // how much the character is moving per frame
		[HideInInspector]
		public Vector2 deltaPosition;
        // simply, the direction in which the character is moving on
		[HideInInspector]
		public Vector2 movement;
		// desired movement direction without collision computing
		// set this by user input or AI
		[HideInInspector]
		public Vector2 desiredMovement;
        
        // resulting gravity = gravityModifier * Physics2D.gravity
        public float gravityModifier = 1;
        // When a top collision happens, change the character velocity.y to this one
		// This is a kind of impulse that will be applied on the character
		// every time a top collision happens
        public float upCollisionSpeedModifier = -.5f;
        
        // return true if the character is on the ground
        public bool isGrounded
        {
            get { return collision.bottom; }
        }

        // return true if the character is following down
        public bool isFalling
        {
			// movement.y < 0f means that the character is going down
            get { return isGrounded == false && movement.y < 0f; }
        }

        // return true if the character is sliding on a wall or a generic surface
		// this means that the character is going down while a lateral collision (left or right) happens
        public bool isSliding
        {
            get { return (collision.left || collision.right) && movement.y < 0f; }
        }

        // return true if the character is jumping,
		// which means that it is moving up without a bottom collision enabled
        public bool isJumping
        {
            get { return !isGrounded && !collision.left && !collision.right && movement.y > 0f; }
        }

        // return true if the character is on a slope
        public bool isOnSlope
        {
            get { return collision.bottom && collision.groundNormal.x != 0.0f; }
        }

        // update the state of the character on every fixed framerate frame
        protected virtual void FixedUpdate()
        {
            desiredMovement = Vector2.zero;
            // store the last collision state
            lastCollision = collision;

            // Apply the gravity to the character's velocity
            velocity += gravityModifier * Physics2D.gravity * Time.deltaTime;

            // If the character is grounded,
            // don't let him to move down
            if (isGrounded && velocity.y < 0f)
                velocity.y = 0;

            // If the character has a collision on top,
            // don't let him to move up
            // but apply a movement into the reverse direction
            if (isGrounded == false && collision.top && velocity.y > 0f)
                velocity.y = upCollisionSpeedModifier;

            // Don't let the character to move left (or right)
            // exists a lateral collision along the lateral moving direction
            if ((collision.left && velocity.x < 0) || (collision.right && velocity.x > 0f))
                velocity.x = 0f;

            // Compute the delta position
            deltaPosition = velocity * Time.deltaTime;
            // Store the direction in which the character is moving on
            movement = deltaPosition.normalized;
            
            // Update the collision state
            // deltaPosition will be changed according to the collider behavior
            collision = collider.Check(ref deltaPosition);

            // slope movement
            if (isOnSlope)
            {
				// rotate the character
				float rotationAngle = 180.0f - Vector2.Angle (Vector2.down, collision.groundNormal);
				if (transform.rotation.z != rotationAngle)
					transform.rotation = Quaternion.Euler (0.0f, 0.0f, rotationAngle);

			}

            // Move the character
            body.position = body.position + deltaPosition;

			if (Input.GetKeyDown (KeyCode.T))
				Debug.Log (180.0f - Vector2.Angle (Vector2.down, collision.groundNormal));
        }
    }
}

