﻿using BackEnd.Algorithms;
using BackEnd.Containers;

namespace BackEnd.ModesOfOperation
{
    /// <summary>
    /// Electronic Code Book mode of operation
    /// </summary>
    class ECB : IMode
    {
        private ulong key;
        DES des;

        /// <summary>
        /// constructor for ECB
        /// </summary>
        /// <param name="inKey">the encryption key</param>
        public ECB(ulong inKey)
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
            foreach (Block block in blocks)
            {
                encryptedBlocks.AddBlock(des.Encrypt(block));
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
            foreach (Block block in blocks)
            {
                dencryptedBlocks.AddBlock(des.Decrypt(block));
            }
            return dencryptedBlocks;
        }
    }
}
