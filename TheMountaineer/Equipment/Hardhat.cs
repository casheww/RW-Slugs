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

            _lamp = new Lamp(firstChunk.pos, Color.yellow, this);
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
            if (wearer.room == room) return false;

            RemoveFromRoom();
            wearer.room.AddObject(this);
            _lamp.RemoveFromRoom();
            wearer.room.AddObject(_lamp);
            return true;
        }

        private void UpdateLamp()
        {
            if (AbstractHat.charge > 0) _lamp.stayAlive = true;

            // TODO : discharge -> turn off
            float darkness = room.Darkness(Vector2.zero);
            _activeDischarge = Mathf.Clamp01(maxDischarge * Mathf.Clamp01(darkness - activationDarkness));
            AbstractHat.charge -= _activeDischarge;
        }

        private void UpdatePosAndRotation()
        {
            Vector2 wearerBodyOrientation = (wearer.bodyChunks[0].pos - wearer.bodyChunks[1].pos).normalized;
            _rotation = RWCustom.Custom.VecToDeg(wearerBodyOrientation);
            firstChunk.pos = wearer.bodyChunks[1].pos + wearerBodyOrientation * separationFromHead;
        }

        
        public HardhatAbstract AbstractHat => abstractPhysicalObject as HardhatAbstract;

        public Player wearer;
        public Vector2 anchorPos;
        public float anchorRotation;
        private const float separationFromHead = 9f;
        public static readonly Color mainColor = new Color(0.9f, 0.8f, 0.4f);
        private float _rotation;
        
        private float _activeDischarge;
        private const float maxDischarge = 0.00005f;
        private const float activationDarkness = 0.7f;

        private Lamp _lamp;


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
                DrawRotation = anchorRotation;
                SpritePos = anchorPos + camPos + RWCustom.Custom.DegToVec(anchorRotation) * separationFromHead;
            }
            else
            {
                DrawRotation = _rotation;
                SpritePos = Vector2.Lerp(firstChunk.lastPos, firstChunk.pos, timeStacker);
            }

            s.SetPosition(SpritePos - camPos);
            s.rotation = DrawRotation;
            
            if (slatedForDeletetion || room != rCam.room)
                sLeaser.CleanSpritesAndRemove();
        }

        public Vector2 SpritePos { get; private set; }
        public float DrawRotation { get; private set; }

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
