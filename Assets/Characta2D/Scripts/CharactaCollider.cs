using UnityEngine;

/*
	Custom collider behaviour
*/

namespace Characta2D
{
	[AddComponentMenu("Characta2D/Collider")]
	[RequireComponent(typeof(BoxCollider2D))]
	public class CharactaCollider : MonoBehaviour
	{
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

		// This represents the margin outside the collider box bounds
		public float outBoundsMargin = 0.01f;
		// This represents the margin inside the collider box bounds
		public float inBoundsMargin = 0.01f;
		// min normal along the y axis that can define a vertical collision (top or bottom)
		public float minNormalX = .8f;
		// the same for left, right
		public float minNormalY = .8f;
		// how much time ray should be visible in the editor
		private readonly float debugLineDuration = 0.01f;

		// how much rays we have to use for collision detection
		[SerializeField]
		int verticalRays = 4;
		[SerializeField]
		int horizontalRays = 6;

		public CollisionStateInfo Check(ref Vector2 movement)
		{
			Vector2 position2D = transform.position;
			var size = collider.bounds.size / 2;
			// get the direction in which the character will move on
			var direction = movement.normalized;

			// the collision distance cannot be 0
			float collisionMargin = Mathf.Max(.01f, outBoundsMargin);
			float collisionInMargin = Mathf.Max (.01f, inBoundsMargin);
			Vector2 collisionDistance = new Vector2 (
                size.x + Mathf.Abs (movement.x) + collisionMargin,
                size.y + Mathf.Abs (movement.y) + collisionMargin
            );

			// create the object in which we can store
			// the current collision state
			var collision = new CollisionStateInfo();

			// vertical collision
			for (int i = 0; i <= verticalRays; i++) {
				float amount = (float)(collider.bounds.size.x - 2 * collisionInMargin) / verticalRays;

				Vector3 origin = new Vector3 (
					transform.position.x - size.x + collisionInMargin + (i * amount),
					transform.position.y,
					transform.position.z
                );

				// bottom collision
				float hitDistance = CheckDirection (ref collision, origin, Vector2.down, collisionDistance.y, Color.black);
				if (movement.y < 0f && hitDistance < collisionDistance.y) {
					float deltaHit = hitDistance - size.y - collisionMargin;
					movement.y = -deltaHit;
				}

				// top collision
				hitDistance = CheckDirection (ref collision, origin, Vector2.up, collisionDistance.y, Color.black);
				if (movement.y > 0f && hitDistance < collisionDistance.y) {
					float deltaHit = hitDistance - size.y - collisionMargin;
					movement.y = deltaHit;
				}
			}

			// horizontal collision
			for (int i = 0; i <= horizontalRays; i++) {
				float amount = (float)(collider.bounds.size.y - 2 * collisionInMargin) / horizontalRays;

				Vector3 origin = new Vector3 (
					transform.position.x,
					transform.position.y - size.y + collisionInMargin + (i * amount),
					transform.position.z
				);

				// right collision
				float hitDistance = CheckDirection (ref collision, origin, Vector2.right, collisionDistance.x, Color.black);
				if (movement.x > 0f && hitDistance < collisionDistance.x) {
					float deltaHit = hitDistance - size.x;
					movement.x = deltaHit;
				}

				// left collision
				hitDistance = CheckDirection (ref collision, origin, Vector2.left, collisionDistance.x, Color.black);
				if (movement.x < 0f && hitDistance < collisionDistance.x) {
					float deltaHit = hitDistance - size.x;
					movement.x = -deltaHit;
				}
			}

			// Integrity check
			CheckIntegrity (ref collision);

			return collision;
		}

		// Check for a collision along a specified direction
		float CheckDirection(ref CollisionStateInfo collision, Vector2 origin, Vector2 direction, float distance, Color color)
		{
			RaycastHit2D[] hits = Physics2D.RaycastAll(origin, direction, distance);

            bool computeGroundNormal = true;

			for (int i = 0; i < hits.Length; i++)
			{
				// skip this collider
				if (hits[i].collider == collider)
					continue;

				var normal = hits[i].normal;

				if (normal.y > minNormalY)
					collision.bottom = true;
				else if (normal.y < -minNormalY)
					collision.top = true;

				if (normal.x > minNormalX)
					collision.left = true;
				else if (normal.x < -minNormalX)
					collision.right = true;
				
				Debug.DrawLine(origin, hits[i].point, color, debugLineDuration);

				if (hits[i].distance < distance)
					distance = hits[i].distance;	
                
                // useful for slope detection
                if(direction == Vector2.down && computeGroundNormal)
                {
                    if (collision.groundNormal == Vector2.zero)
                        collision.groundNormal = normal;
                    else
                    {
                        if (Mathf.Sign(collision.groundNormal.x) != Mathf.Sign(normal.x) && collision.groundNormal.x != 0.0f) {
                            collision.groundNormal = (collision.bottom) ? Vector2.up : Vector2.zero;
                            computeGroundNormal = false;
                        }
                        else
                        {
                            if (Mathf.Abs(normal.x) > Mathf.Abs(collision.groundNormal.x))
                                collision.groundNormal = normal;
                        }
                    }
                }			
			}
			return distance;
		}

		void CheckIntegrity(ref CollisionStateInfo collision)
		{
			if (collision.isInvalid)
				collision.Clear ();	
		}
	}
}
