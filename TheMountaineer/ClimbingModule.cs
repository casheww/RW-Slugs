using UnityEngine;

namespace TheMountaineer
{
    class ClimbingModule
    {
        public ClimbingModule(PlayerGraphics graphics)
        {
            this.graphics = graphics;
        }

        PlayerGraphics graphics;

        public static void ManageClimb(Player player, int x, int y)
        {
            if (!MountaineerPlugin.Climbers.ContainsKey(player))
            {
                MountaineerPlugin.Climbers.Add(player, new ClimbingModule(player.graphicsModule as PlayerGraphics));
            }

            if (y == 0) return;

            MountaineerPlugin.Climbers[player].PlayAnimation(x, y);
        }

        public void PlayAnimation(int x, int y)
        {
            foreach (SlugcatHand hand in graphics.hands)
            {                
                BodyChunk mainBC = graphics.owner.bodyChunks[0];
                Vector2 goalPos;

                if (movingHand == hand)
                {
                    // hand is set to move this update

                    goalPos = hand.pos + new Vector2(0, 30f * y);
                    movingHandUpdateCount--;

                    if (movingHandUpdateCount == 0)
                    {
                        movingHand = null;
                    }
                }
                else
                {
                    // keep hand stationary with respect to the wall
                    goalPos = hand.pos;

                    // is hand far enough away from head to need to be moved?
                    bool move = (y == 1 && hand.pos.y - mainBC.pos.y < -5f) ||
                                (y == -1 && hand.pos.y - mainBC.pos.y > 5f);

                    // ... and is the other hand idle?
                    if (move && movingHand is null)
                    {
                        // if this point is reached, this hand will start moving next update
                        movingHand = hand;
                        movingHandUpdateCount = movingHandUpdateMax;
                    }
                }

                hand.FindGrip(graphics.owner.room, hand.pos, hand.pos, 90f, goalPos, 0, 0, true);
            }
        }

        SlugcatHand movingHand = null;
        int movingHandUpdateCount;
        const int movingHandUpdateMax = 7;

    }
}
