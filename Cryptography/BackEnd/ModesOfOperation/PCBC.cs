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
    /// Propagating or Plaintext Cipher-Block Chaining mode of operation
    /// </summary>
    class PCBC : IMode
    {
        private ulong key;
        DES des;

        /// <summary>
        /// constructor for ECB
        /// </summary>
        /// <param name="inKey">the encryption key</param>
        public PCBC(ulong inKey)
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
            Block xorVector = new Block(new BitList(), blocks[0].Size);
            foreach (Block block in blocks)
            {
                Block xor = new Block(block.GetContent().Xor(xorVector.GetContent()), block.Size);
                Block y = des.Encrypt(xor);
                encryptedBlocks.AddBlock(y);
                xorVector = new Block(block.GetContent().Xor(y.GetContent()), block.Size);
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
            Block xorVector = new Block(new BitList(), blocks[0].Size);
            foreach (Block block in blocks)
            {
                Block s = des.Decrypt(block);
                Block x = new Block(s.GetContent().Xor(xorVector.GetContent()), block.Size);
                dencryptedBlocks.AddBlock(x);
                xorVector = new Block(block.GetContent().Xor(x.GetContent()), block.Size);
            }
            return dencryptedBlocks;
        }
    }
}
