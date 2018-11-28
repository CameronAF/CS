using System;

namespace BackEnd.Containers
{
    class Block
    {
        private BitList _block;
        public int OriginalLength { get; protected set; }
        public int Size => _block.Count;

        // constructor for block
        // input the byte array that makes up the block
        // what is the size of the 
        public Block(BitList input, int blockSize)
        {
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

        // Get the content of block in bytes
        public BitList GetContent()
        {
            return new BitList(_block);
        }

        // Get the content of block in bytes original size
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
