
namespace TheMountaineer.Equipment
{
    public class HardhatProperties : Fisobs.FisobProperties
    {
        public override void GetGrabability(Player player, ref Player.ObjectGrabability grabability)
            => grabability = Player.ObjectGrabability.OneHand;

        public override void GetScavCollectibleScore(Scavenger scav, ref int score)
            => score = 5;
    }
}
