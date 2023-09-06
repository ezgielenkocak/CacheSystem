using NRedisStack.RedisStackCommands;
using NRedisStack.Search;
using NRedisStack.Search.Aggregation;
using NRedisStack.Search.Literals.Enums;
using StackExchange.Redis;
using System.Net.Sockets;

public class Program
{
    private static void Main(string[] args)
    {
        #region RedisConnection
        //ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("localhost:1453"); //bağlantı için 
        //IDatabase db = redis.GetDatabase();
        #endregion


        #region Redise Basit veri ekleme-getirme
        //  db.StringSet("baby", "alice"); //stringset ile key-value' çiftini redis'e ekledim
        //  Console.WriteLine(db.StringGet("baby")); //key değerini isteyip value'yu yazdırdım.
        #endregion


        #region Redise Hash Veri Seti Ekleme-getirme
        // var hash = new HashEntry[]
        //{
        //     new HashEntry("name", "Elen"),
        //     new HashEntry("surname", "Koçak"),
        //     new HashEntry("age", "22"),
        //};
        // db.HashSet("userinfo", hash);

        // var hashFields = db.HashGetAll("userinfo");
        // Console.WriteLine(String.Join(";", hashFields));
        #endregion

        #region RedisClusterConnectionAndGetSetData
        //   ConfigurationOptions options = new()
        //  {
        //    EndPoints =
        //    {
        //        {"localhost", 1453 }
        //    },
        //  };
        //  ConnectionMultiplexer cluster=ConnectionMultiplexer.Connect(options);
        //  IDatabase db = cluster.GetDatabase();

        //  db.StringSet("fruit", "apple");
        //  Console.WriteLine(db.StringGet("fruit"));
        #endregion

        #region RedisSearch
        ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("localhost:6379");
        var db = redis.GetDatabase();
        var ft = db.FT(); //search and filter
        var json = db.JSON(); //convert to json

        //add dummy data
        var user1 = new
        {
            name = "elon musk",
            email = "elon@outlook.com",
            age = 54,
            city = "London"
        };

        var user2 = new
        {
            name = "mark lir",
            email = "mark@outlook.com",
            age = "34",
            city = "Marakesh"
        };
        var user3 = new
        {
            name = "Paul Zamir",
            email = "paul.zamir@example.com",
            age = 35,
            city = "Tel Aviv"

        };

        #region Schema Oluşturma
        var shcema = new Schema()
            .AddTextField(new FieldName("$.name", "name"))
            //.AddTextField(new FieldName("$.email", "email"))
            .AddTagField(new FieldName("$.city", "city"))
            .AddNumericField(new FieldName("$.age", "age")); 
        #endregion


        #region Index yaratma ve değişkenlere karşılık isim verme
        //ft.Create(
        //    "idx:userindex",
        //    new FTCreateParams().On(IndexDataType.JSON).Prefix("user:"), shcema);
        #endregion

        #region Oluşturduğum json nesnesini yukarıdaki user123 nesnelerine setlemek
        json.Set("user:1", "$", user1);
        json.Set("user:2", "$", user2);
        json.Set("user:3", "$", user3);
        #endregion


        #region //ismi paul ve yaşı 30-60 aralığındA
        //var response = ft.Search("idx:userindex", new Query("Paul @age:[30 60]")).Documents.Select(x => x["json"]);
        //Console.WriteLine(String.Join("\n", response));
        #endregion


        #region  //ismi paul olanın yaşını döndürmek
        var response_city = ft.Search("idx:userindex", new Query("Paul").ReturnFields(new FieldName("$.age", "age"))).Documents.Select(x => x["age"]);
        Console.WriteLine(String.Join("\n", response_city));
        #endregion



        #region şehirleri getirme

        #endregion
        var request = new AggregationRequest("*").GroupBy("@city", Reducers.Count().As("count"));
        var result = ft.Aggregate("idx:userindex", request);

        for (int i = 0; i < result.TotalResults; i++)
        {
            var row = result.GetRow(i);
            Console.WriteLine($"{row["city"]}-{row["count"]}");
        }

        Console.ReadKey();
        #endregion


    }
}