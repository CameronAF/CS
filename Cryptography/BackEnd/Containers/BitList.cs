using System;
using System.Collections;
using System.Collections.Generic;

namespace BackEnd.Containers
{
    class BitList : ICollection
    {
        private List<bool> bitList = new List<bool>();

        public int Count => bitList.Count;
        public object SyncRoot => throw new NotImplementedException();
        public bool IsSynchronized => throw new NotImplementedException();

        // constructors
        public BitList() { }
        public BitList(byte[] input) : this(new BitArray(input)) { }
        public BitList(bool[] input) { Populate(input); }
        public BitList(BitArray input) { Populate(input); }
        public BitList(List<bool> input) { Populate(input); }
        public BitList(BitList input) { Populate(input); }

        private void Populate(ICollection collection)
        {
            foreach (bool x in collection) { bitList.Add(x); }
        }

        // shift left (<---) by the amont of spaces specified
        public void ShiftLeft(int spaces)
        {
            var result = bitList.GetRange(spaces, bitList.Count - spaces);
            result.AddRange(bitList.GetRange(0, spaces));
            bitList = result;
        }

        // shift --->
        public void ShiftRight(int spaces)
        {
            var result = bitList.GetRange(bitList.Count - spaces, spaces);
            result.AddRange(bitList.GetRange(0, bitList.Count - spaces));
            bitList = result;
        }

        // add a range to the end of BitArray
        public BitList AddRange(BitList array2)
        {
            BitList newBL = new BitList(bitList);
            foreach (bool x in array2)
            {
                newBL.Add(x);
            }
            return newBL;
        }

        // add a bool to BitList
        public void Add(bool input)
        {
            bitList.Add(input);
        }

        // Remove a value at given index
        public void RemoveAt(int index)
        {
            bitList.RemoveAt(index);
        }

        // Split the BitArrray down the middle and return the back end of the array
        public BitList Split()
        {
            List<bool> end = new List<bool>();
            for (int x = bitList.Count / 2; x < bitList.Count; x++)
            {
                end.Add(bitList[x]);
            }
            int middle = bitList.Count / 2;
            bitList.RemoveRange(middle, middle);

            BitList endArray = new BitList(end);
            return endArray;
        }

        // Xor this BitList and a given BitList. results are returned
        public BitList Xor(BitList list2)
        {
            BitList result = new BitList();
            for (int i = 0; i < bitList.Count; i++)
            {
                if (bitList[i] == list2[i])
                {
                    result.Add(false);
                }
                else
                {
                    result.Add(true);
                }
            }
            return result;
        }

        // And this BitList and a given BitList. results are returned
        public BitList And(BitList list2)
        {
            BitList result = new BitList();
            for (int i = 0; i < bitList.Count; i++)
            {
                if (bitList[i] == true && list2[i] == true)
                {
                    result.Add(true);
                }
                else
                {
                    result.Add(false);
                }
            }
            return result;
        }

        // Return bool indicating if this BitList and a given BitList contain the same values
        public bool Equals(BitList bitList2)
        {
            for (int i = 0; i < bitList.Count; i++)
            {
                if (bitList[i] != bitList2[i]) { return false; }
            }
            return true;

        }

        // Allows BitList class to be indexble
        public bool this[int number]
        {
            get
            {
                // This is invoked when accessing Layout with the [ ].
                if (number >= 0 && number < bitList.Count)
                {
                    // Bounds were in range, so return the stored value.
                    return bitList[number];
                }
                // Return an error string.
                throw new Exception("index Error");
            }
            set
            {
                // This is invoked when assigning to Layout with the [ ].
                if (number >= 0 && number < bitList.Count)
                {
                    // Assign to this element slot in the internal array.
                    bitList[number] = value;
                }
            }
        }

        // create a new instance of BitList with these values
        public object Clone()
        {
            BitList copy = new BitList();
            foreach (bool x in bitList)
            {
                copy.Add(x);
            }
            return copy;
        }

        // Returns an enumerator that iterates theough the BitList.
        public IEnumerator GetEnumerator()
        {
            return bitList.GetEnumerator();
        }

        // Coppies the elements of BitList to an array, starting at a particular Array index
        public void CopyTo(Array array, int index)
        {
            if (array == null)
                throw new ArgumentNullException("array");
            bool[] ppArray = array as bool[];
            if (ppArray == null)
                throw new ArgumentException();
            ((ICollection<bool>)this).CopyTo(ppArray, index);
        }

        // convert to a byte array
        public byte[] ToByte()
        {
            int x = bitList.Count / 8;
            if (bitList.Count % 8 != 0) { x++; }
            byte[] array = new byte[x];
            BitArray ba = new BitArray(bitList.ToArray());
            ba.CopyTo(array, 0);
            return array;
        }

        // get a BitList starting at an index of a given count
        public BitList GetRange(int index, int count)
        {
            return new BitList(bitList.GetRange(index, count));
        }
    }
}
