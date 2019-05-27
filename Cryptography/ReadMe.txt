This is a WPF application that executed encryption algorithms

HOW TO USE:
  1) Input a Message in the "Your Message" section
  2) Pick an encryption algorithm from the drop-down list
  3) Pick the size of the encryption key
      - Keys larger than 64 may take several minutes or hours to process
  4) Click the Encrypt button
      - The encrypted message will show in the center box and the decrypted message will be shown in the right box.

ABOUT THE PROGRAM:
The solution uses a MVVM architecture design patter and is separated into a UI and a BackEnd project. 

The UI allows 
 - the user to enter a message 
 - encrypt the message by a desired crypto algorithm
 - size of the encryption key

The BackEnd project contains the 
 - crypto algorithms
 - the containers that allow for bit and block manipulation 
 - the Big Integer generator
 - the View Models 
 
The algorithms that have been implemented are DES, ElGamal and a Hybrid. 
Six modes of operation have been implemented to be used with DES or any future block cipher algorithm that is to be created. Some slight modifications will need to be made to the modes to account for other algorithms, such as AES. 

NOTE:
The encryption and decryption times are very inefficient. This will later be remedied by using Cryto++ library
