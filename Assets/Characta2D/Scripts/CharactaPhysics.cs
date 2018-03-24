using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Characta2D
{
    [AddComponentMenu("Characta2D/Physics")]
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(BoxCollider2D))]
    public class CharactaPhysics : MonoBehaviour
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

        /* this is based on a box collider, which can be configured (in the editor) to fit the sprite */
        BoxCollider2D _collider;
        public new BoxCollider2D collider
        {
            get
            {
                if (_collider == null)
                    _collider = GetComponent<BoxCollider2D>();
                return _collider;
            }
        }

        // how much rays we have to use for collision detection
        [SerializeField]
        int verticalRays = 4;
        [SerializeField]
        int horizontalRays = 6;

        // The velocity of this object, which will let the character to move on the scene
        //[HideInInspector]
        public Vector2 velocity;
        // how much the character is moving per frame
        //[HideInInspector]
        public Vector2 deltaPosition;
        // simply, the direction in which the character is moving on
        //[HideInInspector]
        public Vector2 movement;
        // desired movement direction without collision computing
        //[HideInInspector]
        public Vector2 input;

        // resulting gravity = gravityModifier * Physics2D.gravity
        public float gravityModifier = 1;
        // When a top collision happens, change the character velocity.y to this one
        // This is a kind of impulse that will be applied on the character
        // every time a top collision happens
        public float upCollisionSpeedModifier = -.5f;
        // This represents the margin outside/inside the collider box bounds
        public Vector3 boundsMargin = Vector3.zero;
        // Horizontal collision check in bounds margin
        public float horizontalMargin = 0.2f;

        // store the last collision state
        [HideInInspector]
        public Characta2D.CollisionStateInfo lastCollision = new CollisionStateInfo();
        // store the current collision state
        //[HideInInspector]
        public Characta2D.CollisionStateInfo collision = new CollisionStateInfo();

        struct RaycastOrigins
        {
            public Vector2 topLeft, topRight;
            public Vector2 bottomLeft, bottomRight;
        };

        RaycastOrigins origins;

        public void Awake()
        {
            verticalRays = Mathf.Max(verticalRays, 3);
            horizontalRays = Mathf.Max(horizontalRays, 3);
        }

        public void FixedUpdate()
        {            
            // store the last collision state
            lastCollision = collision;

            // Apply input
            velocity.x = input.x;
            // Apply the gravity to the character's velocity
            velocity += gravityModifier * Physics2D.gravity * Time.deltaTime;

            // If the character is grounded,
            // don't let him to move down
            if (collision.down && velocity.y < 0f)
                velocity.y = 0;

            // If the character has a collision on top,
            // don't let him to move up
            // but apply a movement into the reverse direction
            if (collision.down == false && collision.up && velocity.y > 0f)
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
            CheckCollisions(ref deltaPosition);

            // check integrity
            if (lastCollision.IsInvalid && collision.IsInvalid)
                collision.Clear();

            // Move the character
            body.position = body.position + deltaPosition;

            // reset the input
            input = Vector2.zero;
        }

        private void CheckCollisions(ref Vector2 deltaPosition)
        {
            UpdateOrigins();
            collision.Clear();
            HorizontalCollision(Vector2.right, ref deltaPosition);
            HorizontalCollision(Vector2.left, ref deltaPosition);
            VerticalCollision(Vector2.down, ref deltaPosition);
            VerticalCollision(Vector2.up, ref deltaPosition);
        }

        private void HorizontalCollision(Vector2 raysDirection, ref Vector2 deltaPosition)
        {
            float amount = (float)(collider.bounds.size.y - horizontalMargin * 2) / (horizontalRays - 1);
            var origin = origins.bottomRight;
            if (raysDirection == Vector2.left)
                origin = origins.bottomLeft;
            origin.y += horizontalMargin;

            var distance = Mathf.Abs(deltaPosition.x);

            for (int i = 0; i < verticalRays; i++)
            {
                Debug.DrawRay(origin, raysDirection * distance, Color.magenta);

                RaycastHit2D[] hits = Physics2D.RaycastAll(origin, raysDirection, distance);

                foreach (var hit in hits)
                {
                    // ignore this collider
                    if (hit.collider == collider)
                        continue;

                    // ignore this collision if the tag is stored in exceptions
                    //if (ignoreCollisionTags.Contains(hits[i].collider.tag))
                    //    continue;

                    if (raysDirection == Vector2.right)
                        collision.right = true;
                    else collision.left = true;

                    if (Mathf.Sign(deltaPosition.x) != Mathf.Sign(raysDirection.x))
                        return;
                    distance -= distance - hit.distance;
                }

                origin.y += amount;
            }

            deltaPosition.x = Mathf.Sign(deltaPosition.x) * distance;
        }

        private void VerticalCollision(Vector2 raysDirection, ref Vector2 deltaPosition)
        {
            float amount = (float)(collider.bounds.size.x) / (verticalRays - 1);
            var origin = origins.bottomLeft;
            if(raysDirection == Vector2.up)
                origin = origins.topLeft;

            var distance = Mathf.Abs(deltaPosition.y);

            for (int i = 0; i < verticalRays; i++)
            {
                Debug.DrawRay(origin, raysDirection * distance, Color.magenta);

                RaycastHit2D[] hits = Physics2D.RaycastAll(origin, raysDirection, distance);

                foreach(var hit in hits)
                {
                    // ignore this collider
                    if (hit.collider == collider)
                        continue;

                    // ignore this collision if the tag is stored in exceptions
                    //if (ignoreCollisionTags.Contains(hits[i].collider.tag))
                    //    continue;
                                       
                    if (raysDirection == Vector2.up)
                        collision.up = true;
                    else collision.down = true;

                    if (Mathf.Sign(deltaPosition.y) != Mathf.Sign(raysDirection.y))
                        return;
                    distance -= distance - hit.distance;
                }

                origin.x += amount;
            }

            deltaPosition.y = Mathf.Sign(deltaPosition.y) * distance;
        }

        private void UpdateOrigins()
        {
            var bounds = collider.bounds;
            bounds.Expand(boundsMargin);
            origins.bottomLeft = bounds.min;
            origins.topRight = bounds.max;
            origins.bottomRight = new Vector3(bounds.max.x, bounds.min.y, bounds.min.z);
            origins.topLeft = new Vector3(bounds.min.x, bounds.max.y, bounds.max.z);
        }
    }
}
