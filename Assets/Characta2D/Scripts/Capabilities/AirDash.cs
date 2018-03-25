using UnityEngine;
using UnityEngine.Events;

namespace Characta2D
{
    [AddComponentMenu("Characta2D/Capability/AirDash")]
    public class AirDash : Characta2D.CharactaCapability
    {
        public float maxSpeed = 10f;
        public float duration = .2f;
        float timer = 0f;
        public string inputButton = "Fire3";

		// can dash once time at jump
        bool activated = false;

		float lastDirection = 0.0f;

        // dash events
        public UnityEvent OnDashStart = new UnityEvent();
        public UnityEvent OnDashStop = new UnityEvent();

        bool canActivate
        {
            get { return timer <= 0f && (character.isJumping || character.isFalling) && !activated; }
        }

        void LateUpdate()
		{
			if (character.input.x != 0.0f)
				lastDirection = 1 * Mathf.Sign(character.input.x);

			if (isPlayer && canActivate && Input.GetButtonDown(inputButton))
            {
                timer = duration;
                character.EnableGravity(false);
                character.ApplyHorizontalInput(lastDirection * maxSpeed);
                activated = true;
                OnDashStart.Invoke();
            }

            if (timer > 0f)
            {
                timer -= Time.deltaTime;
                character.ApplyHorizontalInput(lastDirection * maxSpeed);
                if (timer <= 0f)
                {
                    character.EnableGravity();
                    OnDashStop.Invoke();                    
                }
            }

            if (character.isGrounded)
                activated = false;
        }

		public override void Activate ()
		{
			if (!isPlayer && canActivate) {
				timer = duration;
                character.EnableGravity(false);
                character.ApplyHorizontalInput(lastDirection * maxSpeed);
                activated = true;
				OnDashStart.Invoke();
			}
		}
    }
}
