using System;
using BepInEx;
using BepInEx.Configuration;
using UnityEngine;

namespace TheMountaineer
{
    [BepInPlugin("casheww.mountaineer", "The Mountaineer", "1.0.0")]
    class MountaineerPlugin : BaseUnityPlugin
    {
        public MountaineerPlugin()
        {
            Instance = this;
            SlugBase.SlugBaseCharacter character = new MountaineerCharacter(Info.Metadata.Name);
            SlugBase.PlayerManager.RegisterCharacter(character);
        }

        private void OnEnable()
        {
            new Fisobs.FisobRegistry(new[] {Equipment.HardhatFisob.instance}).ApplyHooks();
        }

        private void Awake()
        {
            ConfigHatKey = Config.Bind("Controls", "hatKey", KeyCode.Q,
                "The key to use for equipping/unequipping the Mountaineer's hat.");
        }


        public static MountaineerPlugin Instance { get; private set; }
        public ConfigEntry<KeyCode> ConfigHatKey { get; private set; }

    }
}
