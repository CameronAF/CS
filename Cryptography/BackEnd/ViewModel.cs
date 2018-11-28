using BackEnd.Algorithms;
using BackEnd.Containers;
using BackEnd.ModesOfOperation;
using BackEnd.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Numerics;
using System.Text;
using System.Threading;
using static BackEnd.Algorithms.ElGamal;

namespace BackEnd
{
    public class ViewModel : INotifyPropertyChanged
    {
        protected string _originalText = "Input you message here";
        protected string _encryptedText = "Encrypted Text will be here after clicking Encrypt Button";
        protected string _decryptedText = "Decryopted Text will be here after clicking Encrypt Button";
        public List<int> KeySizes { get; protected set; } = new List<int>() { 16, 32, 64, 128, 256, 512, 1024, 2048, 4096 };
        public int SelectedKeySize { get; set; } = 64;
        public Dictionary<string, Func<string, object>> AlgorithmFunctions { get; protected set; } = new Dictionary<string, Func<string, object>>();

        public event PropertyChangedEventHandler PropertyChanged;

        public string OriginalText
        {
            get => _originalText;
            set { _originalText = value; OnPropertyChanged(nameof(EncryptedText)); }
        }
        public string EncryptedText
        {
            get => _encryptedText;
            set { _encryptedText = value; OnPropertyChanged(nameof(EncryptedText)); }
        }
        public string DecryptedText
        {
            get => _decryptedText;
            set { _decryptedText = value; OnPropertyChanged(nameof(DecryptedText)); }
        }

        public ViewModel()
        {
            AlgorithmFunctions.Add("DES with ECB", DES_ECB);
            AlgorithmFunctions.Add("ElGamal", ElGamel);
            AlgorithmFunctions.Add("Hybrid", Hybrid);
        }

        public void Execute(string methodName, string strs)
        {
            Func<string, object> method;

            if (!AlgorithmFunctions.TryGetValue(methodName, out method))
            {
                // Not found;
                throw new Exception();
            }

            method(strs);
        }

        private void OnPropertyChanged(string propertyName)
        {
            // take a copy to prevent thread issues
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public object DES_ECB(string plainText)
        {
            //get content of text and put into blocks
            OriginalText = plainText;
            byte[] arr = Encoding.ASCII.GetBytes(OriginalText); // 8 bits per byte - 64 bits = 8 bytes
            RandomBigInteger rnd = new RandomBigInteger();
            BigInteger prime = rnd.MakePrime(64);
            ECB ecb = new ECB((ulong)prime);
            Blocks blocks = new Blocks(new BitList(arr), 64);
            // pass blocks though encrption
            blocks = ecb.Send(blocks);
            arr = blocks.ToByte();
            EncryptedText = Encoding.ASCII.GetString(arr);
            // pass blocks though decrption
            blocks = ecb.Receive(blocks);
            arr = blocks.ToByte();
            DecryptedText = Encoding.ASCII.GetString(arr);
            return null;
        }

        public object ElGamel(string plainText)
        {
            ElGamal alice = new ElGamal();
            ElGamal bob = new ElGamal();
            List<Thread> threads = new List<Thread>();
            threads.Add(new Thread(() => { alice = new ElGamal(SelectedKeySize); }));
            threads.Add(new Thread(() => { bob = new ElGamal(SelectedKeySize); }));
            threads.ForEach(t => t.Start());
            threads.ForEach(t => t.Join());
            List<Message> message = bob.Encrypt(plainText, alice.publicKey);
            EncryptedText = bob.GetMessage(message);
            DecryptedText = alice.Decrypt(message);
            return null;
        }

        public object Hybrid(string plainText)
        {
            // prep players 
            OriginalText = plainText;
            byte[] arr = Encoding.ASCII.GetBytes(OriginalText); // 8 bits per byte - 64 bits = 8 bytes
            ElGamal alice = new ElGamal();
            ElGamal bob = new ElGamal();
            List<Thread> threads = new List<Thread>();
            threads.Add(new Thread(() => { alice = new ElGamal(SelectedKeySize); }));
            threads.Add(new Thread(() => { bob = new ElGamal(SelectedKeySize); }));
            threads.ForEach(t => t.Start());
            threads.ForEach(t => t.Join());
            //To encrypt a message addressed to Alice in a hybrid cryptosystem, Bob does the following:
            // 1. Obtains Alice's public key.
            PublicKey aKey = alice.publicKey;
            // 2. Generates a fresh symmetric key for the data encapsulation scheme.
            RandomBigInteger rnd = new RandomBigInteger();
            BigInteger prime = rnd.MakePrime(64);
            // 3. Encrypts the message under the data encapsulation scheme, using the symmetric key just generated.
            ECB ecb = new ECB((ulong)prime);
            Blocks blocks = new Blocks(new BitList(arr), 64);
            blocks = ecb.Send(blocks);
            arr = blocks.ToByte();
            EncryptedText = "Message: " + Encoding.ASCII.GetString(arr);
            // 4. Encrypts the symmetric key under the key encapsulation scheme, using Alice's public key.
            List<Message> message = bob.Encrypt(prime.ToString(), aKey);
            EncryptedText = EncryptedText + "\nKey: " + bob.GetMessage(message);
            // 5. Sends both of these encryptions to Alice.

            //To decrypt this hybrid ciphertext, Alice does the following:
            // 1. Uses her private key to decrypt the symmetric key contained in the key encapsulation segment.
            DecryptedText = "\nKey: " + alice.Decrypt(message);
            // 2. Uses this symmetric key to decrypt the message contained in the data encapsulation segment.
            blocks = ecb.Receive(blocks);
            arr = blocks.ToByte();
            DecryptedText = "Message: " + Encoding.ASCII.GetString(arr) + DecryptedText;
            return null;
        }
    }
}
