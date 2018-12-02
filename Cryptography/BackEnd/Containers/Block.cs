using System;

namespace BackEnd.Containers
{
    class Block
    {
        private BitList _block;
        public int OriginalLength { get; protected set; }
        public int Size => _block.Count;

        /// <summary>
        /// Initializes a new instance of the Block.
        /// </summary>
        /// <param name="input">A BitList of bool values</param>
        /// <param name="blockSize">Optional: The size the block will be if bigger then the BitList</param> 
        public Block(BitList input, int blockSize = 0)
        {
            if (blockSize == 0) { blockSize = input.Count; };
            // put content of input into block. if the input is smaller then the block, fill with zeros
            if (input.Count <= blockSize)
            {
                // creat array
                _block = new BitList(input);
                OriginalLength = input.Count;
                // if array is smaller then block size, fill with zeros
                while (_block.Count != blockSize)
                {
                    _block.Add(false);
                }
            }
            // the input is larger then the size of the block
            else
            {
                throw new Exception("Input for block constructor is to large");
            }
        }

        /// <summary>
        /// Converts a block vlaue to a BitList.
        /// </summary>
        /// <returns>The value of the current block object converted to BitList.</returns>
        public BitList GetContent()
        {
            return new BitList(_block);
        }

        /// <summary>
        /// Converts a block vlaue to a byte array.
        /// </summary>
        /// <returns>The value of the current block object converted to an array of bytes.</returns>
        public byte[] ToByte()
        {
            // create a BitList that will contain all blocks
            BitList bl = new BitList(_block);
            // append the BitList to be the length of the original data
            while (bl.Count > OriginalLength)
            {
                bl.RemoveAt(bl.Count - 1);
            }
            return bl.ToByte();
        }
    }
}
