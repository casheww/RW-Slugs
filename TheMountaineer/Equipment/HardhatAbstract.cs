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

        public void UpdateCharge(float darkness)
        {
            if (charge < offThreshold)
            {
                charge = 0;
                return;
            }
            
            float discharge = Mathf.Clamp01(maxDischarge * Mathf.Clamp01(darkness - activationDarkness));
            charge -= discharge;
            Debug.Log($"{charge}\t{discharge}\t{darkness}");
        }

        public override string ToString() => this.SaveToString($"{charge}");


        public float charge;
        public bool HasCharge => charge > 0;
        
        private const float maxDischarge = 5e-4f;
        private const float activationDarkness = 0.7f;
        private const float offThreshold = 1e-3f;

    }
}
