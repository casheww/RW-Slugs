using BepInEx;

namespace TheMountaineer
{
    [BepInPlugin("casheww.mountaineer", "The Mountaineer", "0.1.0")]
    class MountaineerPlugin : BaseUnityPlugin
    {
        public MountaineerPlugin()
        {
            SlugBase.SlugBaseCharacter character = new MountaineerCharacter(Info.Metadata.Name);
            SlugBase.PlayerManager.RegisterCharacter(character);
        }
    }
}
