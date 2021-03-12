using System.Collections.Generic;
using BepInEx;

namespace TheMountaineer
{
    [BepInPlugin("casheww.mountaineer", "The Mountaineer", "0.1.0")]
    class MountaineerPlugin : BaseUnityPlugin
    {
        public MountaineerPlugin()
        {
            Climbers = new Dictionary<Player, Climber>();

            SlugBase.SlugBaseCharacter character = new MountaineerCharacter(Info.Metadata.Name);
            SlugBase.PlayerManager.RegisterCharacter(character);
            CharacterInstance = character;

            Hooks.Apply();
        }

        public static SlugBase.SlugBaseCharacter CharacterInstance { get; private set; }
        public static Dictionary<Player, Climber> Climbers { get; set; }
    }
}
