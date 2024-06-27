// using aspnet.webapi.Data;
// using BenchmarkDotNet.Attributes;
// using Bogus;

// namespace aspnet.webapi;

// [MemoryDiagnoser]
// public class PaginationBenchmarks
// {
//     private static readonly Faker Faker = new();
//     private readonly ApplicationDbContext _context;
//     private readonly int _totalItems;
//     private readonly int _pageSize;

//     public PaginationBenchmarks()
//     {
//         _context = new ApplicationDbContext();
//         _totalItems = _context.Set<Entity>().Count();
//         _pageSize = 50;
//     }

//     [Params(100)]
//     public int Size { get; set; }

//     private User[] GetUsers()
//     {
//         return Enumerable
//             .Range(1, Size)
//             .Select(_ => new User
//             {
//                 Email = Faker.Internet.Email(),
//                 FirstName = Faker.Name.FirstName(),
//                 LastName = Faker.Name.LastName(),
//                 PhoneNumber = Faker.Phone.PhoneNumber(),
//             })
//             .ToArray();
//     }

//     [Benchmark]
//     public async Task EfAddOneAndSave()
//     {
//         using var context = new ApplicationDbContext();

//         foreach (var user in GetUsers())
//         {
//             context.Set<User>().Add(user);
//             await context.SaveChangesAsync();
//         }
//     }

//     [Benchmark]
//     public async Task EfAddOneByOne()
//     {
//         using var context = new ApplicationDbContext();

//         foreach (var user in GetUsers())
//         {
//             context.Set<User>().Add(user);
//         }

//         await context.SaveChangesAsync();
//     }

//     [Benchmark]
//     public async Task EfAddRange()
//     {
//         using var context = new ApplicationDbContext();
//         context.Set<User>().AddRange(GetUsers());
//         await context.SaveChangesAsync();
//     }
// }
