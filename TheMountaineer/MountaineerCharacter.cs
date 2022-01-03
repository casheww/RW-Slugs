using System.Collections.Generic;
using RWCustom;
using SlugBase;
using TheMountaineer.Equipment;
using UnityEngine;

namespace TheMountaineer
{
    public class MountaineerCharacter : SlugBaseCharacter
    {
        public MountaineerCharacter(string slugName) : base(slugName, FormatVersion.V1, 0, true)
        {
            _climbingModules = new List<ClimbingModule>();
            _hatModules = new List<HatModule>();
        }

        
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
            On.PlayerGraphics.DrawSprites += PlayerGraphics_DrawSprites;
        }
        protected override void Disable()
        {
            On.Player.Update -= Player_Update;
            On.Player.TerrainImpact -= Player_TerrainImpact;
            On.PlayerGraphics.DrawSprites -= PlayerGraphics_DrawSprites;
        }
        
        private void Player_Update(On.Player.orig_Update orig, Player self, bool eu)
        {
            orig(self, eu);
            if (!IsMe(self)) return;

            if (Input.GetKeyDown(KeyCode.T))
            {
                HardhatAbstract ahh = new HardhatAbstract(self.abstractPhysicalObject.world,
                    self.abstractPhysicalObject.pos, self.abstractPhysicalObject.world.game.GetNewID());
                ahh.RealizeInRoom();
            }
            
            ClimbingModule cm = GetClimbingModule(self);
            HatModule hm = GetHatModule(self);
            
            if (cm.afterClimbingCounter > 0)
                cm.afterClimbingCounter--;
            if (hm.afterDonCounter > 0)
                hm.afterDonCounter--;
            
            if (self.input[0].pckp && self.bodyMode == Player.BodyModeIndex.WallClimb)
                cm.Climb();

            hatKeyPressed = Input.GetKey(MountaineerPlugin.Instance.ConfigHatKey.Value);
            if (!hatKeyPressed) hatKeyWasReleased = true;
            
            if (hatKeyPressed && hatKeyWasReleased &&
                cm.afterClimbingCounter == 0 && hm.afterDonCounter == 0)
            {
                hm.ChangeHat(eu);
                hatKeyWasReleased = false;
            }

        }

        private static void Player_TerrainImpact(On.Player.orig_TerrainImpact orig, Player self, int chunk,
            IntVector2 direction, float speed, bool firstContact)
        {
            // new collision speed thresholds
            if (speed > vanillaNonStunSpeed && speed < fatalSpeed)
                speed = speed < stunSpeed ? vanillaNonStunSpeed : vanillaNonFatalSpeed;
            
            orig(self, chunk, direction, speed, firstContact);
        }

        private void PlayerGraphics_DrawSprites(On.PlayerGraphics.orig_DrawSprites orig, PlayerGraphics self,
            RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, float timeStacker, Vector2 camPos)
        {
            orig(self, sLeaser, rCam, timeStacker, camPos);
            
            Hardhat hat = GetHatModule(self.owner as Player).HatSlot;
            if (hat == null) return;
            hat.anchorPos = sLeaser.sprites[3].GetPosition();
            hat.anchorRotation = sLeaser.sprites[3].rotation;
        }
        
        #endregion Hooks

        
        private ClimbingModule GetClimbingModule(Player player)
        {
            foreach (ClimbingModule cm in _climbingModules)
                if (cm.player == player)
                    return cm;

            ClimbingModule c = new ClimbingModule(player);
            _climbingModules.Add(c);
            return c;
        }

        private HatModule GetHatModule(Player player)
        {
            foreach (HatModule hm in _hatModules)
                if (hm.player == player)
                    return hm;

            HatModule h = new HatModule(player);
            _hatModules.Add(h);
            return h;
        }
        

        private readonly List<ClimbingModule> _climbingModules;
        private readonly List<HatModule> _hatModules;

        private bool hatKeyPressed;
        private bool hatKeyWasReleased;

        private const float fatalSpeed = 100f;
        private const float vanillaNonFatalSpeed = 59f;
        private const float stunSpeed = 60f;
        private const float vanillaNonStunSpeed = 34f;

        

    }
}
