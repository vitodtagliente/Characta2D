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
        
        bool canActivate
        {
            get { return timer <= 0f && character.isGrounded; }
        }

        void LateUpdate()
        {
			if (isPlayer && canActivate && Input.GetButtonDown(inputButton))
            {
                timer = duration;
                // set the dash speed
				character.velocity.x = character.facingDirection * maxSpeed;
                OnDashStart.Invoke();
            }

            if (timer > 0f)
            {
                timer -= Time.deltaTime; 
                character.velocity.x = character.facingDirection * maxSpeed;
                if (timer <= 0f)
                    OnDashStop.Invoke();
            }
        }

		public override void Activate ()
		{
			if (!isPlayer && canActivate) {
				timer = duration;
				character.velocity.x = character.facingDirection * maxSpeed;
				OnDashStart.Invoke();
			}
		}
    }
}
