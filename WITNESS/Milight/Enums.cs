using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WITNESS.Milight
{
    class Enums
    {
        public enum Type
        {
            Normal,
            Combo
        }

        public enum GroupType
        {
            One,
            Two,
            Three,
            Four
        }

        // Most of the commands here were reversed from the Android Milight app, some are probably missing
        public enum Command
        {
            None = 0x00,

            AllOn = 0x35,
            AllOff = 0x39,

            BrightnessUp = 0x3C,
            BrightnessDown = 0x34,

            Group1On = 0x38,
            Group1Off = 0x3B,
            Group2On = 0x3D,
            Group2Off = 0x33,
            Group3On = 0x37,
            Group3Off = 0x3A,
            Group4On = 0x32,
            Group4Off = 0x36,

            AllFull = 0xB5,

            Group1Max = 0xB8,
            Group2Max = 0xBD,
            Group3Max = 0xB7,
            Group4Max = 0xB2,

            // RGB - Unsused
            RGBColor = 0x20,
            RGBOff = 0x21,
            RGBOn = 0x22,
            RGBBrightnessUp = 0x23,
            RGBBrightnessDown = 0x24,
            RGBSpeedUp = 0x25,
            RGBSpeedDown = 0x26,
            RGBDiscoNext = 0x27,
            RGBDiscoLast = 0x28,

            // RGBW  
            RGBWOff = 0x41,
            RGBWOn = 0x42,
            RGBWDiscoSpeedSlower = 0x43,
            RGBWDiscoSpeedFaster = 0x44,
            RGBWGroup1AllOn = 0x45,
            RGBWGroup1AllOff = 0x46,
            RGBWGroup2AllOn = 0x47,
            RGBWGroup2AllOff = 0x48,
            RGBWGroup3AllOn = 0x49,
            RGBWGroup3AllOff = 0x4A,
            RGBWGroup4AllOn = 0x4B,
            RGBWGroup4AllOff = 0x4C,
            RGBWDiscoLights = 0x4D,
            RGBWBrightness = 0x4E,
            RGBWColor = 0x40,

            SetToWhite = 0xC2,
            SetToWhiteGroup1 = 0xC5,
            SetToWhiteGroup2 = 0xC7,
            SetToWhiteGroup3 = 0xC9,
            SetToWhiteGroup4 = 0xCB,

            // TODO: Figure out what those do
            UNKNOWN1 = 0xB9,
            UNKNOWN2 = 0xB6,
            UNKNOWN3 = 0xBB,
            UNKNOWN4 = 0xBA,
            UNKNOWN5 = 0xB3
        }
    }
}
