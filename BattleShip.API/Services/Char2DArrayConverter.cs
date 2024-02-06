using System.Text.Json;
using System.Text.Json.Serialization;

namespace BattleShip.API.Services;

public class Char2DArrayConverter : JsonConverter<char[,]>
{
    public override char[,]? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var list = JsonSerializer.Deserialize<List<List<char>>>(ref reader, options);
        if(list == null) {
            return null;
        }
        var array = new char[list.Count, list[0].Count];
        for (int i = 0; i < list.Count; i++)
        {
            for (int j = 0; j < list[i].Count; j++)
            {
                array[i, j] = list[i][j];
            }
        }
        return array;
    }

    public override void Write(Utf8JsonWriter writer, char[,] value, JsonSerializerOptions options)
    {
        var list = new List<List<char>>();
        for (int i = 0; i < value.GetLength(0); i++)
        {
            var innerList = new List<char>();
            for (int j = 0; j < value.GetLength(1); j++)
            {
                innerList.Add(value[i, j]);
            }
            list.Add(innerList);
        }
        JsonSerializer.Serialize(writer, list, options);
    }
}