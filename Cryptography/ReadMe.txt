Student: Cameron Anzola-Ferreira (Z13188339)

---Assugnment 3---
This Assignment was to create a program that uses ElGamal public key encryption sheme to encrypte a message 
and reverse the algorithum to decrpte it.
The main executable can be ran from the zip file
"..\Cryptography\UI\bin\Release\UI.exe

The solution is broken up into a UI and a backend dll:
the MainWindow class in UI is the GUI of the program and initializes a viewModel 
from the backend project to populate the view and perform calculations

The Backend program is divided into 5 section:
 1) Algorithums 
 2) Containers (no changes made from Assignment 2)
 3) Modes of Operations (no changes made from Assignment 2)
 4) Utilities
 5) ViewModel

The Following changes where made for this assignment:
 1) Algorithums 
	- ElGamal.cs is an instantated class that creates public key and privite key on contruction.
		An instance calls Encrypte on a string of text with anothers prublic key information and a List of Messages is returned (if the text is larger then the key the text is broken up).
		the other instance calls Decrypt providing the list of messages to return the original text.
		systme class BigInteger is used for these large key calculations.
 2) Utilities
	- RandomBigInteger.cs is a class that make BigIntegers. most of these methods are from online
		NextBigInteger creates a BigInteger of a given bit size or range.
		MakePrime makes a prime of a given bit size by useing NextBigInteger and calling IsProbablePrime of a given certainty.
		Generator finds a Primative root from a given BigInteger.	
 3) ViewModel.cs
	- ViewModel is the comunicator of a MVVM archatecture design model. 
		This class binds elements to the view to be updated automaticly by inheriting from INotifyPropertyChanged when a property in the class gets modified. ViewModel calls the other backend proccessing classes based on the UI options chosen. When a user chooses a algorithum in the UI, the Execute method executes the appropreate method to impliment the algorithum. Only two algorithum are implimented at this time.

How to Run Program:
 1) run the executable
 2) input the message you would like to encrypte in the first text box
 3) choose which algorithum to execute (DES always uses 64 bit)
 4) click button to execute the algorithum
 
###--WARNNING--###
 - at the moment, using a key larger then 64 bits may take a while to compute.
 - use a key larger then 64 bits may also not produce proper results as the 
	primechecker algorithum may produce inacurate results due to the chosen certainty
	(a certainty of 10000 is chosen as to big a number took to long to compute)

...
...
...
---Assugnment 2--- (old)
This Assignment was to create a program that uses DES to encrypte a message 
and reverse the algorithum to decrpte it.
The main executable can be ran from the zip file
"..\Cryptography\UI\bin\Release\UI.exe

The program is broken up into the following classes:
 1) MainWindow.cs - the GUI of the program using xaml
 2) BitList.cs - collection containter used to store bits for easy manipulation
 3) Block.cs - container to store segments of BitList
 4) Blocks.cs - container to store multiple blocks
 5) ECB.cs - Mode of Operation inherating from IMode interface that executede Des algorithum
 6) DES.cs - DES algorithum
 7) Utility.cs - Utilities needed for program. random suffler
 
How to Run Program:
 1) run the executable
 2) input the message you would like to encrypte in the first text box
 3) click button to execute DES algorithum
 
Future Work:
This program is set up to easily add more Modes of Operations and other algorithums to practice different encryption methods.
