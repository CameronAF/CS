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
    /// Cipher Block Chaining mode of operation
    /// </summary>
    class CBC : IMode
    {
        private ulong key;
        DES des;

        /// <summary>
        /// constructor for ECB
        /// </summary>
        /// <param name="inKey">the encryption key</param>
        public CBC(ulong inKey)
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
            Block y = new Block(new BitList(), blocks[0].Size);
            foreach (Block block in blocks)
            {
                Block xor = new Block(block.GetContent().Xor(y.GetContent()),block.Size);
                y = des.Encrypt(xor);
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
            Blocks dencryptedBlocks = new Blocks(blocks.BlockSize, blocks.OriginalLength);
            Block prevY = new Block(new BitList(), blocks[0].Size);
            foreach (Block block in blocks)
            {
                Block xorer = des.Decrypt(block);
                Block x = new Block(xorer.GetContent().Xor(prevY.GetContent()), block.Size);
                dencryptedBlocks.AddBlock(x);
                prevY = block; // shalow copy is ok since we dont do calculations here
            }
            return dencryptedBlocks;
        }
    }
}
