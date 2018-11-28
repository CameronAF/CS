using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Chain_of_responsibility
{
    class Program
    {
        static void Main(string[] args)
        {
            string str = "America! the beautifull hoe ";
            byte[] arr = Encoding.ASCII.GetBytes(str); // 8 bits per byte - 64 bits = 8 bytes
            ECB ecb = new ECB(1218155198077);
            BitArray array = new BitArray(arr);
            BitList ba = new BitList(array);
            Blocks blocks = new Blocks(arr, 64);
            // DES
            byte[] arr2 = blocks.ToByte();
            string str2 = Encoding.ASCII.GetString(arr2);
        }
    }
}
