using TheMountaineer.Fisobs;
using UnityEngine;

namespace TheMountaineer.Equipment
{
    public class HardhatAbstract : AbstractPhysicalObject
    {
        public HardhatAbstract(World world, WorldCoordinate pos, EntityID id) :
            base(world, HardhatFisob.instance.Type, null, pos, id)
        {
            charge = 1f - Random.value * 0.1f;
        }

        public override void Realize()
        {
            if (realizedObject is null)
                realizedObject = new Hardhat(this);
            
            base.Realize();
        }

        public override string ToString() => this.SaveToString($"{charge}");

        
        public float charge;
    }
}
