using TheMountaineer.Fisobs;
using UnityEngine;

namespace TheMountaineer.Equipment
{
    public class HardhatFisob : Fisob
    {
        public HardhatFisob() : base("mountaineer_hardhat")
        {
            IconColor = Hardhat.mainColor;
        }

        public override AbstractPhysicalObject Parse(World world, EntitySaveData saveData)
        {
            return new HardhatAbstract(world, saveData.Pos, saveData.ID)
            {
                charge = float.Parse(saveData.CustomData)
            };
        }

        public override SandboxState GetSandboxState(MultiplayerUnlocks unlocks) => SandboxState.Unlocked;

        public override FisobProperties GetProperties(PhysicalObject forObject) => _properties;

        public static readonly HardhatFisob instance = new HardhatFisob();
        private static readonly HardhatProperties _properties = new HardhatProperties();

    }
}
