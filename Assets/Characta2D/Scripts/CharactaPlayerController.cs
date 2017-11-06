using UnityEngine;

/*
 * Simple Characta2D Player Controller
 * 
 */

namespace Characta2D
{
    [AddComponentMenu("Characta2D/PlayerController")]
    [RequireComponent(typeof(SpriteRenderer))]
	public class CharactaPlayerController : Characta2D.CharactaSprite
    {        
        // The movement speed
        public float speed = 7f;
		public string horizontalInputAxis = "Horizontal";
		public string verticalInputAxis = "Vertical";
		        
		// Get the use input
		public override void Update()
        {
			desiredMovement.x = Input.GetAxis(horizontalInputAxis);
			desiredMovement.y = Input.GetAxis(verticalInputAxis);
            // update the velocity according to the user input
			velocity.x = desiredMovement.x * speed;

			base.Update ();
        }

    }
}
