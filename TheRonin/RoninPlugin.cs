using BepInEx;
using BepInEx.Configuration;
using UnityEngine;

namespace TheRonin
{
    [BepInPlugin("casheww.ronin", "The Ronin", "0.1.0")]
    class RoninPlugin : BaseUnityPlugin
    {
        public RoninPlugin()
        {
            SlugBase.SlugBaseCharacter character = new RoninCharacter();
            SlugBase.PlayerManager.RegisterCharacter(character);

            _Logger = Logger;
        }

        public static BepInEx.Logging.ManualLogSource _Logger { get; private set; }


        void Awake()
        {
            KeyCodes = new ConfigEntry<KeyCode>[4];

            for (int i = 0; i < KeyCodes.Length; i++)
            {
                KeyCodes[i] = Config.Bind("Keybinds", $"player focus {i}", KeyCode.Backslash);
            }
        }

        public static ConfigEntry<KeyCode>[] KeyCodes { get; private set; }

    }
}
