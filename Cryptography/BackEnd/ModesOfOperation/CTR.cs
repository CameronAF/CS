using BackEnd.Algorithms;
using BackEnd.Containers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.ModesOfOperation
{
    /// <summary>
    /// Counter mode of operation
    /// </summary>
    class CTR :IMode
    {
        private ulong key;
        DES des;

        /// <summary>
        /// constructor for ECB
        /// </summary>
        /// <param name="inKey">the encryption key</param>
        public CTR(ulong inKey)
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
            int i = 0;
            foreach (Block block in blocks)
            {
                Block counter = new Block(new BitList(BitConverter.GetBytes(i)), blocks[0].Size);
                Block s = des.Encrypt(counter);
                Block y = new Block(block.GetContent().Xor(s.GetContent()), block.Size);
                encryptedBlocks.AddBlock(y);
                i++;
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
