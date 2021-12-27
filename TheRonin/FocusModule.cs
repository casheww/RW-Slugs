using System.Collections.Generic;
using UnityEngine;

namespace TheRonin
{
    class FocusModule
    {
        public FocusModule()
        {
            Focus = 1;
            frames = 0;
            unfadeFrames = 0;

            focusState = FocusState.Unfocused;

            stateFrames = new Dictionary<FocusState, int>()
            {
                { FocusState.Unfocused, 20 },
                { FocusState.Readying, 160 },
                { FocusState.Focused, 60 },
                { FocusState.Cooldown, 80 }
            };
        }

        public float Focus
        {
            get => _focus;
            set => _focus = Mathf.Clamp01(value);
        }
        private float _focus;

        public void Update(Player player, bool focusKeyPressed)
        {
            if (focusKeyPressed && player.touchedNoInputCounter > 10 && !UnfadeInProgress)
            {
                unfadeFrames = 0;

                DoFocus(player);
                frames++;
            }
            else
            {
                frames = 0;

                FocusUnfade(player);
                unfadeFrames++;

                // switch to cooldown mode if in focus mode to prevent abuse by not using 100% of the focus time
                focusState = focusState == FocusState.Focused ? FocusState.Cooldown : FocusState.Unfocused;

                Focus += -focusLossCoef * Mathf.Pow(player.aerobicLevel, 2) + passiveFocusRegen;
            }
        }

        void DoFocus(Player player)
        {
            int fMax = stateFrames[focusState];

            if (frames < fMax)
            {
                if (focusState == FocusState.Readying)
                {
                    FocusFade(player, (float)frames / fMax);
                }

                else if (focusState == FocusState.Focused)
                {
                    // increase focus
                    Focus += activeFocusChargePerFrame;

                    // steady breathing
                    player.AerobicIncrease(-0.1f);
                    // prevent player from 'unbreathing' which seriously messes up some playergraphics position stuff
                    player.aerobicLevel = Mathf.Clamp01(player.aerobicLevel);
                }

                else if (focusState == FocusState.Cooldown)
                {
                    FocusUnfade(player);
                }
            }
            else
            {
                frames = 0;
                focusState++;
                if (focusState > FocusState.Cooldown) focusState = FocusState.Unfocused;
            }
        }

        static int frames;
        static FocusState focusState;

        enum FocusState
        {
            Unfocused,
            Readying,
            Focused,
            Cooldown
        }

        readonly Dictionary<FocusState, int> stateFrames;

        const float activeFocusChargePerFrame = 0.01f;
        const float focusLossCoef = 0.02f;
        const float passiveFocusRegen = 0.0001f;


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

        void FocusUnfade(Player player)
        {
            if (effect == null || effect.Alpha == 0) return;

            if (unfadeFrames == 0) unfadeAlphaStart = effect.Alpha;

            FocusFade(player, Mathf.Lerp(unfadeAlphaStart, 0, (float)unfadeFrames / unfadeFrameMax));
        }

        int unfadeFrames;
        const int unfadeFrameMax = 80;
        float unfadeAlphaStart;

        bool UnfadeInProgress => 0 < unfadeFrames && unfadeFrames < unfadeFrameMax;

        FocusFadeEffect effect = null;

    }
}
