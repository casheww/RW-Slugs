using System;
using UnityEngine;

namespace TheRonin
{
    class FocusFadeEffect : UpdatableAndDeletable, IDrawable
    {
        public FocusFadeEffect(Player player)
        {
            this.player = player;
            _alpha = 0;
        }

        readonly Player player;

        public float Alpha
        {
            get => _alpha;
            set => _alpha = Mathf.Clamp01(value);
        }
        private float _alpha;

        public void AddToContainer(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, FContainer newContatiner)
        {
            foreach (FSprite s in sLeaser.sprites)
            {
                s.RemoveFromContainer();
                rCam.ReturnFContainer("Bloom").AddChild(s);
            }
        }

        public void ApplyPalette(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, RoomPalette palette) { }

        public void DrawSprites(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, float timeStacker, Vector2 camPos)
        {
            // centre gradient sprite
            Vector2 pos = player.mainBodyChunk.pos - camPos;
            FSprite cSprite = sLeaser.sprites[centreSpriteIndex];
            cSprite.SetPosition(pos);
            cSprite.alpha = Alpha;

            FSprite s;

            float centreMinX = pos.x - cSprite.width / 2;
            float centreMaxX = pos.x + cSprite.width / 2;
            float centreMinY = pos.y - cSprite.height / 2;
            float centreMaxY = pos.y + cSprite.height / 2;

            // top sprite - spans width of window
            s = sLeaser.sprites[0];
            s.scaleX = rCam.sSize.x + 1;
            s.scaleY = rCam.sSize.y - centreMaxY;
            s.SetPosition(rCam.sSize.x / 2, Mathf.Lerp(centreMaxY, rCam.sSize.y, 0.5f));
            s.alpha = Alpha;

            // left sprite - same height as centre
            s = sLeaser.sprites[1];
            s.scaleX = centreMinX;
            s.scaleY = cSprite.height;
            s.SetPosition(Mathf.Lerp(0, centreMinX, 0.5f), pos.y);
            s.alpha = Alpha;

            // right sprite - same height as centre
            s = sLeaser.sprites[2];
            s.scaleX = rCam.sSize.x - centreMaxX;
            s.scaleY = cSprite.width;
            s.SetPosition(Mathf.Lerp(centreMaxX, rCam.sSize.x, 0.5f), pos.y);
            s.alpha = Alpha;

            // bottom sprite - spans width of window
            s = sLeaser.sprites[3];
            s.scaleX = rCam.sSize.x + 1;
            s.scaleY = centreMinY;
            s.SetPosition(rCam.sSize.x / 2, Mathf.Lerp(0, centreMinY, 0.5f));
            s.alpha = Alpha;

        }

        public void InitiateSprites(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam)
        {
            // load the fade texture if not already loaded
            if (Futile.atlasManager.GetAtlasWithName("focusFade") == null)
            {
                Texture2D overlay = new Texture2D(1, 1);
                overlay.LoadImage(Resources.FocusGradient);

                HeavyTexturesCache.LoadAndCacheAtlasFromTexture("focusFade", overlay);
            }

            sLeaser.sprites = new FSprite[5];

            // create overlay sprites
            for (int i = 0; i < sLeaser.sprites.Length; i++)
            {
                sLeaser.sprites[i] = i == centreSpriteIndex ? new FSprite("focusFade") : new FSprite("pixel");
                sLeaser.sprites[i].color = Color.black;
            }

            AddToContainer(sLeaser, rCam, null);
        }

        // 4 "pixel" and then 1 "focusFade"
        const int centreSpriteIndex = 4;

    }
}
