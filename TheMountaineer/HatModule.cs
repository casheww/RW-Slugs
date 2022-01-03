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
            Hardhat fromHead = HatSlot;
            if (fromHead != null)
            {
                Debug.Log($"{fromHead.abstractPhysicalObject.ID} off");
                fromHead.wearer = null;
            }
            else Debug.Log("nothing to doff");
            
            HatSlot = null;
            return fromHead;
        }
        
        private void DonHat(int grasp, Hardhat hat)
        {
            player.ReleaseObject(grasp, false);
            Debug.Log($"{hat.abstractPhysicalObject.ID} to head");
            hat.wearer = player;
            HatSlot = hat;
        }
        

        public readonly Player player;
        public int afterDonCounter;
        public Hardhat HatSlot { get; private set; }

    }
}
