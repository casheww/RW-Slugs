using BepInEx;

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

    }
}
