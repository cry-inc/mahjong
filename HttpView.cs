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
        private Tile _selected = null;
        private bool _showHint = false;
        private bool _showMoveable = false;

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

            if (path == "/field.json")
            {
                e.Response.ContentType.Value = "application/json";
                buffer = Encoding.UTF8.GetBytes(BuildFieldJson());
            }
            else if (path == "/types.json")
            {
                e.Response.ContentType.Value = "application/json";
                buffer = Encoding.UTF8.GetBytes(BuildTypesJson());
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
                buffer = Encoding.UTF8.GetBytes(BuildFieldJson());
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
            if (action.StartsWith("moveable"))
                _showMoveable = !_showMoveable;
            else if (action.StartsWith("hint"))
                _showHint = !_showHint;
            else if (action.StartsWith("select"))
            {
                string[] splitted = action.Split(new char[]{'|'}, 
                    StringSplitOptions.RemoveEmptyEntries);
                if (splitted.Length != 3)
                    return;

                int x, y;
                if (int.TryParse(splitted[1], out x) && int.TryParse(splitted[2], out y))
                {
                    Tile tile = _field.GetTileFromCoord(x, y);
                    if (tile == null)
                        return;
                    else if (_selected == null || _selected == tile)
                        _selected = tile;
                    else
                    {
                        _field.Play(_selected, tile);
                        _selected = null;
                    }
                }
            }  
        }

        private string BuildFieldJson()
        {
            TilePair hintPair = null;
            if (_showHint)
                hintPair = _field.GetHint();

            Tile[] tiles = _field.GetSortedTiles();
            List<string> tilesJson = new List<string>();
            foreach (Tile tile in tiles)
            {
                string selected = (tile == _selected) ? "true" : "false";
                string disabled = !_field.CanMove(tile) && _showMoveable ? "true" : "false";
                string hint = _showHint && hintPair != null && (tile == hintPair.Tile1 || tile == hintPair.Tile2) ? "true" : "false";
                string type = (tile != null) ? tile.Type.Name : "empty";
                string tileJson = "          {\n";
                tileJson += "              \"x\": " + tile.X + ",\n";
                tileJson += "              \"y\": " + tile.Y + ",\n";
                tileJson += "              \"z\": " + tile.Z + ",\n";
                tileJson += "              \"type\": \"" + type + "\",\n";
                tileJson += "              \"selected\": " + selected + ",\n";
                tileJson += "              \"hint\": " + hint + ",\n";
                tileJson += "              \"disabled\": " + disabled + "\n";
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

        private string BuildTypesJson()
        {
            List<string> typesJson = new List<string>();
            for (int i = 0; i < _field.Types.Length; i++)
            {
                string type = 
                    "       {\n" +
                    "           \"id\": " + _field.Types[i].Id + ",\n" +
                    "           \"name\": \"" + _field.Types[i].Name + "\"\n" +
                    "       }";
                typesJson.Add(type);
            }

            return
            "{\n" +
            "   \"types\": [\n" +
            string.Join(",\n", typesJson.ToArray()) + "\n" +
            "   ]\n" +
            "}\n";
        }
    }
}
