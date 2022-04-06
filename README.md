# Dotnet.HashIds

What is a HashId? In short it is a hashed id (duuh), that can be encoded / decoded from your values, eg. integers. This can be done using a salt to ensure that we can protect our API's from attacks like: [Insecure Direct Object References](https://owasp.org/www-project-web-security-testing-guide/latest/4-Web_Application_Security_Testing/05-Authorization_Testing/04-Testing_for_Insecure_Direct_Object_References)

In short if a attacker gains access to url `example.com/1337` he would be able to start attacking our website with running numbers like `1338,1339,1340` etc. But if we use hashed ids instead, like `example.com/sAr24Q`, it would be far harder for a hacker to try to brute force gaining access to other results. So our `sAr24Q` could simply map to the integer `1337` and we are not exposing any database logic to our website. In same style as we see on youtube. Eg: [https://youtu.be/dQw4w9WgXcQ](https://youtu.be/dQw4w9WgXcQ)

So why go through all this and not just use Guids as primary key in the data? Well because there are several reasons. I have copy pasta'ed some stackexchange below here:

### GUID Pros

* Unique across every table, every database and every server
* Allows easy merging of records from different databases
* Allows easy distribution of databases across multiple servers
* You can generate IDs anywhere, instead of having to roundtrip to the database, unless partial * sequentiality is needed (i.e. with newsequentialid())
* Most replication scenarios require GUID columns anyway

### GUID Cons

* It is a whopping 4 times larger than the traditional 4-byte index value; this can have serious performance and storage implications if you're not careful
* Cumbersome to debug (where userid='{BAE7DF4-DDF-3RG-5TY3E3RF456AS10}')
* The generated GUIDs should be partially sequential for best performance (eg, newsequentialid() on SQL Server 2005+) and to enable use of clustered indexes

### SQL Performance on Id vs Uid

https://www.sqlskills.com/blogs/kimberly/guids-as-primary-keys-andor-the-clustering-key/

https://dba.stackexchange.com/questions/264/guid-vs-int-which-is-better-as-a-primary-key

https://www.mssqltips.com/sqlservertip/5105/sql-server-performance-comparison-int-versus-guid/

### Performance of HashIds (on my pc)

|               Method |       Mean |    Error |    StdDev | Code Size |  Gen 0 | Allocated |
|--------------------- |-----------:|---------:|----------:|----------:|-------:|----------:|
|        RoundtripInts | 3,907.9 ns |  9.62 ns |   9.00 ns |      54 B |      - |     512 B |
|       RoundtripLongs | 4,589.7 ns | 70.31 ns |  65.77 ns |   2,932 B |      - |     512 B |
|         RoundtripHex | 3,980.6 ns | 77.93 ns | 104.04 ns |      53 B | 0.0153 |   1,344 B |
|         SingleNumber |   878.0 ns | 11.71 ns |  10.95 ns |     128 B |      - |      64 B |
| SingleNumberAsParams |   913.5 ns |  9.74 ns |   9.11 ns |   1,829 B | 0.0019 |     160 B |
