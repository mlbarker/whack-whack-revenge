

namespace Assets.Scripts.Persistence
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using System.Linq;
    using System.Text;

    [StructLayout(LayoutKind.Explicit)]
    public struct DataBlock
    {
        [FieldOffset(0)]
        public int AllTheBytes;

        [FieldOffset(0)]
        public byte byte1;

        [FieldOffset(1)]
        public byte byte2;

        [FieldOffset(2)]
        public byte byte3;

        [FieldOffset(3)]
        public byte byte4;
    }
}
