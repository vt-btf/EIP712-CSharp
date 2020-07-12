using System;
using System.Collections.Generic;
using System.Text;

namespace EIP712Signature
{
    class Signature
    {
        public string R { get; set; }
        public string S { get; set; }
        public byte V { get; set; }
    }
}
