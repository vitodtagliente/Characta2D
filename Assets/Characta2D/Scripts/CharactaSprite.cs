using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Characta2D
{
	[AddComponentMenu("Characta2D/Sprite")]
	public class CharactaSprite : Characta2D.CharactaBehaviour {

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
			// flip the sprite if it is necessary
			if (desiredMovement.x < 0f)
				spriteRenderer.flipX = facingRight;
			else if (desiredMovement.x > 0f)
				spriteRenderer.flipX = !facingRight;
		}
	}
}
