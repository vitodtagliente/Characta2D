using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Characta2D
{
    [AddComponentMenu("Characta2D/Object")]
    [RequireComponent(typeof(Characta2D.CharactaPhysics))]
    public class CharactaObject : MonoBehaviour
    {

        Characta2D.CharactaPhysics _physics;
        public Characta2D.CharactaPhysics physics
        {
            get
            {
                if (_physics == null)
                    _physics = GetComponent<Characta2D.CharactaPhysics>();
                return _physics;
            }
        }

        SpriteRenderer _spriteRenderer;
        public SpriteRenderer spriteRenderer
        {
            get
            {
                if (_spriteRenderer == null)
                    _spriteRenderer = GetComponent<SpriteRenderer>();
                return _spriteRenderer;
            }
        }

        // return true if the character is on the ground
        public bool isGrounded
        {
            get { return physics.collision.down; }
        }

        // return true if the character is following down
        public bool isFalling
        {
            // movement.y < 0f means that the character is going down
            get { return isGrounded == false && physics.movement.y < 0f; }
        }

        // return true if the character is sliding on a wall or a generic surface
        // this means that the character is going down while a lateral collision (left or right) happens
        public bool isSliding
        {
            get { return (physics.collision.left || physics.collision.right) && physics.movement.y < 0f; }
        }

        // return true if the character is jumping,
        // which means that it is moving up without a bottom collision enabled
        public bool isJumping
        {
            get { return !isGrounded && !physics.collision.left && !physics.collision.right && physics.movement.y > 0f; }
        }

        // input movement
        public Vector2 input
        {
            get { return physics.input; }
        }

        public Vector2 velocity
        {
            get { return physics.velocity; }
        }      
        
        public Characta2D.CollisionStateInfo collision
        {
            get { return physics.collision; }
        }

        public void ApplyInput(float horizontal, float vertical)
        {
            physics.input = new Vector2(horizontal, vertical);
        }

        public void ApplyHorizontalInput(float value)
        {
            physics.input.x = value;
        }

        public void ApplyVerticalInput(float value)
        {
            physics.input.y = value;
        }

        public void EnableGravity(bool value = true)
        {
            physics.gravityEnabled = value;
        }
    }
}
