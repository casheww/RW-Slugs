using TheMountaineer.Equipment;
using UnityEngine;

namespace TheMountaineer
{
    public class HatModule
    {
        public HatModule(Player player)
        {
            this.player = player;
        }

        public void ChangeHat(bool eu)
        {
            if (eu) return;

            Hardhat fromHead = DoffHat();
            int grasp = 0;
            
            for (int i = 0; i < player.grasps.Length; i++)
            {
                if (player.grasps[i]?.grabbed is Hardhat hat)
                {
                    DonHat(i, hat);
                    grasp = i;
                    break;
                }
            }
            
            if (fromHead != null)
                player.SlugcatGrab(fromHead, grasp);
            
            afterDonCounter = 40;
        }

        private Hardhat DoffHat()
        {
            Hardhat fromHead = _hatSlot;
            if (fromHead != null)
            {
                Debug.LogWarning($"{fromHead.abstractPhysicalObject.ID} off");
                fromHead.wearer = null;
            }
            else Debug.LogWarning("nothing to doff");
            
            _hatSlot = null;
            return fromHead;
        }
        
        private void DonHat(int grasp, Hardhat hat)
        {
            player.ReleaseObject(grasp, false);
            Debug.LogWarning($"{hat.abstractPhysicalObject.ID} to head");
            hat.wearer = player;
            _hatSlot = hat;
        }
        

        public readonly Player player;
        public int afterDonCounter;
        private Hardhat _hatSlot;

    }
}
