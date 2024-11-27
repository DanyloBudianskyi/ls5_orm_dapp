using Dapper;
using ls5_orm_dapp.Models;
using Microsoft.Data.SqlClient;

namespace ls5_orm_dapp
{
    public class CompanyData
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string technology { get; set; } = string.Empty;
        public override string ToString()
        {
            return $"Id: {Id}, Name: {Name}, Technology: {technology}";
        }
    }
    public class Program
    {
        static void Main(string[] args)
        {
            var connectionString = @"Server=DESKTOP-8ISC2JM;Database=Shop;Integrated Security=True; TrustServerCertificate=True;";
            var db = new SqlConnection(connectionString);
            db.Open();
            User user1 = new User { Name = "John", Email = "john@gmail.com" };
            User user2 = new User { Name = "Alex", Email = "alex@gmail.com" };
            Order order1 = new Order { UserId = 1, Product = "Product1", Quantity = 5 };
            Order order2 = new Order { UserId = 2, Product = "Product2", Quantity = 1 };
            //AddUser(db, user1);
            //AddUser(db, user2);
            //AddOrder(db, order1);
            //AddOrder(db, order2);
            DeleteOrder(db, 1);
            DeleteUser(db, 1);
            var users = GetUsersWithOrders(db);
            foreach (var user in users)
            {
                Console.WriteLine($"Id: {user.Id}, Name: {user.Name}, Email: {user.Email}\nOrders:\n");
                foreach(var order in user.Orders)
                {
                    Console.WriteLine($"\tOrder ID: {order.Id}, Product: {order.Product}, Quantity: {order.Quantity}\n");
                }
            }
            db.Close();
        }
        public static int AddUser(SqlConnection connection, User user)
        {
            string sql = @"insert into Users(Name, Email) values(@Name, @Email)";
            return connection.Execute(sql, user);
        }
        public static int AddOrder(SqlConnection connection, Order order)
        {
            string sql = @"insert into Orders(UserId, Product, Quantity) values(@UserId, @Product, @Quantity)";
            return connection.Execute(sql, order);
        }
        public static int DeleteUser(SqlConnection connection, int id)
        {
            string sql = @"delete from Users where Id = @Id";
            return connection.Execute(sql, new { Id = id });
        }
        public static int DeleteOrder(SqlConnection connection, int id)
        {
            string sql = @"delete from Orders where Id = @Id";
            return connection.Execute(sql, new { Id = id });
        }
        public static List<User> GetUsersWithOrders(SqlConnection connection)
        {
            string sql = @"SELECT u.*, o.Id AS OrderId, o.UserId, o.Product, o.Quantity FROM Users u LEFT JOIN Orders o ON u.Id = o.UserId;";
            var userDictionary = new Dictionary<int, User>();
            var result = connection.Query<User, Order, User>(sql, (user, order) => {
                if (!userDictionary.TryGetValue(user.Id, out var currentUser))
                {
                    currentUser = user; currentUser.Orders = new List<Order>();
                    userDictionary.Add(currentUser.Id, currentUser);
                }
                if (order != null) { currentUser.Orders.Add(order); }
                return currentUser;
            }, splitOn: "OrderId");
            return result.Distinct().ToList();
        }
    }
}
