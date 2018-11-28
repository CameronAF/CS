using BackEnd.Algorithms;
using BackEnd.Containers;

namespace BackEnd.ModesOfOperation
{
    class ECB : IMode
    {
        private ulong key;
        DES des;

        // constructor
        public ECB(ulong inKey)
        {
            this.key = inKey;
            des = new DES(key);
        }

        // Send blocks to be encrypted 
        public Blocks Send(Blocks blocks)
        {
            Blocks encryptedBlocks = new Blocks(blocks.BlockSize, blocks.OriginalLength);
            foreach (Block block in blocks)
            {
                encryptedBlocks.AddBlock(des.Encrypt(block));
            }
            return encryptedBlocks;
        }

        // Receive blocks to be decrypted 
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
