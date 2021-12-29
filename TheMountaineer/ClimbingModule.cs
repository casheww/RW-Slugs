using UnityEngine;

namespace TheMountaineer
{
    class ClimbingModule
    {
        public ClimbingModule(PlayerGraphics graphics)
        {
            _graphics = graphics;
        }

        public void PlayAnimation(int x, int y)
        {
            foreach (SlugcatHand hand in _graphics.hands)
            {                
                BodyChunk mainBC = _graphics.owner.bodyChunks[0];
                Vector2 goalPos;

                if (_movingHand == hand)
                {
                    // hand is set to move this update

                    goalPos = hand.pos + new Vector2(0, 30f * y);
                    _movingHandUpdateCount--;

                    if (_movingHandUpdateCount == 0)
                    {
                        _movingHand = null;
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
                    if (move && _movingHand is null)
                    {
                        // if this point is reached, this hand will start moving next update
                        _movingHand = hand;
                        _movingHandUpdateCount = movingHandUpdateMax;
                    }
                }

                hand.FindGrip(_graphics.owner.room, hand.pos, hand.pos, 90f, goalPos, 0, 0, true);
            }
        }

        private readonly PlayerGraphics _graphics;
        private SlugcatHand _movingHand;
        private int _movingHandUpdateCount;
        private const int movingHandUpdateMax = 7;

    }
}
