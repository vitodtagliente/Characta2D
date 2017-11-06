using UnityEngine;
using System.Collections.Generic;

/*
	Custom collider behavior
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

		// inore some collisions if the collider tag is contained in this list
		[SerializeField]
		List<string> ignoreCollisionTags = new List<string>();

		// rays should be min 3 per direction
		void OnValidate()
		{
			verticalRays = Mathf.Max (verticalRays, 3);
			horizontalRays = Mathf.Max (horizontalRays, 3);
		}

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
            
			Vector3 lastPoint = Vector3.zero;

			// vertical collision
			for (int i = 0; i < verticalRays; i++) {
				float amount = (float)(collider.bounds.size.x - 2 * collisionInMargin) / (verticalRays - 1);

				Vector3 origin = new Vector3 (
					transform.position.x - size.x + collisionInMargin + (i * amount),
					transform.position.y,
					transform.position.z
                );

				// slope management
				if (transform.rotation.z != 0.0f) {
					origin = transform.position + transform.right * (-size.x + collisionInMargin + (i * amount));

					if (lastPoint != Vector3.zero)
						Debug.DrawLine (lastPoint, origin, Color.blue, 0.1f);
					lastPoint = origin;
				}

                // bottom collision
                float hitDistance = collisionDistance.y;
				if(CheckDirection (ref collision, origin, -transform.up, collisionDistance.y, out hitDistance))
                {
                    if (movement.y < 0f && hitDistance < collisionDistance.y)
                    {
                        float deltaHit = hitDistance - size.y;
                        movement.y = -deltaHit;
                    }

					int range = verticalRays / 3;
                    if (i <= range)
                        collision.bottomInfo.left = true;
                    else if (i >= verticalRays - range)
						collision.bottomInfo.right = true;
					else collision.bottomInfo.center = true;
                }				

				// top collision
				if(CheckDirection (ref collision, origin, transform.up, collisionDistance.y, out hitDistance))
                {
                    if (movement.y > 0f && hitDistance < collisionDistance.y)
                    {
                        float deltaHit = hitDistance - size.y;
                        movement.y = deltaHit;
                    }

					int range = verticalRays / 3;
                    if (i <= range)
                        collision.topInfo.left = true;
                    else if (i >= verticalRays - range)
						collision.topInfo.right = true;
					else collision.topInfo.center = true;
                }
            }

			// horizontal collision
			for (int i = 0; i < horizontalRays; i++) {
				float amount = (float)(collider.bounds.size.y - 2 * collisionInMargin) / (horizontalRays - 1);

				Vector3 origin = new Vector3 (
					transform.position.x,
					transform.position.y - size.y + collisionInMargin + (i * amount),
					transform.position.z
				);

                // right collision
                float hitDistance = collisionDistance.x;
				if(CheckDirection (ref collision, origin, transform.right, collisionDistance.x, out hitDistance))
                {
                    if (movement.x > 0f && hitDistance < collisionDistance.x)
                    {
                        float deltaHit = hitDistance - size.x;
                        movement.x = deltaHit;
                    }

					int range = horizontalRays / 3;
					if (i <= range)
                        collision.rightInfo.bottom = true;
					else if (i >= horizontalRays - range)
                        collision.rightInfo.top = true;
                    else collision.rightInfo.center = true;
                }				

				// left collision
				if(CheckDirection (ref collision, origin, -transform.right, collisionDistance.x, out hitDistance))
                {
                    if (movement.x < 0f && hitDistance < collisionDistance.x)
                    {
                        float deltaHit = hitDistance - size.x;
                        movement.x = -deltaHit;
                    }

					int range = horizontalRays / 3;
					if (i <= range)
                        collision.leftInfo.bottom = true;
					else if (i >=
						horizontalRays - range)
                        collision.leftInfo.top = true;
                    else collision.leftInfo.center = true;
                }				
			}

			return collision;
		}

		public Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles) {
			return Quaternion.Euler(angles) * (point - pivot) + pivot;
		}

		// Check for a collision along a specified direction
		bool CheckDirection(ref CollisionStateInfo collision, Vector2 origin, Vector2 direction, float distance, out float hitDistance)
		{
			RaycastHit2D[] hits = Physics2D.RaycastAll(origin, direction, distance);

            bool computeGroundNormal = true;
            hitDistance = distance;

			bool collisionResult = false;

			for (int i = 0; i < hits.Length; i++)
			{
				// skip this collider
				if (hits[i].collider == collider)
					continue;

				// ignore this collision if the tag is stored in exceptions
				if (ignoreCollisionTags.Contains (hits [i].collider.tag))
					continue;

				var normal = hits[i].normal;

				if(direction.y != 0.0f)
                {
					if (normal.y > minNormalY)
						collisionResult = true;
                    else if (normal.y < -minNormalY)
						collisionResult = true;
                }

				if(direction.x != 0.0f)
                {
					if (normal.x > minNormalX)
						collisionResult = true;
					else if (normal.x < -minNormalX)
						collisionResult = true;
                }
				
				Debug.DrawLine(origin, hits[i].point, Color.black, debugLineDuration);

				if (hits[i].distance < distance)
					hitDistance = hits[i].distance;	
                
                // useful for slope detection
				if(direction.y < 0.0f && computeGroundNormal)
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

			return collisionResult;
		}
	}
}
