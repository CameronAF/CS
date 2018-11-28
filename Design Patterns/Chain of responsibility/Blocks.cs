using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chain_of_responsibility
{
    class Blocks
    {
        private List<Block> blocks = new List<Block>();
        private int _originalLength;
        private int _blockSize;

        public Blocks(int blockSize, int length)
        {
            // make sure that the desired block size can be broken up into bytes
            if (blockSize % 8 != 0)
            {
                throw new Exception("choose a block size where mod 8 is 0");
            }
            this._blockSize = blockSize;
            this._originalLength = length;
        }

        public Blocks(byte[] input, int blockSize)
        {
            // make sure that the desired block size can be broken up into bytes
            if (blockSize % 8 != 0)
            {
                throw new Exception("choose a block size where mod 8 is 0");
            }
            this._originalLength = input.Length;
            this._blockSize = blockSize;
            int bytesNum = blockSize / 8;

            // if the input is larger then 1 block
            if (input.Count() > bytesNum)
            {
                // split input into blocks
                int i = 0;
                while (i < input.Length - bytesNum)
                {
                    blocks.Add(new Block(SubArray(input, i, bytesNum), bytesNum));
                    i += bytesNum;
                }
                // last block
                if (i < input.Length)
                {
                    blocks.Add(new Block(SubArray(input, i, input.Length - i), bytesNum));
                }
            }
            else
            {
                blocks.Add(new Block(input, bytesNum));
            }
        }

        // Get the content of Blocks as a byte array
        public byte[] ToByte()
        {
            // create a byte array that will contain all blocks
            byte[] result = new byte[blocks.Count * blocks[0].SizeBytes()];

            // fill the array with the content from each block
            int k = 0;
            foreach (Block b in blocks)
            {
                for (int i = 0; i < blocks[0].SizeBytes(); i++)
                {
                    result[k] = b.Get(i);
                    k++;
                }
            }
            // append the array to be the length of the original data
            while (result.Length > _originalLength)
            {
                result = result.Take(result.Count() - 1).ToArray();
            }
            return result;
        }

        public int GetSize()
        {
            return _blockSize;
        }

        public int GetLength()
        {
            return _originalLength;
        }

        public Block GetBlock(int index)
        {
            return blocks[index];
        }

        public List<Block> GetBlocks()
        {
            return blocks;
        }

        public void AddBlock(Block block)
        {
            if (block.SizeBits() == _blockSize)
            {
                blocks.Add(block);
            }
            else
            {
                throw new Exception("size difference - cant add this block with these blocks");
            }
        }

        // break up an array a bytes 
        private byte[] SubArray(byte[] data, int index, int length)
        {
            byte[] result = new byte[length];
            Array.Copy(data, index, result, 0, length);
            return result;
        }
    }
}
