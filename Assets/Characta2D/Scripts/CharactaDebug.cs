using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Characta2D
{
	public class CharactaDebug : MonoBehaviour {

		// which object to be controlled
		public Characta2D.CharactaObject target;

		void OnGUI()
		{
			if (target == null)
				return;
			
			GUI.Label (new Rect (5,  5, 200, 20),  "CharacterState");
			GUI.Label (new Rect (5, 25, 200, 20),  "IsGrounded: " + target.isGrounded.ToString());
			GUI.Label (new Rect (5, 45, 200, 20),  "IsJumping:  " + target.isJumping.ToString());
			GUI.Label (new Rect (5, 65, 200, 20),  "IsFalling:  " + target.isFalling.ToString());
			GUI.Label (new Rect (5, 85, 200, 20),  "IsSliding:  " + target.isSliding.ToString());
			/*
            GUI.Label (new Rect (5, 105, 200, 20), "IsOnSlope:  " + target.isOnSlope.ToString());
			*/

            GUI.Label (new Rect (Screen.width - 105,  5, 200, 20), "CollisionState");
			GUI.Label (new Rect (Screen.width - 105, 25, 200, 20), "Bottom: " + target.collision.down.ToString());
			GUI.Label (new Rect (Screen.width - 105, 45, 200, 20), "Top:    " + target.collision.up.ToString());
			GUI.Label (new Rect (Screen.width - 105, 65, 200, 20), "Left:   " + target.collision.left.ToString());
			GUI.Label (new Rect (Screen.width - 105, 85, 200, 20), "Right:  " + target.collision.right.ToString());
		}
	}
}
