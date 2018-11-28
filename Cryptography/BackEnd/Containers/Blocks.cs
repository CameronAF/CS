using System;
using System.Collections;
using System.Collections.Generic;

namespace BackEnd.Containers
{
    class Blocks : ICollection
    {
        private List<Block> blocks = new List<Block>();
        public int OriginalLength { get; protected set; }
        public int BlockSize { get; protected set; }
        public int Count => blocks.Count;
        public object SyncRoot => throw new NotImplementedException();
        public bool IsSynchronized => throw new NotImplementedException();

        // 3 constructors
        public Blocks(int blockSize, int length)
        {
            // make sure that the desired block size can be broken up into bytes
            if (blockSize % 8 != 0)
            {
                throw new Exception("choose a block size where mod 8 is 0");
            }
            this.BlockSize = blockSize;
            this.OriginalLength = length;
        }

        public Blocks(byte[] input, int blockSize) : this(new BitList(input), blockSize) { }

        public Blocks(BitList input, int blockSize)
        {
            // make sure that the desired block size can be broken up into bytes
            if (blockSize % 8 != 0)
            {
                throw new Exception("choose a block size where mod 8 is 0");
            }
            this.OriginalLength = input.Count;
            this.BlockSize = (blockSize == 0) ? 8 : blockSize;

            // if the input is larger then 1 block
            if (input.Count > BlockSize)
            {
                // split input into blocks
                int i = 0;
                while (i < input.Count - BlockSize)
                {
                    blocks.Add(new Block(input.GetRange(i, BlockSize), BlockSize));
                    i += BlockSize;
                }
                // last block
                if (i < input.Count)
                {
                    blocks.Add(new Block(input.GetRange(i, input.Count - i), BlockSize));
                }
            }
            else
            {
                blocks.Add(new Block(input, BlockSize));
            }
        }

        // Get the content of Blocks as a byte array
        public byte[] ToByte()
        {
            // create a BitList that will contain all blocks
            BitList bl = new BitList();
            // fill the BitList with the content from each block
            foreach (Block b in blocks)
            {
                bl = bl.AddRange(b.GetContent());
            }
            // append the BitList to be the length of the original data
            while (bl.Count > OriginalLength)
            {
                bl.RemoveAt(bl.Count - 1);
            }
            return bl.ToByte();
        }

        // add a block to blocks
        public void AddBlock(Block block)
        {
            // make sure that the block is of the right size
            if (block.Size == BlockSize)
            {
                blocks.Add(block);
            }
            else
            {
                throw new Exception("size difference - cant add this block with these blocks");
            }
        }

        // Returns an enumerator that iterates theough the BitList.
        public IEnumerator GetEnumerator()
        {
            return blocks.GetEnumerator();
        }

        public void CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }
    }
}
