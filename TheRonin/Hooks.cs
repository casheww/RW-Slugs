using UnityEngine;

namespace TheRonin
{
    class Hooks
    {
        public static void Apply()
        {
            On.Player.Update += Player_Update;
            On.HUD.HUD.InitSinglePlayerHud += HUD_InitSinglePlayerHud;
        }

        public static void UnApply()
        {
            On.Player.Update -= Player_Update;
            On.HUD.HUD.InitSinglePlayerHud -= HUD_InitSinglePlayerHud;
        }


        private static void Player_Update(On.Player.orig_Update orig, Player self, bool eu)
        {
            orig(self, eu);

            // only process further for ronin characters
            if (!(SlugBase.PlayerManager.UsingCustomCharacter
                && SlugBase.PlayerManager.CurrentCharacter is RoninCharacter ronin)) return;

            ronin.FocusModule.Update(self, Input.GetKey(KeyCode.Backslash));
        }

        private static void HUD_InitSinglePlayerHud(On.HUD.HUD.orig_InitSinglePlayerHud orig, HUD.HUD self, RoomCamera cam)
        {
            orig(self, cam);

            self.AddPart(new FocusMeter(self));

            if (cam.room.abstractRoom.shelter)
            {
                //FocusMeter.Instance.fade = 1;
            }
        }

    }
}
