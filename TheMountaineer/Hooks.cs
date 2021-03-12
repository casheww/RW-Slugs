using UnityEngine;

namespace TheMountaineer
{
    class Hooks
    {
        public static void Apply()
        {
            On.StoryGameSession.ctor += StoryGameSession_ctor;
            On.Creature.SpitOutOfShortCut += Creature_SpitOutOfShortCut;
            On.Player.Update += PlayerWallClimb;
        }

        static void StoryGameSession_ctor(On.StoryGameSession.orig_ctor orig, StoryGameSession self,
                int saveStateNumber, RainWorldGame game)
        {
            orig(self, saveStateNumber, game);

            if (SlugBase.PlayerManager.CurrentCharacter == MountaineerPlugin.CharacterInstance)
            {
                self.characterStats.poleClimbSpeedFac = 1.25f;
                self.characterStats.corridorClimbSpeedFac = 1.4f;
                self.characterStats.loudnessFac = 1.5f;
            }
        }

        static void Creature_SpitOutOfShortCut(On.Creature.orig_SpitOutOfShortCut orig, Creature self,
        RWCustom.IntVector2 pos, Room newRoom, bool spitOutAllSticks)
        {
            orig(self, pos, newRoom, spitOutAllSticks);

            if (!(self is Player player)) return;

            float darkness = newRoom.Darkness(newRoom.MiddleOfTile(self.abstractCreature.pos.Tile));
            Debug.Log($"darkness: {darkness}");
            if (darkness > 0.7f)
            {
                if (player.playerState.slugcatCharacter == MountaineerPlugin.CharacterInstance.SlugcatIndex)
                {
                    Debug.Log("mountaineer bonus speed");
                    player.slugcatStats.runspeedFac = 1.6f;
                }
            }
            else
            {
                player.slugcatStats.runspeedFac = 1f;
            }
        }

        static void PlayerWallClimb(On.Player.orig_Update orig, Player self, bool eu)
        {
            orig(self, eu);

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
                        climbVel = new Vector2(0, -9f);
                        break;

                    case 0:
                    default:
                        climbVel = new Vector2(0, 0.9f);      // upwards velocity to counteract gravity
                        break;
                }

                climbVel.x = input.x;

                foreach (BodyChunk bc in self.bodyChunks)
                {
                    bc.vel = climbVel;
                }

                Climber.ManageClimb(self, input.x, input.y);
            }
        }

    }
}
