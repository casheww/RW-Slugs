using System;
using SlugBase;

namespace TheChorister
{
    public class ChoristerCat : SlugBaseCharacter
    {
        public ChoristerCat() : base("The Experiment", FormatVersion.V1, 0)
        {

        }

        public override string Description => "My slugcat's description.";

        protected override void Disable() { }
        protected override void Enable() { }

    }
}
