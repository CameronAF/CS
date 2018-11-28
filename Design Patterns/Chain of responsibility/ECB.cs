using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chain_of_responsibility
{
    class ECB : IMode
    {
        private long key;
        DES des;
        public ECB(long inKey)
        {
            this.key = inKey;
            des = new DES(key);
        }

        public Blocks Send(Blocks blocks)
        {
            Blocks encryptedBlocks = new Blocks(blocks.GetSize(), blocks.GetLength());
            foreach (Block block in blocks.GetBlocks())
            {
                encryptedBlocks.AddBlock(des.Encrypt(block, key));
            }
            return encryptedBlocks;
        }
        public Blocks Receive(Blocks blocks)
        {
            Blocks dencryptedBlocks = new Blocks(blocks.GetSize(), blocks.GetLength());
            foreach (Block block in blocks.GetBlocks())
            {
                dencryptedBlocks.AddBlock(des.Decrypt(block, key));
            }
            return dencryptedBlocks;
        }
    }
}
