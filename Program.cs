using System;
using System.IO;

namespace SerializerFun {

    public class Record {

        //--- Properties ---
        public string Foo { get; set; }
    }

    public class Program {

        //--- Class Methods ---
        public static void Main(string[] args) {

        // create an instance to serialize
        var record = new Record {
            Foo = "Hello world!"
        };

        // show serialization with original Lambda serializer based on Newtonsoft.Json
        var oldSerializer = SerializeWith(record, new Amazon.Lambda.Serialization.Json.JsonSerializer());
        Console.WriteLine($"Amazon.Lambda.Serialization.Json.JsonSerializer: {oldSerializer}");

        // show serialization with new Lambda serializer based on System.Text.Json
        var newSerializer = SerializeWith(record, new Amazon.Lambda.Serialization.SystemTextJson.LambdaJsonSerializer());
        Console.WriteLine($"Amazon.Lambda.Serialization.SystemTextJson.LambdaJsonSerializer: {newSerializer}");

        // show serialization with System.Json.Text
        var jsonTextSerializer = System.Text.Json.JsonSerializer.Serialize<Record>(record);
        Console.WriteLine($"System.Text.Json.JsonSerializer: {jsonTextSerializer}");

        // local functions
        string SerializeWith<T>(T value, Amazon.Lambda.Core.ILambdaSerializer serializer) {
            using var buffer = new MemoryStream();
            serializer.Serialize<T>(value, buffer);;
            return System.Text.Encoding.UTF8.GetString(buffer.ToArray());
        }
        }
    }
}
