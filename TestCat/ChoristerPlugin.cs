using BepInEx;
using SlugBase;

namespace TheChorister
{
    [BepInPlugin("casheww.testcat", "TestCat", "0.1.0")]
    class ChoristerPlugin : BaseUnityPlugin
    {
        public ChoristerPlugin()
        {
            PlayerManager.RegisterCharacter();
        }
    }
}
