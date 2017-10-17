using UnityEngine;
using UnityEngine.Events;

namespace Characta2D
{
    [AddComponentMenu("Characta2D/Capability/DoubleJump")]
	[RequireComponent(typeof(Characta2D.Jump))]
    public class DoubleJump : Characta2D.CharactaCapability
    {
        public float jumpTakeOffSpeed = 5;
        public float jumpFallFactor = 0.5f;
        // this is the max number of jumps that the character can do
		// 1 = double jump, 2 = triple jump, ....
        public int extraJumps = 1;
		int jumpCounter;
		// The input button used to ativate this capability
		public string inputButton = "Jump";
		// how much time the jump should live until the 
		// character should start to fall
		public float AIJumpDuration = 0.3f;
		// On jump event
        public UnityEvent OnJump = new UnityEvent();

        // Indica se il personaggio può saltare
        public bool canJump
        {
            get
            {
				return !character.isGrounded && jumpCounter < extraJumps;
            }
        }

        void LateUpdate()
        {
			if (character.isGrounded)
				jumpCounter = 0;
			
			if (!isPlayer)
				return;

            if (Input.GetButtonDown(inputButton) && canJump)
            {
                jumpCounter++;
                character.velocity.y = jumpTakeOffSpeed;
                OnJump.Invoke();
            }
            else if (Input.GetButtonUp(inputButton))
            {
                if (character.velocity.y > 0)
                {
                    character.velocity.y *= jumpFallFactor;
                }
            }
        }

		public override void Activate ()
		{
			if (!isPlayer && canJump) {
				jumpCounter++;
				character.velocity.y = jumpTakeOffSpeed;
				OnJump.Invoke();
				Invoke ("Fall", AIJumpDuration);
			}
		}

		// AI buttonRelease simulation
		void Fall()
		{
			if (character.velocity.y > 0)
				character.velocity.y *= jumpFallFactor;
		}
    }
}
