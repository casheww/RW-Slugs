using UnityEngine;

namespace TheRonin
{
    class FocusModule
    {
        public FocusModule()
        {
            Focus = 1;
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
                Focus += -0.025f * Mathf.Pow(player.aerobicLevel, 2) + 0.0003125f;
            }
        }

        void DoFocus(Player player)
        {
            switch (focusState)
            {
                case FocusState.Unfocused:
                    focusState = FocusState.Readying;
                    origRoomFades = player.room.roomSettings.fadePalette.fades;
                    break;

                case FocusState.Readying:
                    if (frames >= readyingFrames)
                    {
                        focusState = FocusState.Focusing;
                        frames = 0;
                    }
                    FocusFade(player);
                    break;

                case FocusState.Focusing:
                    if (frames >= focusingFrames)
                    {
                        focusState = FocusState.Unfocused;
                        frames = 0;
                        player.room.roomSettings.fadePalette.fades = origRoomFades;
                    }
                    
                    Focus += activeFocusChargePerFrame;
                    break;
            }

            frames++;
        }

        static int frames = 0;
        static FocusState focusState = FocusState.Unfocused;

        enum FocusState
        {
            Unfocused,
            Readying,
            Focusing,
        }

        const int readyingFrames = 40;
        const int focusingFrames = 80;

        const float activeFocusChargePerFrame = 0.05f;

        void FocusFade(Player player)
        {
            RoomSettings.FadePalette fadePalette = player.room.roomSettings.fadePalette;
            for (int i = 0; i < fadePalette.fades.Length; i++)
            {
                fadePalette.fades[i] = Mathf.Lerp(fadePalette.fades[i], 0, (float)frames / readyingFrames);
            }
        }
        float[] origRoomFades;

    }
}
