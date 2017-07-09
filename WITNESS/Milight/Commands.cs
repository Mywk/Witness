using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WITNESS.Milight
{
    class Commands
    {
        private static byte[] suffix = { 0x0, 0x55 };

        public static byte[] GetCommandSequence(int type)
        {
            if (Enum.IsDefined(typeof(Enums.Command), type))
            {
                return (new byte[] { (byte)type }).Concat(suffix).ToArray();
            }
            return null;
        }

        public static byte[] GetCommandSequenceWithParam(int type, byte param)
        {
            if (Enum.IsDefined(typeof(Enums.Command), type))
            {
                var byteArray = (new byte[] { (byte)type }).Concat(suffix).ToArray();
                byteArray[1] = param;
                return byteArray;
            }
            return null;
        }
    }
}
