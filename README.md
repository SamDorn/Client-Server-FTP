# Client-Server-FTP C#
- [What is it?](#what-is-it-)
- [What does it do?](#what-does-it-do-)
- [How does it work?](#how-does-it-work-)
- [How to use it](#how-to-use-it)
  * [Server](#server)
    + [Events](#events)
  * [Client](#client)
    + [Connection](#connection)
    + [Upload](#upload)
    + [Download](#download)
    + [History](#history)
- [Credits](#credits)
- [Known bugs](#known-bugs)

## What is it?
It's a client-server architecture where clients can upload and download files to and from the server, similar to the FTP protocol. <br>
There are 2 application, a server.exe and a client.exe. The server is the one to receive and send files when requested and the client is the one
who can upload and download files.

## What does it do?

Once the server is listening for connections, the clients can connect, upload and download files. <br>
The uploaded files will be saved in Client-Server-FTP\Server\Server\bin\debug\net6.0\files <br>
The downloaded files will be saved in the folder you select.

## How does it work?

Both server and client are written in ___C#___.The only difference is that the client is a windows form, meanwhile the server is a console application. It uses socket to communicate and send files.<br>
For each upload, download and list of available files, the client send first a letter so the server can know what the client what. The receive of the 
Server is managed with switch-case to redirect the right function based on the letter receive.<br>
To send a file it is first necessary to send the file name. <br>After that a byte[] containing the lenght of the file is sent so the progress bar know how much 
byte need to be received and can display the prpgress based on the bytes received so far. <br>
The actual file is sent in block of 4096 kb. After that a terminator is sent, meaning the sending is over.




## How to use it

### Server
Open the server application(exe) that you will find in CLient-Server-FTP\Server\Server\bin\debug\net6.0.<br><br>
If no errors occurs the console should look like this and is ready to accept connections;<br><br>
![immagine](https://user-images.githubusercontent.com/114228291/205443416-5f4a2595-648b-43d2-8513-800561568483.png)
#### Events
1. Client connects to the server: <br>
  ![immagine](https://user-images.githubusercontent.com/114228291/205443550-a13e4d11-38c2-4768-8287-03286fe58d6a.png)
2. Client disconnect from the server:<br>
![immagine](https://user-images.githubusercontent.com/114228291/205443587-1409c0d4-7b25-4490-b7e4-9a40fc1a145c.png)
3. Client upload a file: <br>
![immagine](https://user-images.githubusercontent.com/114228291/205443645-b29468c9-d2b9-46fc-a69f-7816818053b1.png)
4. Client view the available files for the download: <br>
![immagine](https://user-images.githubusercontent.com/114228291/205443758-569cb81f-6c08-4ca2-94e2-a0c7ce7e036d.png)
5. Client download a file: <br>
![immagine](https://user-images.githubusercontent.com/114228291/205443797-6b60812f-49eb-407e-a1f4-e3ab3bd77940.png)


### Client
1) Open the client application(exe) that you will find in Client-Server-FTP\Client\Client-FTP\bin\Debug.<br><br>
If no errors occurs the form should look like this and is ready to connect;<br><br>
![immagine](https://user-images.githubusercontent.com/114228291/205442738-02358ad6-0a90-475d-8de1-e2113983a6ee.png)

#### Connection
Once the client is open and the server is listening you can connect the client, using the ip address, which by default is 127.0.0.1, and the port, default 5000, using the start button.<br>
![immagine](https://user-images.githubusercontent.com/114228291/205442872-64153521-8b35-4fa1-9405-e78d2ca5abd2.png)<br><br>To be able to see if the connection was successful you can check the text under the progress bar, if it was there would be a text like this <br><br>
![immagine](https://user-images.githubusercontent.com/114228291/205443006-8ef7fbe9-1d61-461f-8ce7-3478bfd4e5fb.png)
#### Upload
Once you are connected you can upload a file. In order to do so you have to click the browse button select a file and then click the upload button. <br><br>
![immagine](https://user-images.githubusercontent.com/114228291/205444144-3d10163c-363b-443e-8a9f-c88fe79eb499.png)<br><br>
You can check the status with the progress bar and the text under.
<br> <br>
![immagine](https://user-images.githubusercontent.com/114228291/205444193-47cb6f52-7c5b-4de9-a37e-122dc34998ba.png)<br><br>
Once the upload is finished you can see the text: <br><br>
![immagine](https://user-images.githubusercontent.com/114228291/205444219-4fdf9e89-9bf4-4ed8-939a-c3a49d3ec41c.png)


#### Download

To download a file you need to click view available files button and then select the file you want to download: <br><br>
![immagine](https://user-images.githubusercontent.com/114228291/205444276-f9430ca4-24f8-40f2-8ce5-550724365105.png) <br><br>
After that you need to click the download button where you can select where the file will be downloaded. <br><br>
![immagine](https://user-images.githubusercontent.com/114228291/205444377-7aa3b87a-068e-4291-8154-3e8d35c81095.png)
<br><br>
As for the upload, the progress bar will show the process of the download and after the completition of the download it will appar a text. <br><br>
![immagine](https://user-images.githubusercontent.com/114228291/205444542-cabe7291-658d-4c4e-8dd1-a4821ffa17ad.png)

#### History
If you press the Visualizza storico button you can see the operation you did and the possible error that occured.<br><br>
![immagine](https://user-images.githubusercontent.com/114228291/205445021-7e357b82-7cb3-4b9d-a026-3dccdc497ab8.png)<br><br>

![immagine](https://user-images.githubusercontent.com/114228291/205444624-2dc1f65b-2feb-4cf5-a056-ae2ec77ed7dd.png)

Feel free to use the :grey_question: at the right side of each buttons to understand better what they do.


## Credits
| Nome              | Autore                | Link                                                                  |
| ---------------   | --------------------- | --------------------------------------------------------------------- |
| ProgressBarConsole| DanielSWolf           | https://gist.github.com/DanielSWolf/0ab6a96899cc5377bf54              |
| Client Interface | Pete Batard         | https://github.com/pbatard/rufus            |


## Known bugs

:x: If multiple clients try to download the same file, only the first one is allowed.<br>
:x: Server console gets a bit ugly when multiple clients try to do different things. <br>


