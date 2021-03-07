using System.Collections.Generic;
using BepInEx;

namespace TheMountaineer
{
    [BepInPlugin("casheww.mountaineer", "The Mountaineer", "0.1.0")]
    class MountaineerPlugin : BaseUnityPlugin
    {
        public MountaineerPlugin()
        {
            SlugBase.SlugBaseCharacter character = new ClimberCharacter(Info.Metadata.Name);
            SlugBase.PlayerManager.RegisterCharacter(character);
            CharacterInstance = character;

            Hooks.Apply();
        }

        public static SlugBase.SlugBaseCharacter CharacterInstance { get; private set; }
    }
}
