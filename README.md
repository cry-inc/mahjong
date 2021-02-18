This is a simple Mahjong solitaire game. It is written in C# and
uses the logos of various open source projects as tile images.

![Screenshot](/Mahjong.png)

The game was a quick POC for another implementation written in Scala.

There are no advanced features like a scoreboard or different tile setups,
but there are two other notable things included:
* Play on a web interface using the embedded web server.
  It uses the same graphics as the WinForms user interface.
  You can reach the web interface at http://127.0.0.1:8080/
* Setup Editor for creating your own Tile-Setups
  Start the executable with the argument -editor to activate it.
  You have to replace the file "setup.txt" to play with your custom setup.

Included third party software:
* httpserver - http://webserver.codeplex.com/

I removed the System.Web dependency of the httpserver, so it will run
on the .Net Client Profile. To achieve this, I used this file:
http://google-gdata.googlecode.com/svn/trunk/clients/cs/src/core/HttpUtility.cs
