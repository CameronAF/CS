using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chain_of_responsibility
{
    class DES
    {
        KeyScheduler ks;
        public DES(long key)
        {
            ks = new KeyScheduler(key);
        }
        public Block Encrypt(Block input, long key)
        {
            throw new NotImplementedException();
        }
        public Block Decrypt(Block input, long key)
        {
            throw new NotImplementedException();
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
            public KeyScheduler(long inKey)
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
                        cKey.ShiftLeft(2);
                        dKey.ShiftLeft(2);
                    }
                }
                // concat ckey and dkey to make the key to be returned (48-bits)
                return PC_2(cKey.AddRange(dKey));
            }

            // run threw PC_1 - reduce from 64-bit to 56-bit
            private void PC_1()
            {
                foreach (int index in pc_1table)
                {
                    cKey.Add(privateKey.GetAt(index));
                }
                dKey = cKey.Split();
            }

            // run threw PC_2 - reduce from 56-bit to 48-bit
            private BitList PC_2(BitList cdKey)
            {
                BitList k = new BitList();
                foreach (int index in pc_2table)
                {
                    k.Add(cdKey.GetAt(index));
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
