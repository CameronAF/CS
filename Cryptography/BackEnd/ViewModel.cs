// Algortihums sot one day be implimented:
//  Symmetric (private)
//    Stream Cipher
//      Asychronus Stream Cipher
//      One-Way Function
//      One-Time pad
//      Linear Feedback Shift Registers (LFSR)
//    Block Cipher
//      Data Encryption Standard (DES)
//      Triple DES
//        6 modes of operation
//      Advanced Encryption Standard (AES)
//  Asymmetric (publi)
//    RSA
//    ElGamel
//    Rabin
//    Blam Goldarsser
//  Hybrid - Combo of Symmetric and Asymmetric
//  Primaties and Protocals
//    3 Single Length MDC Hash Fucntion
//    Double-Length MDC Hash Function
//    CDC-MAC algorithm
//    MASH

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
    /// <summary>
    /// <c>ViewModel</c> is the view model in a MVVM archatecture pattern
    /// <para>
    /// This class is instantiated in the UI and has a data dictionary that runs a different function
    /// for the algorithum chosen in the UI
    /// </para>
    /// </summary>
    public class ViewModel : INotifyPropertyChanged
    {
        protected string _originalText = "Input you message here";
        protected string _encryptedText = "Encrypted Text will be here after clicking Encrypt Button";
        protected string _decryptedText = "Decryopted Text will be here after clicking Encrypt Button";
        public List<int> KeySizes { get; protected set; } = new List<int>() { 16, 32, 64, 128, 256, 512, 1024, 2048, 4096 };
        public int SelectedKeySize { get; set; } = 64;
        public Dictionary<string, Func<string, string, object>> AlgorithmFunctions { get; protected set; } = new Dictionary<string, Func<string, string, object>>();

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// <c>OriginalText</c> will be bound to the UIs Original textblockk and update UI when changed
        /// </summary>
        public string OriginalText
        {
            get => _originalText;
            set { _originalText = value; OnPropertyChanged(nameof(EncryptedText)); }
        }
        /// <summary>
        /// <c>EncryptedText</c> will be bound to the UIs Encrypted textblock and update UI when changed
        /// </summary>
        public string EncryptedText
        {
            get => _encryptedText;
            set { _encryptedText = value; OnPropertyChanged(nameof(EncryptedText)); }
        }
        /// <summary>
        /// <c>DecryptedText</c> will be bound to the UIs Decrypted textblock and update UI when changed
        /// </summary>
        public string DecryptedText
        {
            get => _decryptedText;
            set { _decryptedText = value; OnPropertyChanged(nameof(DecryptedText)); }
        }

        /// <summary>
        /// ViewModel constructor that populated the data dictionary
        /// </summary>
        public ViewModel()
        {
            AlgorithmFunctions.Add("DES using ECB mode", DES);
            AlgorithmFunctions.Add("DES using CBC mode", DES);
            AlgorithmFunctions.Add("DES using OFB mode", DES);
            AlgorithmFunctions.Add("DES using CFB mode", DES);
            AlgorithmFunctions.Add("DES using PCBC mode", DES);
            AlgorithmFunctions.Add("DES using CTR mode", DES);
            AlgorithmFunctions.Add("ElGamal", ElGamel);
            AlgorithmFunctions.Add("Hybrid", Hybrid);
        }

        /// <summary>
        /// Execute one of the fucntions in the data dictionary
        /// </summary>
        /// <param name="methodName">The Algorithum that will be executed. this is the key in the data dictionary</param>
        /// <param name="strs">The text that will be encrypted by the algorithum chosen by methodName</param>
        public void Execute(string methodName, string strs)
        {
            Func<string, string, object> method;

            if (!AlgorithmFunctions.TryGetValue(methodName, out method))
            {
                // Not found;
                throw new Exception();
            }

            method(strs, methodName);
        }

        private void OnPropertyChanged(string propertyName)
        {
            // take a copy to prevent thread issues
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Symmetric algorithum Data Encryption Standard with a mode of operation
        /// </summary>
        /// <param name="plainText">The text that will be encrypted and decrypted</param>
        public object DES(string plainText, string methodName)
        {
            // get content of text and put into blocks
            OriginalText = plainText;
            byte[] arr = Encoding.ASCII.GetBytes(OriginalText); // 8 bits per byte - 64 bits = 8 bytes
            Blocks blocks = new Blocks(new BitList(arr), 64);
            // make key
            RandomBigInteger rnd = new RandomBigInteger();
            BigInteger prime = rnd.MakePrime(64);
            // get the mode of operation to be used
            string modeName = methodName.Split(' ')[2];
            IMode mode = GetMode(modeName, prime);
            // pass blocks though encrption
            blocks = mode.Send(blocks);
            arr = blocks.ToByte();
            EncryptedText = Encoding.ASCII.GetString(arr);
            // pass blocks though decrption
            blocks = mode.Receive(blocks);
            arr = blocks.ToByte();
            DecryptedText = Encoding.ASCII.GetString(arr);
            return null;
        }

        /// <summary>
        /// Get the mode of operation to be used with a block cipher encryption
        /// </summary>
        /// <param name="mode">the IMode to get </param>
        /// <param name="key">the prime key needed to instantiate the IMode object</param>
        /// <returns> instance of object of IMode</returns>
        private IMode GetMode(string mode, BigInteger key)
        {
            switch (mode)
            {
                case "ECB":
                    return new ECB((ulong)key);
                case "CBC":
                    return new CBC((ulong)key);
                case "OFB":
                    return new OFB((ulong)key);
                case "CFB":
                    return new CFB((ulong)key);
                case "PCBC":
                    return new PCBC((ulong)key);
                case "CTR":
                    return new CTR((ulong)key);
                default:
                    throw new Exception("Something when wrong");
            }
        }

        /// <summary>
        /// Asymmetric algorithum ElGamel  
        /// </summary>
        /// <param name="plainText">The text that will be encrypted and decrypted</param>
        public object ElGamel(string plainText, string methodName)
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
        
        /// <summary>
        /// Hybrid algorithum using DES and ElGamal  
        /// </summary>
        /// <param name="plainText">The text that will be encrypted and decrypted</param>
        public object Hybrid(string plainText, string methodName)
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
