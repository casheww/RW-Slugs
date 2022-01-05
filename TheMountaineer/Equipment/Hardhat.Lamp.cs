using UnityEngine;

namespace TheMountaineer.Equipment
{
    public partial class Hardhat
    {
        public class Lamp : LightSource
        {
            public Lamp(Vector2 initPos, Color color, UpdatableAndDeletable obj) :
                base(initPos, false, color, obj)
            {
                requireUpKeep = true;
                setAlpha = 1f;
            }

            public override void Update(bool eu)
            {
                base.Update(eu);
                pos = Hat.SpritePos;
                
                float q = Hat.AbstractHat.charge;
                if (q == 0)
                    return;

                _timer++;
                if (_timer > 180) _timer = 0;
                float t = Mathf.Cos(_timer / 180f * 2 * Mathf.PI);
                
                float strength = q == 0 ? 0 : Mathf.Lerp(0.4f, 1f, q);
                
                setRad = 250 + 50 * t * strength;
                setAlpha = 0.8f + 0.2f * t * strength;
            }


            public Hardhat Hat => tiedToObject as Hardhat;
            
            private int _timer;
        }
    }
}
