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
    /// Cipher Feedback mode of operation
    /// </summary>
    class CFB :IMode
    {
        private ulong key;
        DES des;

        /// <summary>
        /// constructor for CFB
        /// </summary>
        /// <param name="inKey">the encryption key</param>
        public CFB(ulong inKey)
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
            Block prevY = new Block(new BitList(), blocks[0].Size);
            foreach (Block block in blocks)
            {
                Block s = des.Encrypt(prevY);
                Block y = new Block(block.GetContent().Xor(s.GetContent()), block.Size);
                encryptedBlocks.AddBlock(y);
                prevY = new Block(y.GetContent(), y.Size);
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
            Blocks dencryptedBlocks = new Blocks(blocks.BlockSize, blocks.OriginalLength);
            Block prevY = new Block(new BitList(), blocks[0].Size);
            foreach (Block block in blocks)
            {
                Block s = des.Encrypt(prevY);
                Block x = new Block(block.GetContent().Xor(s.GetContent()), block.Size);
                dencryptedBlocks.AddBlock(x);
                prevY = new Block(block.GetContent(), block.Size);
            }
            return dencryptedBlocks;
        }
    }
}
