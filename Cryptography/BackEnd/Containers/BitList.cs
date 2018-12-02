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

        /// <summary>
        /// populate this from a Collection
        /// </summary>
        /// <param name="collection">the Collection that will populate this</param>
        private void Populate(ICollection collection)
        {
            foreach (bool x in collection) { bitList.Add(x); }
        }

        /// <summary>
        /// shift left (&lt;---) by the amount of spaces specified
        /// </summary>
        /// <param name="spaces">the amount of shaces to shift</param>
        public void ShiftLeft(int spaces)
        {
            var result = bitList.GetRange(spaces, bitList.Count - spaces);
            result.AddRange(bitList.GetRange(0, spaces));
            bitList = result;
        }

        /// <summary>
        /// shift right (---&gt;) by the amount of spaces specified
        /// </summary>
        /// <param name="spaces">the amount of shaces to shift</param>
        public void ShiftRight(int spaces)
        {
            var result = bitList.GetRange(bitList.Count - spaces, spaces);
            result.AddRange(bitList.GetRange(0, bitList.Count - spaces));
            bitList = result;
        }

        /// <summary>
        /// Append the elements of the BitList to the end of the BitList.
        /// </summary>
        /// <param name="array2">The Collection whose elements should be appened to the end of this BitList. </param>
        /// <returns>a new intance of BitList containing elements from both this and the given collection </returns>
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

        /// <summary>
        /// Adds a bool to the end of the BitList.
        /// </summary>
        /// <param name="input">The bool to be added to the end of the BitList</param>
        public void Add(bool input)
        {
            bitList.Add(input);
        }

        /// <summary>
        /// Removes the element at the specified index of the BitList
        /// </summary>
        /// <param name="index">The zero-based index of the element to remove.</param>
        public void RemoveAt(int index)
        {
            bitList.RemoveAt(index);
        }

        /// <summary>
        /// Split the BitArrray in half, keeping the fisrt half.
        /// </summary>
        /// <returns>the end half of the BitList after the split</returns>
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

        /// <summary>
        /// XOR logic on this BitList and a given BitList.
        /// </summary>
        /// <param name="list2">The BitList to be used in the XOR operation</param>
        /// <returns>The output resuts of the XOR logic as a BitList</returns>
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

        /// <summary>
        /// AND logic on this BitList and a given BitList.
        /// </summary>
        /// <param name="list2">The BitList to be used in the AND operation</param>
        /// <returns>The output resuts of the AND logic as a BitList</returns>
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

        /// <summary>
        /// Determines whether this instance and a specified BitList, have the same value in the same order.
        /// </summary>
        /// <param name="bitList2">The BitList to compare to this instance.</param>
        /// <returns>true if obj value is the same as this instance; otherwise, false.  If obj is null, the method returns false.</returns>
        public bool Equals(BitList bitList2)
        {
            try
            {
                for (int i = 0; i < bitList.Count; i++)
                {
                    if (bitList[i] != bitList2[i]) { return false; }
                }
                return true;
            }
            catch { NullReferenceException ex; }
            {
                return false;
            }

        }

        /// <summary>
        /// Gets or sets the BitList at the specified index.
        /// </summary>
        /// <param name="number">The zero-based index of the element to get or set.</param>
        /// <returns>The bool at the specified index.</returns>
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

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An IEnumerator that can be used to iterate through the collection.</returns>
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

        /// <summary>
        /// Converts a BitList vlaue to a byte array.
        /// </summary>
        /// <returns>The value of the current BitList object converted to an array of bytes.</returns>
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
