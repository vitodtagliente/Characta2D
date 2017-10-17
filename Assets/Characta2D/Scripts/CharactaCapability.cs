using UnityEngine;

/*
 * Plugin style system for Characta
 */ 

namespace Characta2D
{
    [RequireComponent(typeof(Characta2D.CharactaPlayerController))]
    public abstract class CharactaCapability : MonoBehaviour
    {

		CharactaPlayerController _character;
        public CharactaPlayerController character
        {
            get
            {
                if (_character == null)
                    _character = GetComponent<CharactaPlayerController>();
                return _character;
            }
        }

		// set to true if the character is controller by the player
		// false for the AI
		public bool isPlayer = true;

		public virtual void Activate()
		{
		}
	}
}
