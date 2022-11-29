# Client-Server-FTP C#

Simple code that send files throgh socket. 

## What does it do?

The clients will be uploding files to the server and the clients
can view the files available and can download them.
## How does it work?
Both server and client are written in ___c#___.The only difference is that client is a windows form, meanwhile the server is console. It uses socket to send files.
it is necessary to use binary writer, since to method Socket.Send() only accept 
byte[], to write the file with the received data.

The uploaded files to the server will be saved in ___/Server-FTP/bin/Debug/net6.0/files/.___

The downloaded files from the sever will be saved in ___/Client-FTP/bin/Debug/files/.___
## How to use it
Firstly you need to excecute the ___server-FTP.exe___ that you will find in ___/Server-FTP/bin/Debug/net6.0/.___
Once the server is active and ready to receive you can excecute the ___client-FTP.exe___ that you will find in 
___/Client-FTP/bin/Debug/.___ <br> <br>
Feel free to use the :grey_question: at the right side of the application to understand better what to do
## Known bugs

:x: Try/catch not implemented: if an error occurs it will not be handled; <br>
:x: Stop button doesn't disconnect form the server properly;<br>
:x: Progress bar not implemented;<br>



