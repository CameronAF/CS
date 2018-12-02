using System;
using System.Collections;
using System.Collections.Generic;

namespace BackEnd.Containers
{
    /// <summary>
    /// Block class is a collection of blocks
    /// </summary>
    class Blocks : ICollection
    {
        private List<Block> blocks = new List<Block>();
        public int OriginalLength { get; protected set; }
        public int BlockSize { get; protected set; }
        public int Count => blocks.Count;
        public object SyncRoot => throw new NotImplementedException();
        public bool IsSynchronized => throw new NotImplementedException();

        /// <summary>
        /// Initializes a new empty instance of the Blocks.
        /// </summary>
        /// <param name="blockSize">The size of each block</param>
        /// <param name="length">The total size of all the blocks</param>
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

        /// <summary>
        /// Initializes a new instance of the Blocks using the values in a byte array.
        /// </summary>
        /// <param name="input">An array of byte values</param>
        /// <param name="blockSize">The size of each block</param>
        public Blocks(byte[] input, int blockSize) : this(new BitList(input), blockSize) { }

        /// <summary>
        /// Initializes a new instance of the Blocks using the values in a BitList.
        /// </summary>
        /// <param name="input">A Bitlist of bool values</param>
        /// <param name="blockSize">The size of each block</param>
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

        /// <summary>
        /// Converts a Blocks vlaue to a byte array.
        /// </summary>
        /// <returns>The value of the current Blocks object converted to an array of bytes.</returns>
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

        /// <summary>
        /// Adds an Block to the end of the Blocks.
        /// </summary>
        /// <param name="block">The Block to be added to the end of the Blocks</param>
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

        /// <summary>
        /// Gets or sets the Block at the specified index.
        /// </summary>
        /// <param name="number">The zero-based index of the element to get or set.</param>
        /// <returns>The Block at the specified index.</returns>
        public Block this[int number]
        {
            get
            {
                // This is invoked when accessing Layout with the [ ].
                if (number >= 0 && number < blocks.Count)
                {
                    // Bounds were in range, so return the stored value.
                    return blocks[number];
                }
                // Return an error string.
                throw new Exception("index Error");
            }
            set
            {
                // This is invoked when assigning to Layout with the [ ].
                if (number >= 0 && number < blocks.Count)
                {
                    // Assign to this element slot in the internal array.
                    blocks[number] = value;
                }
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An IEnumerator that can be used to iterate through the collection.</returns>
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
