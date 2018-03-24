using UnityEngine;

/*
 * Plugin style system for Characta
 */ 

namespace Characta2D
{
	[RequireComponent(typeof(Characta2D.CharactaObject))]
    public abstract class CharactaCapability : MonoBehaviour
    {

		Characta2D.CharactaObject _character;
		public Characta2D.CharactaObject character
        {
            get
            {
                if (_character == null)
					_character = GetComponent<Characta2D.CharactaObject>();
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
