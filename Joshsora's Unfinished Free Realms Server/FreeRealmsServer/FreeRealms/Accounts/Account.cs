using FreeRealms.Characters;

namespace FreeRealms.Accounts
{
    public class Account
    {
        // Account Info
        public uint ID;
        public string Username;
        public uint AccessLevel;
        public bool Banned;

        // World-info
        public PlayableCharacter Character;

        public bool HasCharacter()
        {
            if (Character == null)
            {
                return false;
            }

            return true;
        }
    }
}
