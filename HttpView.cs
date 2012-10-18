using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

using HttpServer;
using HttpServer.Headers;


namespace Mahjong
{
    class HttpView
    {
        private Field _field;
        private HttpListener _listener;

        public HttpView(Field field)
        {
            _field = field;
            _listener = HttpListener.Create(System.Net.IPAddress.Any, 8080);
            _listener.RequestReceived += OnRequest;
            _listener.Start(5);
        }

        private void OnRequest(object sender, RequestEventArgs e)
        {
            e.Response.Connection.Type = ConnectionType.Close;
            string path = e.Request.Uri.LocalPath;
            byte[] buffer;

            if (path == "/mahjong.json")
            {
                e.Response.ContentType.Value = "application/json";
                buffer = Encoding.UTF8.GetBytes(BuildJson());
            }
            else if (path == "/jquery.js")
            {
                e.Response.ContentType.Value = "application/json";
                buffer = Encoding.UTF8.GetBytes(File.ReadAllText("web/jquery.js"));
            }
            else if (path.StartsWith("/action/"))
            {
                e.Response.ContentType.Value = "application/json";
                DoAction(path.Replace("/action/", ""));
                buffer = Encoding.UTF8.GetBytes(BuildJson());
            }
            else if (path.StartsWith("/image/"))
            {
                e.Response.ContentType.Value = "image/png";
                string image = path.Replace("/image/", "");
                string filePath = "tiles/" + image;
                if (File.Exists(filePath))
                    buffer = File.ReadAllBytes(filePath);
                else
                    buffer = new byte[0];
            }
            else
                buffer = Encoding.UTF8.GetBytes(File.ReadAllText("web/index.html"));

            e.Response.Body.Write(buffer, 0, buffer.Length);
        }

        private void DoAction(string action)
        {
            string[] splitted = action.Split('|');

            if (splitted.Length != 4)
                return;

            int x1, y1, x2, y2;
            if (int.TryParse(splitted[0], out x1) && int.TryParse(splitted[1], out y1) && int.TryParse(splitted[2], out x2) && int.TryParse(splitted[3], out y2))
            {
                Tile t1 = _field.GetTileFromCoord(x1, y1);
                Tile t2 = _field.GetTileFromCoord(x2, y2);
                if (t1 == null || t2 == null)
                    return;
                PlayResult result = _field.Play(t1, t2);
            }
        }

        private string BuildJson()
        {
            
            Tile[] tiles = _field.GetSortedTiles();
            List<string> tilesJson = new List<string>();
            foreach (Tile tile in tiles)
            {
                string type = (tile != null) ? tile.Type.Name : "tile";
                string tileJson = "          {\n";
                tileJson += "              \"x\": " + tile.X + ",\n";
                tileJson += "              \"y\": " + tile.Y + ",\n";
                tileJson += "              \"z\": " + tile.Z + ",\n";
                tileJson += "              \"type\": \"" + type + "\"\n";
                tileJson += "          }";
                tilesJson.Add(tileJson);
            }

            return
            "{\n" +
            "   \"field\": {\n" +
            "       \"fieldwidth\": " + Field.WIDTH + ",\n" +
            "       \"fieldheight\": " + Field.HEIGHT + ",\n" +
            "       \"tilewidth\": " + Tile.WIDTH + ",\n" +
            "       \"tileheight\": " + Tile.HEIGHT + ",\n" +
            "       \"tiles\": [\n" +
            string.Join(",\n", tilesJson.ToArray()) + "\n" +  
            "       ]\n" +
            "   }\n" +
            "}\n";
        }
    }
}
