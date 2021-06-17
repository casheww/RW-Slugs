using UnityEngine;

namespace TheRonin
{
    class FocusModule
    {
        public FocusModule()
        {
            Focus = 1;
            frames = 0;
            focusState = FocusState.Unfocused;
        }

        public float Focus
        {
            get => _focus;
            set => _focus = Mathf.Clamp01(value);
        }
        private float _focus;

        public void Update(Player player, bool focusKeyPressed)
        {
            if (focusKeyPressed && player.touchedNoInputCounter > 10)
            {
                DoFocus(player);
            }
            else
            {
                frames = 0;
                focusState = FocusState.Unfocused;

                Focus += -focusLossCoef * Mathf.Pow(player.aerobicLevel, 2) + passiveFocusRegen;
            }
        }

        void DoFocus(Player player)
        {
            switch (focusState)
            {
                case FocusState.Unfocused:
                    focusState = FocusState.Readying;
                    break;

                case FocusState.Readying:
                    if (frames >= readyingFrames)
                    {
                        frames = 0;
                        focusState = FocusState.Focusing;
                    }
                    FocusFade(player, (float)frames / readyingFrames);
                    break;

                case FocusState.Focusing:
                    if (frames >= focusingFrames)
                    {
                        frames = 0;
                        focusState = FocusState.Unfocused;
                    }
                    
                    // increase focus
                    Focus += activeFocusChargePerFrame;

                    // steady breathing
                    player.AerobicIncrease(-0.1f);
                    // prevent player from 'unbreathing' which seriously messes up some playergraphics position stuff
                    player.aerobicLevel = Mathf.Clamp01(player.aerobicLevel);
                    break;
            }

            frames++;
        }

        static int frames;
        static FocusState focusState;

        enum FocusState
        {
            Unfocused,
            Readying,
            Focusing,
        }

        const int readyingFrames = 40;
        const int focusingFrames = 80;

        const float activeFocusChargePerFrame = 0.05f;
        const float focusLossCoef = 0.025f;
        const float passiveFocusRegen = 0.0003125f;


        void FocusFade(Player player, float alpha)
        {
            if (effect == null)
            {
                // first call
                effect = new FocusFadeEffect(player);
                player.room.AddObject(effect);
            }
            else if (effect.room != player.room)
            {
                // player has changed room
                effect.RemoveFromRoom();
                player.room.AddObject(effect);
            }

            effect.Alpha = alpha;
        }

        FocusFadeEffect effect = null;

    }
}
