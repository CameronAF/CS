using System;

namespace Chain_of_responsibility
{
    class Block
    {
        private byte[] _block;

        // constructor for block
        // input the byte array that makes up the block
        // what is the size of the 
        public Block(byte[] input, int arraySize)
        {
            // creat array
            _block = new byte[arraySize];

            // put content of input into block. if the input is smaller then the block, fill with zeros
            if (input.Length <= arraySize)
            {
                int i = 0;
                while (i < input.Length)
                {
                    _block[i] = input[i];
                    i++;
                }
                while (i < arraySize)
                {
                    _block[i] = 0;
                    i++;
                }

            }
            // the input is larger then the size of the block
            else
            {
                throw new Exception("Input for block constructor is to large");
            }
        }
        // Get the size of block in bytes
        public int SizeBytes()
        {
            return _block.Length;
        }
        // Get the size of block in bytes
        public int SizeBits()
        {
            return _block.Length * 8;
        }
        // Get the content of block in bytes
        public byte Get(int index)
        {
            return _block[index];
        }
    }
}
