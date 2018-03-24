using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Characta2D
{
	[AddComponentMenu("Characta2D/Sprite")]
    [RequireComponent(typeof(Characta2D.CharactaPhysics))]
    public class CharactaSprite : Characta2D.CharactaObject
    {
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
		public void Update()
		{
			// flip the sprite if it is necessary
			if (input.x < 0f)
				spriteRenderer.flipX = facingRight;
			else if (input.x > 0f)
				spriteRenderer.flipX = !facingRight;
		}
	}
}
