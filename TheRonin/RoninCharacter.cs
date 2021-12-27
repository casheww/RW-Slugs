using SlugBase;
using System.Collections.Generic;
using UnityEngine;

namespace TheRonin
{
    class RoninCharacter : SlugBaseCharacter
    {
        public RoninCharacter() : base("The Ronin", FormatVersion.V1, 2)
        {
            PlayerFocusModules = new Dictionary<Player, FocusModule>();
        }

        public override string Description => "Well-versed in combat, the Ronin is agile and can rival foes many times its size.";

        protected override void Enable() { Hooks.Apply(); }
        protected override void Disable() { Hooks.UnApply(); }

        protected override void GetStats(SlugcatStats stats)
        {
            stats.

            stats.bodyWeightFac = 1.1f;
            stats.loudnessFac = 1.1f;

            if (stats.malnourished)
            {
                stats.bodyWeightFac = 0.9f;
                stats.corridorClimbSpeedFac = 0.8f;
                stats.runspeedFac = 0.85f;
                stats.throwingSkill = 0;

                return;
            }

            stats.corridorClimbSpeedFac = Mathf.Lerp(0.9f, 1.3f, FocusModule.Focus);
            stats.lungsFac = Mathf.Lerp(1, 1.8f, FocusModule.Focus);
            stats.runspeedFac = Mathf.Lerp(0.9f, 1.2f, FocusModule.Focus);
            stats.throwingSkill = FocusModule.Focus > 0.75f ? 2 : 1;
        }

        public override Color? SlugcatColor() => new Color(0.64f, 0.64f, 0.58f);


        public static Dictionary<Player, FocusModule> PlayerFocusModules { get; private set; }

    }
}
