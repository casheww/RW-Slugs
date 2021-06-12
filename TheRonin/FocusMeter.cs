using HUD;
using UnityEngine;

namespace TheRonin
{
    class FocusMeter : HudPart
    {
        public FocusMeter(HUD.HUD hud) : base(hud)
        {
            Instance = this;

            this.hud = hud;

            upPos = new Vector2(200, 75);
            restPos = new Vector2(200, 20);

            line = new FSprite("pixel", true)
            {
                scaleX = 1,
                scaleY = 3
            };
            FContainer.AddChild(line);
        }

        const float maxWidth = 200;

        public override void Draw(float timeStacker)
        {
            if (hud == null
                || hud.foodMeter == null
                || !(hud.owner is Player player)
                || !(player.graphicsModule is PlayerGraphics pGraphics)
                || !(SlugBase.PlayerManager.CurrentCharacter is RoninCharacter ronin))
                return;

            // lerp alpha of focus bar based on alpha of food meter... ez clap
            line.alpha = Mathf.Lerp(0.3f, 1, hud.foodMeter.lineSprite.alpha);
            // alpha pusle that follows slugcat's breath
            line.alpha += 0.15f * Mathf.Sin(Mathf.Lerp(pGraphics.lastBreath, pGraphics.breath, timeStacker) * Mathf.PI * 2);

            // scale focus bar width to focus value
            line.scaleX = Mathf.Lerp(0, maxWidth, ronin.FocusModule.Focus);

            // set position
            if (hud.foodMeter.lineSprite.alpha > 0 && frames < translationFrameMax) frames++;
            else if (hud.foodMeter.lineSprite.alpha == 0 && frames >= 0) frames--;

            float smoothedFrameProp = Mathf.Pow((float)frames / translationFrameMax, 3);
            line.SetPosition(Vector2.Lerp(restPos, upPos, smoothedFrameProp));
        }

        public static FocusMeter Instance { get; private set; }

        Vector2 upPos;
        Vector2 restPos;
        int frames = 0;
        const int translationFrameMax = 15;

        FSprite line;

        FContainer FContainer => hud.fContainers[1];

    }
}
