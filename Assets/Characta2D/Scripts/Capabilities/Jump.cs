using UnityEngine;
using UnityEngine.Events;
using System.Collections;

namespace Characta2D
{
    [AddComponentMenu("Characta2D/Capability/Jump")]
    public class Jump : CharactaCapability
    {
        public float jumpTakeOffSpeed = 7;
        public float jumpFallFactor = 0.5f;
        // The input button used to ativate this capability
        public string inputButton = "Jump";
		// how much time the jump should live until the 
		// character should start to fall
		public float AIJumpDuration = 0.5f;
        // On jump event
        public UnityEvent OnJump = new UnityEvent();

        // can the character jump?
        public bool canJump
        {
            get
            {
                return character.isGrounded;
            }
        }
        
        void LateUpdate()
        {
			// if the character is a player
			// it is controlled by the user input
			// the AI, instead, should invoke the Activate method
			if (!isPlayer)
				return;

			if (Input.GetButtonDown (inputButton) && canJump)
            {
                character.physics.velocity.y = jumpTakeOffSpeed;
                OnJump.Invoke();
            }
            else if (Input.GetButtonUp(inputButton))
            {
                if (character.velocity.y > 0)
                {
                    character.physics.velocity.y *= jumpFallFactor;
                }
            }
        }

		public override void Activate ()
		{
			if (!isPlayer && canJump) {
				character.physics.velocity.y = jumpTakeOffSpeed;
				OnJump.Invoke();
				Invoke ("Fall", AIJumpDuration);
			}
		}

		// AI buttonRelease simulation
		void Fall()
		{
			if (character.velocity.y > 0)
				character.physics.velocity.y *= jumpFallFactor;
		}
	}
}
