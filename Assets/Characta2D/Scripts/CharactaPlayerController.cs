using UnityEngine;

/*
 * Simple Characta2D Player Controller
 * 
 */

namespace Characta2D
{
    [AddComponentMenu("Characta2D/PlayerController")]
    [RequireComponent(typeof(SpriteRenderer))]
    public class CharactaPlayerController : Characta2D.CharactaBehaviour
    {        
        // The movement speed
        public float speed = 7f;
		public string horizontalInputAxis = "Horizontal";
		public string verticalInputAxis = "Vertical";

        SpriteRenderer _spriteRenderer;
        SpriteRenderer spriteRenderer
        {
            get
            {
                if (_spriteRenderer == null)
                    _spriteRenderer = GetComponent<SpriteRenderer>();
                return _spriteRenderer;
            }
        }

		// Is the character looking at right? 
		// this attribute should be used to configure (in Editor) the starting state 
        [SerializeField]
        bool facingRight = true;
        // Return the looking on direction.
		// -1f = left, 1f = right
        public float facingDirection
        {
            get
            {
                if (spriteRenderer.flipX == facingRight)
                    return (facingRight) ? -1f : 1f;
                else return (facingRight) ? 1f : -1f;
            }
        }
        
		// Get the use input
        public virtual void Update()
        {
			desiredMovement.x = Input.GetAxis(horizontalInputAxis);
			desiredMovement.y = Input.GetAxis(verticalInputAxis);
            // update the velocity according to the user input
			velocity.x = desiredMovement.x * speed;

            // flip the sprite if it is necessary
			if (desiredMovement.x < 0f)
                spriteRenderer.flipX = facingRight;
			else if (desiredMovement.x > 0f)
                spriteRenderer.flipX = !facingRight;
        }

    }
}
