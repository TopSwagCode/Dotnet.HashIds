# Dotnet.HashIds

### __What is a HashId?__

In short it is a hashed id (duuh), that can be encoded / decoded from our values, eg. integers. This can be done using a salt, to ensure that we can protect our API's from attacks like: [Insecure Direct Object References](https://owasp.org/www-project-web-security-testing-guide/latest/4-Web_Application_Security_Testing/05-Authorization_Testing/04-Testing_for_Insecure_Direct_Object_References)

In short if a attacker gains access to url `example.com/1337` he would be able to start attacking our website with running numbers like `1338,1339,1340` etc. But if we use hashed ids instead, like `example.com/sAr24Q`, it would be far harder for a hacker to try to brute force gaining access to other results. So our `sAr24Q` could simply map to the integer `1337` and we are not exposing any database logic to our website. In same style as we see on youtube. Eg: https://youtu.be/dQw4w9WgXcQ


### __Example__ 

This could be implement as a simple hello world console app. But it would make more sense to show it as a WebAPI project.

First we need to install the nuget package

``` shell
$ dotnet add package Hashids.net --version 1.5.0
```

Then we can add it to the IOC container

``` csharp
builder.Services.AddSingleton<IHashids>(_ => new Hashids("Our super secret salt!", 5));
```

So we are creating our HashIds with a super secret salt (shh don't tell anyone) and setting the minimum length to be 5. In this project there is a simple in-memory database to store products that consists of a int Id, Guid Uid, string Name, that we can fetch from our API. The API endpoints using HashIds looks like the following:

``` csharp
[HttpGet("hashId")]
public IEnumerable<ProductResponse> GetHashId()
{
    return _productDatabase.Products.Select(x => new ProductResponse(_hashids.Encode(x.Id), x.Name));
}
```

Return json that would look like:

``` json
[
  {
    "id": "o56ky",
    "name": "Product: 1"
  },
  {
    "id": "4mXxm",
    "name": "Product: 2"
  },
  {
    "id": "4mkay",
    "name": "Product: 3"
  },
  {
    "id": "9mveB",
    "name": "Product: 4"
  },
  {
    "id": "VywDB",
    "name": "Product: 5"
  }
]
```

And for getting single element:


``` csharp
[HttpGet("hashId/{hashId}")]
public ActionResult<ProductResponse> GetByHashId(string hashId)
{
    if(_hashids.TryDecodeSingle(hashId, out int id))
    {
        var product = _productDatabase.Products.SingleOrDefault(x => x.Id == id);

        if (product == null)
            return NotFound();
        
        return Ok(new ProductResponse(hashId, product.Name));
    }

    return NotFound();
}
```

Return json that would look like:

``` json
{
  "id": "4mkay",
  "name": "Product: 3"
}
```

So as you can see, it is easy to encode and decode these HashIds to and from integers. We are using TryDecode because we want to return `NotFound()` in case of a hacker is trying to guess the salt of our HashIds. If we throw exception when we get invalid input, the hackers would know when they are hitting valid input. So we would threat all input as valid.

### __Why not Guids?__

So why go through all this and not just use Guids as primary key in the data? Well because there are several reasons. There is some copy pasta from stackexchange here:

#### __GUID Pros__

* Unique across every table, every database and every server
* Allows easy merging of records from different databases
* Allows easy distribution of databases across multiple servers
* You can generate IDs anywhere, instead of having to roundtrip to the database, unless partial * sequentiality is needed (i.e. with newsequentialid())
* Most replication scenarios require GUID columns anyway

##### __GUID Cons__

* It is a whopping 4 times larger than the traditional 4-byte index value; this can have serious performance and storage implications if you're not careful
* Cumbersome to debug (where userid='{BAE7DF4-DDF-3RG-5TY3E3RF456AS10}')
* The generated GUIDs should be partially sequential for best performance (eg, newsequentialid() on SQL Server 2005+) and to enable use of clustered indexes

#### __SQL Performance on Id vs Uid__

https://www.sqlskills.com/blogs/kimberly/guids-as-primary-keys-andor-the-clustering-key/

https://dba.stackexchange.com/questions/264/guid-vs-int-which-is-better-as-a-primary-key

https://www.mssqltips.com/sqlservertip/5105/sql-server-performance-comparison-int-versus-guid/

### __Performance of HashIds__

So it does come with minor performance price. I have downloaded the source code from https://github.com/ullmark/hashids.net and ran the performance benchmarks on my local pc.

|               Method |       Mean |    Error |    StdDev | Code Size |  Gen 0 | Allocated |
|--------------------- |-----------:|---------:|----------:|----------:|-------:|----------:|
|        RoundtripInts | 3,907.9 ns |  9.62 ns |   9.00 ns |      54 B |      - |     512 B |
|       RoundtripLongs | 4,589.7 ns | 70.31 ns |  65.77 ns |   2,932 B |      - |     512 B |
|         RoundtripHex | 3,980.6 ns | 77.93 ns | 104.04 ns |      53 B | 0.0153 |   1,344 B |
|         SingleNumber |   878.0 ns | 11.71 ns |  10.95 ns |     128 B |      - |      64 B |
| SingleNumberAsParams |   913.5 ns |  9.74 ns |   9.11 ns |   1,829 B | 0.0019 |     160 B |

### __Should we use it?__

It depends ;) It is up to you decide for fits your project. Maybe you already have existing database that use integers and this is a easy implementation to not expose your integer Id's. Maybe you have a insane amount of relations in your table and you want to reduce disk usage by storing it as integers instead of guids that are 4 times the size. Or maybe you want to be able to merge datasources easy and use guids that are unique across systems?

If you made it this far I hope you liked this repository and leave a star ;) 