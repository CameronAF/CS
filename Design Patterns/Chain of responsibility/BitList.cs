using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chain_of_responsibility
{
    class BitList : ICollection, ICloneable
    {
        private List<bool> array = new List<bool>();

        public BitList() { }

        public BitList (BitArray input)
        {
            foreach(bool x in input)
            {
                array.Add(x);
            }
        }

        public BitList(List<bool> input)
        {
            foreach (bool x in input)
            {
                array.Add(x);
            }
        }

        // shift left (<---) by the amont of spaces specified
        public void ShiftLeft(int spaces)
        {
            var result = array.GetRange(spaces, array.Count - spaces);
            result.AddRange(array.GetRange(0, spaces));
            array = result;
        }

        // shift --->
        public void ShiftRight(int spaces)
        {
            var result = array.GetRange(array.Count - spaces, spaces);
            result.AddRange(array.GetRange(0, array.Count - spaces));
            array = result;
        }

        // add a range to the end of BitArray
        public BitList AddRange(BitList array2)
        {
            BitList newBL = new BitList(array);
            foreach (bool x in array2)
            {
                newBL.Add(x);
            }
            return newBL;
        }

        public void Add(bool input)
        {
            array.Add(input);
        }

        public void RemoveAt(int index)
        {
            array.RemoveAt(index);
        }

        public bool GetAt(int index)
        {
            return array[index];
        }
        // Split the BitArrray down the middle and return the back end of the array
        public BitList Split()
        {
            List<bool> end = new List<bool>();
            for (int x = array.Count / 2; x < array.Count; x++ )
            {
                end.Add(array[x]);
            }
            int middle = array.Count / 2;
            array.RemoveRange(middle, middle);

            BitList endArray = new BitList(end);
            return endArray;
        }

        public int Count => array.Count;

        public object SyncRoot => throw new NotImplementedException();

        public bool IsSynchronized => throw new NotImplementedException();

        public object Clone()
        {
            List<bool> copy = new List<bool>();
            foreach (bool x in array)
            {
                copy.Add(x);
            }
            return copy;
        }

        public void CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }

        // Returns an enumerator that iterates theough the BitList.
        public IEnumerator GetEnumerator()
        {
            return array.GetEnumerator();
        }
    }
}
