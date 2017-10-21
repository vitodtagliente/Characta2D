using UnityEngine;
using UnityEngine.Events;

namespace Characta2D
{
    [AddComponentMenu("Characta2D/Capability/Dash")]
    public class Dash : Characta2D.CharactaCapability
    {
        public float maxSpeed = 20f;
        public float duration = 0.2f;
        float timer = 0f;
        public string inputButton = "Fire3";
        // activation events
        public UnityEvent OnDashStart = new UnityEvent();
        public UnityEvent OnDashStop = new UnityEvent();

		float lastDirection = 0.0f;
        
        bool canActivate
        {
            get { return timer <= 0f && character.isGrounded; }
        }

        void LateUpdate()
        {
			if (character.desiredMovement.x != 0.0f)
				lastDirection = 1 * Mathf.Sign(character.desiredMovement.x);

			if (isPlayer && canActivate && Input.GetButtonDown(inputButton))
            {
                timer = duration;
                // set the dash speed
				character.velocity.x = lastDirection * maxSpeed;
                OnDashStart.Invoke();
            }

            if (timer > 0f)
            {
                timer -= Time.deltaTime; 
				character.velocity.x = lastDirection * maxSpeed;
                if (timer <= 0f)
                    OnDashStop.Invoke();
            }
        }

		public override void Activate ()
		{
			if (!isPlayer && canActivate) {
				timer = duration;
				character.velocity.x = lastDirection * maxSpeed;
				OnDashStart.Invoke();
			}
		}
    }
}
