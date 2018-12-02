using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BackEnd.Algorithms;
using BackEnd.Containers;

namespace BackEnd.ModesOfOperation
{
    /// <summary>
    /// Output Feedback mode of operation
    /// </summary>
    class OFB : IMode
    {
        private ulong key;
        DES des;

        /// <summary>
        /// constructor for OFB
        /// </summary>
        /// <param name="inKey">the encryption key</param>
        public OFB(ulong inKey)
        {
            this.key = inKey;
            des = new DES(key);
        }

        /// <summary>
        /// Send blocks to be encrypted 
        /// </summary>
        /// <param name="blocks">Blocks that will be encrypted</param>
        /// <returns>Blocks of encypted text</returns>
        public Blocks Send(Blocks blocks)
        {
            Blocks encryptedBlocks = new Blocks(blocks.BlockSize, blocks.OriginalLength);
            Block s = new Block(new BitList(), blocks[0].Size);
            foreach (Block block in blocks)
            {
                s = des.Encrypt(s);
                Block y = new Block(block.GetContent().Xor(s.GetContent()), block.Size);
                encryptedBlocks.AddBlock(y);
            }
            return encryptedBlocks;
        }

        /// <summary>
        /// Receive blocks to be decrypted 
        /// </summary>
        /// <param name="blocks">Blocks that will be decrypted</param>
        /// <returns>Blocks of decrypted text</returns>
        public Blocks Receive(Blocks blocks)
        {
            return Send(blocks);
        }
    }
}
