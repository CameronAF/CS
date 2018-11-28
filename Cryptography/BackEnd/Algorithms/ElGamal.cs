using BackEnd.Containers;
using BackEnd.Utilities;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using System.Threading;

namespace BackEnd.Algorithms
{
    class ElGamal
    {
        private BigInteger P;
        private BigInteger a;
        private BigInteger B;
        private BigInteger d;
        public PublicKey publicKey;

        public ElGamal() { }

        public ElGamal(int keySize)
        {
            RandomBigInteger rnd = new RandomBigInteger();
            P = rnd.MakePrime(keySize);
            List<Thread> threads = new List<Thread>();
            threads.Add(new Thread(() => { a = rnd.Generator(P); }));
            threads.Add(new Thread(() => { d = rnd.NextBigInteger(3, (keySize < 2147483647) ? keySize - 2 : 2147483647); }));
            threads.ForEach(t => t.Start());
            threads.ForEach(t => t.Join());
            B = BigInteger.ModPow(a, d, P);
            publicKey = new PublicKey(P, a, B);
        }

        public List<Message> Encrypt(string plaintext, PublicKey othersKey)
        {
            Random rnd = new Random();
            byte[] arr = Encoding.ASCII.GetBytes(plaintext);
            Blocks blocks = new Blocks(arr, (P.ToByteArray().Length -1)*8 );
            List<Message> messages = new List<Message>();
            foreach(Block block in blocks)
            {
                BigInteger k = rnd.Next(1, 1000);//p-2 < 2147483647 ? (int)p-2 : 2147483647);
                BigInteger m = new BigInteger(block.ToByte());
                BigInteger r = BigInteger.ModPow(othersKey.a, k, othersKey.P);
                BigInteger t = ((BigInteger.ModPow(othersKey.B, k, othersKey.P) * (m % othersKey.P)) % othersKey.P);
                messages.Add(new Message(r, t));
            }
            return messages;
        }

        public string Decrypt(List<Message> messages)
        {
            BitList bitList = new BitList();
            foreach (Message message in messages)
            {
                BigInteger num = ((BigInteger.ModPow(message.r, (P - 1 - d), P) * (message.t % P)) % P);
                bitList = bitList.AddRange(new BitList(num.ToByteArray()));
            }
            byte[] arr2 = bitList.ToByte();
            return Encoding.ASCII.GetString(arr2);
        }

        public string GetMessage(List<Message> messages)
        {
            BitList bitList = new BitList();
            foreach (Message message in messages)
            {
                bitList = bitList.AddRange(new BitList(message.t.ToByteArray()));
            }
            byte[] arr2 = bitList.ToByte();
            return Encoding.ASCII.GetString(arr2);
        }

        // Public Key
        public class PublicKey
        {
            public PublicKey(BigInteger x, BigInteger y, BigInteger z)
            {
                P = x;
                a = y;
                B = z;
            }
            public BigInteger P { get; protected set; }
            public BigInteger a { get; protected set; }
            public BigInteger B { get; protected set; }
        }

        // Message
        public class Message
        {
            public Message(BigInteger R, BigInteger T)
            {
                r = R;
                t = T;
            }
            public BigInteger r { get; protected set; }
            public BigInteger t { get; protected set; }
        }
    }
}
