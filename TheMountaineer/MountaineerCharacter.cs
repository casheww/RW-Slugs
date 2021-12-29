using RWCustom;
using SlugBase;
using UnityEngine;

namespace TheMountaineer
{
    public class MountaineerCharacter : SlugBaseCharacter
    {
        public MountaineerCharacter(string slugName) : base(slugName, FormatVersion.V1, 0, true) {}

        
        #region SlugBase
        
        public override string Description => "A hardy traveller, this little creature is an expert climber and caver.";
        
        public override bool CanEatMeat(Player player, Creature creature) => true;
        
        public override void GetFoodMeter(out int maxFood, out int foodToSleep)
        {
            maxFood = 8;
            foodToSleep = 6;
        }

        protected override void GetStats(SlugcatStats stats)
        {
            stats.poleClimbSpeedFac = 1.2f;
            stats.corridorClimbSpeedFac = 1.2f;
            stats.loudnessFac = 1.75f;
        }

        public override Color? SlugcatColor(int slugcatCharacter, Color baseColor)
        {
            Color col = new Color(0.75f, 0.7f, 0.55f);
            return slugcatCharacter == -1 ? col : Color.Lerp(baseColor, col, 0.75f);
        }
        public override Color? SlugcatEyeColor(int slugcatCharacter) => new Color(0.13f, 0.14f, 0.12f);

        #endregion SlugBase

        #region Hooks

        protected override void Enable()
        {
            On.Player.Update += Player_Update;
            On.Player.TerrainImpact += Player_TerrainImpact;
        }
        protected override void Disable()
        {
            On.Player.Update -= Player_Update;
            On.Player.TerrainImpact -= Player_TerrainImpact;
        }
        
        private void Player_Update(On.Player.orig_Update orig, Player self, bool eu)
        {
            orig(self, eu);
            if (!IsMe(self)) return;
            
            if (_climbingModule is null)
                _climbingModule = new ClimbingModule(self.graphicsModule as PlayerGraphics);

            if (self.input[0].pckp && self.bodyMode == Player.BodyModeIndex.WallClimb)
            {
                Player.InputPackage input = self.input[0];
                Vector2 climbVel;

                switch (input.y)
                {
                    case 1:
                        climbVel = new Vector2(0, 3f);
                        break;

                    case -1:
                        climbVel = new Vector2(0, -9f);         // I have no idea why this is still so slow
                        break;

                    default:
                        climbVel = new Vector2(0, 0.9f);        // upwards velocity to counteract gravity
                        break;
                }

                climbVel.x = input.x;

                foreach (BodyChunk bc in self.bodyChunks)
                    bc.vel = climbVel;

                _climbingModule.PlayAnimation(input.x, input.y);
            }
        }
        
        private static void Player_TerrainImpact(On.Player.orig_TerrainImpact orig, Player self, int chunk,
            IntVector2 direction, float speed, bool firstContact)
        {
            Debug.LogWarning(speed);
            // new collision speed thresholds
            if (speed > vanillaNonStunSpeed && speed < fatalSpeed)
                speed = speed < stunSpeed ? vanillaNonStunSpeed : vanillaNonFatalSpeed;
            
            orig(self, chunk, direction, speed, firstContact);
        }

        #endregion Hooks


        private ClimbingModule _climbingModule;
        private const float fatalSpeed = 100f;
        private const float vanillaNonFatalSpeed = 59f;
        private const float stunSpeed = 60f;
        private const float vanillaNonStunSpeed = 34f;

    }
}
