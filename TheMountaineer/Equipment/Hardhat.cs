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
            bounce = 0.2f;
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

            if (TryChangeRoom())
                return;
            
            UpdateLamp();
            UpdatePosAndRotation();
        }

        private bool TryChangeRoom()
        {
            if (wearer.room == room) return false;

            RemoveFromRoom();
            wearer.room.AddObject(this);
            return true;
        }

        private void UpdateLamp()
        {
            float darkness = room.Darkness(Vector2.zero);
            _activeDischarge = Mathf.Clamp01(maxDischarge * Mathf.Clamp01(darkness - activationDarkness));
            AbstractHat.charge -= _activeDischarge;

            lampBrightness = _activeDischarge;
            if (AbstractHat.charge < lampFlickerThreshold)
            {
                // TODO : flicker
            }
        }

        private void UpdatePosAndRotation()
        {
            Vector2 wearerBodyOrientation = (wearer.bodyChunks[0].pos - wearer.bodyChunks[1].pos).normalized;
            _rotation = RWCustom.Custom.VecToDeg(wearerBodyOrientation);
            firstChunk.pos = wearer.bodyChunks[1].pos + wearerBodyOrientation * headSeparation;
        }

        
        public HardhatAbstract AbstractHat => abstractPhysicalObject as HardhatAbstract;

        public Player wearer;
        public Vector2 anchorPos;
        public float anchorRotation;
        private const float headSeparation = 32f;
        public static readonly Color mainColor = new Color(0.8f, 0.7f, 0.4f);
        private float _rotation;
        
        private float _activeDischarge;
        private const float maxDischarge = 0.005f;
        private const float activationDarkness = 0.7f;

        private float lampBrightness;
        private const float lampFlickerThreshold = 0.2f;


        #region Drawable
        
        public void InitiateSprites(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam)
        {
            sLeaser.sprites = new[]
            {
                new FSprite("Menu_Symbol_Arrow"),
                //new CustomFSprite("Futile_White")
            };
            AddToContainer(sLeaser, rCam, null);
        }

        public void DrawSprites(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, float timeStacker, Vector2 camPos)
        {
            FSprite s = sLeaser.sprites[0];

            Vector2 pos;
            float rot;
            if (wearer != null)
            {
                rot = anchorRotation;
                pos = anchorPos + RWCustom.Custom.DegToVec(rot) * headSeparation;
            }
            else
            {
                pos = Vector2.Lerp(firstChunk.lastPos, firstChunk.pos, timeStacker) - camPos;
                rot = _rotation;
            }

            s.SetPosition(pos);
            s.rotation = rot;
            
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
