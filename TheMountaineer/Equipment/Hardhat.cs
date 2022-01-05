using UnityEngine;

namespace TheMountaineer.Equipment
{
    public partial class Hardhat : PlayerCarryableItem, IDrawable
    {
        public Hardhat(AbstractPhysicalObject abstractPhysicalObject) : base(abstractPhysicalObject)
        {
            bodyChunks = new [] {new BodyChunk(this, 0, Vector2.zero, 0, 0.16f)};
            bodyChunkConnections = new BodyChunkConnection[0];
            airFriction = 0.999f;
            gravity = 0.9f;
            bounce = 0.2f;
            surfaceFriction = 0.3f;
            collisionLayer = 2;
            waterFriction = 0.98f;
            buoyancy = 0.6f;

            _lamp = new Lamp(firstChunk.pos, new Color(1f, 0.9f, 0.65f), this);
        }

        public override void PlaceInRoom(Room placeRoom)
        {
            base.PlaceInRoom(placeRoom);
            firstChunk.HardSetPosition(placeRoom.MiddleOfTile(abstractPhysicalObject.pos));
            room.AddObject(_lamp);
        }

        public override void Update(bool eu)
        {
            base.Update(eu);

            UpdateLamp();
            
            if (wearer == null) return;
            Forbid();

            if (TryChangeRoom())
                return;
            
            UpdatePosAndRotation();
        }

        private bool TryChangeRoom()
        {
            if (wearer.room == room || wearer.room == null) return false;

            RemoveFromRoom();
            wearer.room.AddObject(this);
            ChangeLampRoom();
            return true;
        }

        private void ChangeLampRoom()
        {
            _lamp.RemoveFromRoom();
            wearer.room.AddObject(_lamp);
        }

        private void UpdateLamp()
        {
            AbstractHat.UpdateCharge(room.Darkness(Vector2.zero));
        }

        private void UpdatePosAndRotation()
        {
            Vector2 wearerBodyOrientation = (wearer.bodyChunks[0].pos - wearer.bodyChunks[1].pos).normalized;
            _rotation = RWCustom.Custom.VecToDeg(wearerBodyOrientation);
            firstChunk.pos = wearer.bodyChunks[1].pos + wearerBodyOrientation * separationFromHead;
        }

        
        public HardhatAbstract AbstractHat => abstractPhysicalObject as HardhatAbstract;

        public Player wearer;
        private Lamp _lamp;

        public Vector2 anchorPos;
        public float anchorRotation;
        private float _rotation;
        private const float separationFromHead = 9f;
        public static readonly Color mainColor = new Color(0.9f, 0.8f, 0.4f);
        
        public Vector2 SpritePos { get; private set; }
        public float SpriteRotation { get; private set; }


        #region Drawable
        
        public void InitiateSprites(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam)
        {
            sLeaser.sprites = new[] {new FSprite("Menu_Symbol_Arrow")};
            AddToContainer(sLeaser, rCam, null);
        }

        public void DrawSprites(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, float timeStacker, Vector2 camPos)
        {
            FSprite s = sLeaser.sprites[0];

            if (wearer != null)
            {
                SpriteRotation = anchorRotation;
                SpritePos = anchorPos + camPos + RWCustom.Custom.DegToVec(anchorRotation) * separationFromHead;
            }
            else
            {
                SpriteRotation = _rotation;
                SpritePos = Vector2.Lerp(firstChunk.lastPos, firstChunk.pos, timeStacker);
            }

            s.SetPosition(SpritePos - camPos);
            s.rotation = SpriteRotation;
            
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
