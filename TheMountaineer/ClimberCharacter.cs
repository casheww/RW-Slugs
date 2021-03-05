using System;
using SlugBase;
using UnityEngine;

namespace TheMountaineer
{
    public class ClimberCharacter : SlugBaseCharacter
    {
        public ClimberCharacter(string slugName) : base(slugName, FormatVersion.V1, 0)
        {
        }

        public override string Description => 
            "While struggling in the dazzling light of day, this creature is a hardy climber and caver.";

        protected override void Disable() { }
        protected override void Enable() { }

        public override bool CanEatMeat(Player player, Creature creature) => true;

        public override void GetFoodMeter(out int maxFood, out int foodToSleep)
        {
            maxFood = 9;
            foodToSleep = 4;
        }

        public override Color? SlugcatColor() => new Color(0.66f, 0.61f, 0.55f);
        public override Color? SlugcatEyeColor() => new Color(0.13f, 0.14f, 0.12f);
    }
}
