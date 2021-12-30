using UnityEngine;

namespace TheMountaineer.Equipment
{
    public class Hardhat : PlayerCarryableItem, IDrawable
    {
        public Hardhat(AbstractPhysicalObject abstractPhysicalObject) : base(abstractPhysicalObject)
        {
            bodyChunks = new [] {new BodyChunk(this, 0, Vector2.zero, 0, 0.16f)};
            bodyChunkConnections = new BodyChunkConnection[0];
            airFriction = 0.999f;
            gravity = 0.9f;
            bounce = 0.4f;
            surfaceFriction = 0.3f;
            collisionLayer = 2;
            waterFriction = 0.98f;
            buoyancy = 0.6f;
        }

        public override void PlaceInRoom(Room placeRoom)
        {
            base.PlaceInRoom(placeRoom);
            firstChunk.HardSetPosition(placeRoom.MiddleOfTile(abstractPhysicalObject.pos));
        }

        public override void Update(bool eu)
        {
            base.Update(eu);

            if (wearer == null) return;
            Forbid();

            Vector2 wearerBodyOrientation = wearer.bodyChunks[0].pos - wearer.bodyChunks[1].pos;

            _rotationDeg = RWCustom.Custom.VecToDeg(wearerBodyOrientation);
            firstChunk.pos = wearer.bodyChunks[1].pos + wearerBodyOrientation.normalized * 32f;
        }


        public static readonly Color mainColor = new Color(0.8f, 0.7f, 0.4f);
        private float _rotationDeg;
        public Player wearer;
        
        
        #region Drawable
        
        public void InitiateSprites(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam)
        {
            sLeaser.sprites = new[] {new FSprite("Menu_Symbol_Arrow")};
            AddToContainer(sLeaser, rCam, null);
        }

        public void DrawSprites(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, float timeStacker, Vector2 camPos)
        {
            FSprite s = sLeaser.sprites[0];
            Vector2 v = Vector2.Lerp(firstChunk.lastPos, firstChunk.pos, 0.8f + timeStacker * 0.2f);
            s.SetPosition(v - camPos);
            s.rotation = _rotationDeg;
            
            if (slatedForDeletetion || room != rCam.room)
                sLeaser.CleanSpritesAndRemove();
        }

        public void ApplyPalette(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, RoomPalette palette)
        {
            sLeaser.sprites[0].color = mainColor;
        }

        public void AddToContainer(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, FContainer newContatiner)
        {
            if (newContatiner is null)
                newContatiner = rCam.ReturnFContainer("Items");

            foreach (FSprite sprite in sLeaser.sprites)
            {
                sprite.RemoveFromContainer();
                newContatiner.AddChild(sprite);
            }
        }
        
        #endregion Drawable

    }
}
