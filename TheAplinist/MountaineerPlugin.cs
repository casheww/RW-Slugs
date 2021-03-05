using BepInEx;
using SlugBase;

namespace TheMountaineer
{
    [BepInPlugin("casheww.mountaineer", "The Mountaineer", "0.1.0")]
    class MountaineerPlugin : BaseUnityPlugin
    {
        public MountaineerPlugin()
        {
            PlayerManager.RegisterCharacter(new ClimberCharacter(Info.Metadata.Name));
        }

    }
}
