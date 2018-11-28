using BackEnd.Containers;
using BackEnd.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BackEnd.Algorithms
{
    class DES
    {
        private KeyScheduler ks;
        // Tables needed for DES algorithum
        private List<byte> ipTable = new List<byte>(64);
        private List<byte> expansionTable = new List<byte>(48)
            {   32, 1,  2,  3,  4,  5,  4,  5,
                6,  7,  8,  9,  8,  9,  10, 11,
                12, 13, 12, 13, 14, 15, 16, 17,
                16, 17, 18, 19, 20, 21, 20, 21,
                22, 23, 24, 25, 24, 25, 26, 27,
                28, 29, 28, 29, 30, 31, 32, 1   };
        Dictionary<Tuple<byte, byte, byte>, byte> sTables = new Dictionary<Tuple<byte, byte, byte>, byte>(); // sfun, row, col
        private List<byte> fPermutationTable = new List<byte>(32);
        private List<byte> ipTableInverse = new List<byte>(64);

        // Constructor
        public DES(ulong key)
        {
            ks = new KeyScheduler(key);
            MakeTables();
        }

        #region Encryption/Decryption Methods
        // Encrypt input block using DES algorithum
        public Block Encrypt(Block input)
        {
            // get bits from block
            BitList data1 = input.GetContent();
            // run through Initial Permutaion
            data1 = InitialPurmutation(data1);
            // split into L and R (32-bits each)
            BitList data2 = data1.Split();
            // run 16 rounds
            for (int i = 0; i < 16; i++)
            {
                // fun function with key
                BitList fout = Function(data2, ks.GetK_enc());
                // Xor and swap sides
                data1 = data1.Xor(fout);
                BitList temp = data2;
                data2 = data1;
                data1 = temp;
            }
            // swap sides
            BitList temp2 = data2;
            data2 = data1;
            data1 = temp2;
            // run through inverse of Initial Permutaion
            data1 = data1.AddRange(data2);
            data1 = FinalPurmutation(data1);
            // return cyper block
            return new Block(data1, input.Size);
        }

        // Decrypt input block using DES algorithum
        public Block Decrypt(Block input)
        {
            // get bits from block
            BitList data1 = input.GetContent();
            // run through Initial Permutaion
            data1 = InitialPurmutation(data1);
            // split into L and R (32-bits each)
            BitList data2 = data1.Split();
            // run 16 rounds
            for (int i = 0; i < 16; i++)
            {
                // fun function with key
                BitList fout = Function(data2, ks.GetK_dec());
                // Xor and swap sides
                data1 = data1.Xor(fout);
                BitList temp = data2;
                data2 = data1;
                data1 = temp;
            }
            // swap sides
            BitList temp2 = data2;
            data2 = data1;
            data1 = temp2;
            // run through inverse of Initial Permutaion
            data1 = data1.AddRange(data2);
            data1 = FinalPurmutation(data1);
            // return cyper block
            return new Block(data1, input.Size);
        }

        // Used to run Initial Permutaion
        private BitList InitialPurmutation(BitList input)
        {
            BitList result = new BitList();
            foreach (byte index in ipTable)
            {
                result.Add(input[index]);
            }
            return result;
        }

        // Used to run Inverse of Initial Permutaion
        private BitList FinalPurmutation(BitList input)
        {
            BitList result = new BitList();
            foreach (byte index in ipTableInverse)
            {
                result.Add(input[index]);
            }
            return result;
        }
        #endregion

        #region Function Methods 
        // Run Function with Key
        private BitList Function(BitList input, BitList key)
        {
            // Expand input
            BitList bits = Expand(input);
            // Xor with key
            bits = bits.Xor(key);
            // run through multiple S boxes
            bits = SBoxes(bits);
            // Run Permutaion
            return FPermutation(bits);
        }

        // Run Expand Permutaion
        private BitList Expand(BitList input)
        {
            BitList result = new BitList();
            foreach (byte index in expansionTable)
            {
                result.Add(input[index - 1]);
            }
            return result;
        }

        // Split input into 8 6-bit packs and run through Dictionary of tuples
        private BitList SBoxes(BitList input)
        {
            // split input into 8 BitList of size 6
            List<BitList> sixBits = new List<BitList>(8);
            for (int i = 0; i < 8; i++)
            {
                sixBits.Add(input.GetRange(i * 6, 6));
            }
            // transform BitList using sTables
            BitList results = new BitList();
            // s is the s box to be used
            byte s = 0;
            foreach (BitList bits in sixBits)
            {
                // get the bits 0 and 5 for row in tuple
                BitList twoBits = new BitList();
                twoBits.Add(bits[0]);
                twoBits.Add(bits[5]);
                // bits 1,2,3,4 for col in tuple
                bits[0] = bits[5] = false;
                bits.ShiftLeft(1);
                // get the value in sTable dictionary: s table, row 2-bits, col 4-bits
                // byte[] has 8-bits, convert to BitList and split to remove irelivent last 4 bits
                byte[] tableValue = new byte[1] { sTables[Tuple.Create(s, twoBits.ToByte()[0], bits.ToByte()[0])] };
                BitList value = new BitList(tableValue);
                value.Split();
                // add to results. after 8 runs will be 32-bits long
                results = results.AddRange(value);
                s++;
            }
            return results;
        }

        // Run Permutaion in Function
        private BitList FPermutation(BitList input)
        {
            BitList result = new BitList();
            foreach (byte index in fPermutationTable)
            {
                result.Add(input[index]);
            }
            return result;
        }
        #endregion

        // Make Tables at initialization
        private void MakeTables()
        {
            // make ipTable
            ipTable = new List<byte>(Enumerable.Range(0, 64).Select(i => (byte)i));
            ipTable.Shuffle();
            // make sTables
            for (byte s = 0; s < 8; s++)
            {
                for (byte row = 0; row < 4; row++)
                {
                    List<byte> val = new List<byte>(Enumerable.Range(0, 16).Select(i => (byte)i));
                    val.Shuffle();
                    for (byte col = 0; col < 16; col++)
                    {
                        sTables.Add(Tuple.Create(s, row, col), val[col]);
                    }
                }
            }
            // make fPermutationTable
            fPermutationTable = new List<byte>(Enumerable.Range(0, 32).Select(i => (byte)i));
            fPermutationTable.Shuffle();
            // make ipTableInverse
            ipTableInverse.AddRange(Enumerable.Repeat(99, 64).Select(i => (byte)i));
            for (byte i = 0; i < ipTable.Count; i++)
            {
                ipTableInverse[ipTable[i]] = i;
            }
        }


        // the Kyy schedualer for DES. The whole Key Schedualing algorithium is doen threw this class
        // the only public methods are the constructer, GetK_enc() and  GetK_dec()
        private class KeyScheduler
        {
            private List<int> pc_1table = new List<int>();
            private List<int> pc_2table = new List<int>();
            private BitList privateKey;
            private BitList cKey = new BitList();
            private BitList dKey = new BitList();
            private int round = 0;
            private List<int> oneRotate = new List<int>() { 2, 9, 16 };

            // constructor
            // Given the Private Key. The first key will be ready to be received.
            public KeyScheduler(ulong inKey)
            {
                System.Collections.BitArray array = new System.Collections.BitArray(BitConverter.GetBytes(inKey));
                privateKey = new BitList(array);
                // create the lookup tables
                MakePC_1();
                MakePC_2();
                // reduce through PC_1
                PC_1();
            }

            // Get the Key to use in DES algorithum for encoding
            public BitList GetK_enc()
            {
                // count rounds
                round++;
                // shift 1 on rounds 1, 2, 9, and 16
                // shift 2 on all other rounds
                if (round == 1)
                {
                    cKey.ShiftLeft(1);
                    dKey.ShiftLeft(1);
                }
                else
                {
                    if (oneRotate.Contains(round))
                    {
                        cKey.ShiftLeft(1);
                        dKey.ShiftLeft(1);
                    }
                    else
                    {
                        cKey.ShiftLeft(2);
                        dKey.ShiftLeft(2);
                    }
                }
                // concat ckey and dkey to make the key to be returned (48-bits)
                BitList k = PC_2(cKey.AddRange(dKey));
                // if round 16. this is the last key. reset scheduler
                if (round == 16)
                {
                    round = 0;
                    cKey = new BitList();
                    dKey = new BitList();
                    PC_1();
                }
                return k;

            }

            // Get the Key to use in DES algorithum for decoding
            public BitList GetK_dec()
            {
                // count rounds
                round++;
                // shift 1 on rounds 2, 9, and 16
                // shift 2 on all other rounds
                if (round == 1)
                {
                    // no rotation
                }
                else
                {
                    if (oneRotate.Contains(round))
                    {
                        cKey.ShiftRight(1);
                        dKey.ShiftRight(1);
                    }
                    else
                    {
                        cKey.ShiftRight(2);
                        dKey.ShiftRight(2);
                    }
                }
                // concat ckey and dkey to make the key to be returned (48-bits)
                BitList k = PC_2(cKey.AddRange(dKey));
                // if round 16. this is the last key. reset scheduler
                if (round == 16)
                {
                    round = 0;
                    cKey = new BitList();
                    dKey = new BitList();
                    PC_1();
                }
                return k;
            }

            // run threw PC_1 - reduce from 64-bit to 56-bit
            private void PC_1()
            {
                foreach (int index in pc_1table)
                {
                    cKey.Add(privateKey[index]);
                }
                dKey = cKey.Split();
            }

            // run threw PC_2 - reduce from 56-bit to 48-bit
            private BitList PC_2(BitList cdKey)
            {
                BitList k = new BitList();
                foreach (int index in pc_2table)
                {
                    k.Add(cdKey[index]);
                }
                return k;
            }

            // make pc_1 table - 56 cells
            private void MakePC_1()
            {
                pc_1table = new List<int>(Enumerable.Range(0, 64));
                // remove 8, 16, 24, 32, 40, 48, 56, 64
                pc_1table.RemoveAll(item => ((item + 1) % 8 == 0));
                // shuffle using utility method
                pc_1table.Shuffle();
            }

            // make pc_2 table - 48 cells
            private void MakePC_2()
            {
                List<int> excludeNums = new List<int>();
                // make list of 8 distinct numbers from 0-56 to be excluded
                while (excludeNums.Count < 8)
                {
                    Random rnd = new Random();
                    int rndNum = rnd.Next(0, 55);
                    if (!(excludeNums.Contains(rndNum)))
                    {
                        excludeNums.Add(rndNum);
                    }
                }
                // make list from 0-56 and remove the 8 numbers to be excluded
                pc_2table = new List<int>(Enumerable.Range(0, 56));
                pc_2table.RemoveAll(item => (excludeNums.Contains(item)));
                // shuffle using utility method
                pc_2table.Shuffle();
            }
        }
    }
}
