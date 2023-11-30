using Microsoft.Data.Sqlite;
using System;
using System.Text;

class SkladTable
{
    private string connectionString = "Data Source=SkladTable.sqlite";

    public void CreateSkladTable()
    {
        Console.OutputEncoding = Encoding.UTF8;
        string createSuppliersTable = @"
            CREATE TABLE IF NOT EXISTS Suppliers (
                SupplierId INTEGER UNIQUE PRIMARY KEY,
                SupplierName NVARCHAR(255) NOT NULL
            );
        ";
        string createTypesTable = @"
            CREATE TABLE IF NOT EXISTS Types (
                TypeId INTEGER UNIQUE PRIMARY KEY,
                TypeName NVARCHAR(255) NOT NULL
            );
        ";
        string createGoodsTable = @"
            CREATE TABLE IF NOT EXISTS Goods (
                ProductId INTEGER PRIMARY KEY AUTOINCREMENT,
                Name NVARCHAR(255) UNIQUE NOT NULL,
                TypeId INTEGER NOT NULL,
                SupplierId INTEGER NOT NULL,
                Quantity INTEGER NOT NULL,
                CostPrice DECIMAL(18,2) NOT NULL,
                SupplyDate DATETIME NOT NULL,
                FOREIGN KEY(TypeId) REFERENCES Types(TypeId),
                FOREIGN KEY(SupplierId) REFERENCES Suppliers(SupplierId)
            );
        ";

        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            using (SqliteCommand command = new SqliteCommand(createSuppliersTable, connection))
            {
                command.ExecuteNonQuery();
            }

            using (SqliteCommand command = new SqliteCommand(createTypesTable, connection))
            {
                command.ExecuteNonQuery();
            }

            using (SqliteCommand command = new SqliteCommand(createGoodsTable, connection))
            {
                command.ExecuteNonQuery();
            }
        }

        string insertTypes = @"
            INSERT OR IGNORE INTO Types (TypeId, TypeName) VALUES (1, 'Ноутбук'), (2, 'Планшет'), (3, 'Монітор');
        ";

        string insertSuppliers = @"
            INSERT OR IGNORE INTO Suppliers (SupplierId, SupplierName) VALUES (1, 'Sumsung'), (2, 'Dell'), (3, 'Lenovo');
        ";

        string insertGoods = @"
            INSERT OR IGNORE INTO Goods (Name, TypeId, SupplierId, Quantity, CostPrice, SupplyDate) VALUES 
            ('Ноутбук Lenovo IdeaPad Slim 5 16IAH8', 1, 3, 4, 26999, '2023-11-29'),
            ('Ноутбук Samsung Galaxy Book2 Pro', 1, 1, 3, 45999, '2023-10-29'),
            ('Ноутбук Dell Latitude 5540', 1, 2, 2, 60849, '2022-11-29'),
            ('Планшет Lenovo Tab P11 Plus', 2, 3, 3, 9999, '2023-02-25'),
            ('Планшет Samsung Galaxy Tab S7', 2, 1, 1, 20899, '2023-08-29'),
            ('Dell Latitude 7212 Rugged Extreme Tablet i5', 2, 2, 1, 17450, '2023-01-29'),
            ('Монитор 28 Samsung Odyssey G7 LS28BG702', 3, 1, 2, 18999, '2023-07-29'),
            ('Монитор Lenovo 29 L29w-30', 3, 3, 4, 7999, '2023-04-29'),
            ('Монитор 34 Dell Alienware AW3423DWF', 3, 2, 2, 37999, '2023-08-29'),
            ('Монитор 28 Samsung Odyssey G7 LS28BG702', 3, 1, 2, 18999, '2023-07-29');
        ";

        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            using (SqliteCommand command = new SqliteCommand(insertTypes, connection))
            {
                command.ExecuteNonQuery();
            }

            using (SqliteCommand command = new SqliteCommand(insertSuppliers, connection))
            {
                command.ExecuteNonQuery();
            }

            using (SqliteCommand command = new SqliteCommand(insertGoods, connection))
            {
                command.ExecuteNonQuery();
            }
        }
    }

    public void DisplayAllProducts(SqliteConnection connection)
    {
        string query = "SELECT * FROM Goods";
        using (SqliteCommand command = new SqliteCommand(query, connection))
        using (SqliteDataReader reader = command.ExecuteReader())
        {
            Console.WriteLine("\nВся інформація про товар:");
            while (reader.Read())
            {
                Console.WriteLine($"{reader["Name"]}, {reader["TypeId"]}, {reader["SupplierId"]}, " +
                                  $"{reader["Quantity"]}, {reader["CostPrice"]}, {reader["SupplyDate"]}");
            }
        }
    }

    public void DisplayAllProductTypes(SqliteConnection connection)
    {
        string query = "SELECT DISTINCT TypeName FROM Types";
        using (SqliteCommand command = new SqliteCommand(query, connection))
        using (SqliteDataReader reader = command.ExecuteReader())
        {
            Console.WriteLine("\nУсі типи товарів:");
            while (reader.Read())
            {
                Console.WriteLine(reader["TypeName"]);
            }
        }
    }

    public void DisplayAllSuppliers(SqliteConnection connection)
    {
        string query = "SELECT DISTINCT SupplierName FROM Suppliers";
        using (SqliteCommand command = new SqliteCommand(query, connection))
        using (SqliteDataReader reader = command.ExecuteReader())
        {
            Console.WriteLine("\nУсі постачальники:");
            while (reader.Read())
            {
                Console.WriteLine(reader["SupplierName"]);
            }
        }
    }

    public void DisplayProductWithMaxQuantity(SqliteConnection connection)
    {
        string query = "SELECT * FROM Goods ORDER BY Quantity DESC LIMIT 1";
        using (SqliteCommand command = new SqliteCommand(query, connection))
        using (SqliteDataReader reader = command.ExecuteReader())
        {
            Console.WriteLine("\nТовар з максимальною кількістю:");
            while (reader.Read())
            {
                Console.WriteLine($"{reader["Name"]}, {reader["Quantity"]}");
            }
        }
    }

    public void DisplayProductWithMinQuantity(SqliteConnection connection)
    {
        string query = "SELECT * FROM Goods ORDER BY Quantity ASC LIMIT 1";
        using (SqliteCommand command = new SqliteCommand(query, connection))
        using (SqliteDataReader reader = command.ExecuteReader())
        {
            Console.WriteLine("\nТовар з мінімальною кількістю:");
            while (reader.Read())
            {
                Console.WriteLine($"{reader["Name"]}, {reader["Quantity"]}");
            }
        }
    }

    public void DisplayProductWithMinCost(SqliteConnection connection)
    {
        string query = "SELECT * FROM Goods ORDER BY CostPrice ASC LIMIT 1";
        using (SqliteCommand command = new SqliteCommand(query, connection))
        using (SqliteDataReader reader = command.ExecuteReader())
        {
            Console.WriteLine("\nТовар з мінімальною собівартістю:");
            while (reader.Read())
            {
                Console.WriteLine($"{reader["Name"]}, {reader["CostPrice"]}");
            }
        }
    }

    public void DisplayProductWithMaxCost(SqliteConnection connection)
    {
        string query = "SELECT * FROM Goods ORDER BY CostPrice DESC LIMIT 1";
        using (SqliteCommand command = new SqliteCommand(query, connection))
        using (SqliteDataReader reader = command.ExecuteReader())
        {
            Console.WriteLine("\nТовар з максимальною собівартістю:");
            while (reader.Read())
            {
                Console.WriteLine($"{reader["Name"]}, {reader["CostPrice"]}");
            }
        }
    }

    public void DisplayProductsByCategory(SqliteConnection connection, string category)
    {
        string query = $"SELECT * FROM Goods INNER JOIN Types ON Goods.TypeId = Types.TypeId WHERE TypeName = '{category}'";
        using (SqliteCommand command = new SqliteCommand(query, connection))
        using (SqliteDataReader reader = command.ExecuteReader())
        {
            Console.WriteLine($"\nТовари категорії '{category}':");
            while (reader.Read())
            {
                Console.WriteLine($"{reader["Name"]}, {reader["TypeName"]}");
            }
        }
    }

    public void DisplayProductsBySupplier(SqliteConnection connection, string supplier)
    {
        string query = $"SELECT * FROM Goods INNER JOIN Suppliers ON Goods.SupplierId = Suppliers.SupplierId WHERE SupplierName = '{supplier}'";
        using (SqliteCommand command = new SqliteCommand(query, connection))
        using (SqliteDataReader reader = command.ExecuteReader())
        {
            Console.WriteLine($"\nТовари постачальника '{supplier}':");
            while (reader.Read())
            {
                Console.WriteLine($"{reader["Name"]}, {reader["SupplierName"]}");
            }
        }
    }

    public void DisplayProductWithLongestStorage(SqliteConnection connection)
    {
        string query = "SELECT * FROM Goods ORDER BY SupplyDate ASC LIMIT 1";
        using (SqliteCommand command = new SqliteCommand(query, connection))
        using (SqliteDataReader reader = command.ExecuteReader())
        {
            Console.WriteLine("\nТовар, який знаходиться на складі найдовше:");
            while (reader.Read())
            {
                Console.WriteLine($"{reader["Name"]}, {reader["SupplyDate"]}");
            }
        }
    }

    public void DisplayAverageQuantityByProductType(SqliteConnection connection)
    {
        string query = "SELECT TypeName, AVG(Quantity) AS AverageQuantity FROM Goods INNER JOIN Types ON Goods.TypeId = Types.TypeId GROUP BY TypeName";
        using (SqliteCommand command = new SqliteCommand(query, connection))
        using (SqliteDataReader reader = command.ExecuteReader())
        {
            Console.WriteLine("\nСередня кількість товарів за кожним типом:");
            while (reader.Read())
            {
                Console.WriteLine($"{reader["TypeName"]}, {reader["AverageQuantity"]}");
            }
        }
    }

    static void Main()
    {
        SkladTable skladTable = new SkladTable();
        skladTable.CreateSkladTable();

        using (SqliteConnection connection = new SqliteConnection(skladTable.connectionString))
        {
            try
            {
                connection.Open();
                Console.WriteLine("Підключено до бази даних.");

                skladTable.DisplayAllProducts(connection);
                skladTable.DisplayAllProductTypes(connection);
                skladTable.DisplayAllSuppliers(connection);
                skladTable.DisplayProductWithMaxQuantity(connection);
                skladTable.DisplayProductWithMinQuantity(connection);
                skladTable.DisplayProductWithMinCost(connection);
                skladTable.DisplayProductWithMaxCost(connection);
                skladTable.DisplayProductsByCategory(connection, "Ноутбук"); 
                skladTable.DisplayProductsBySupplier(connection, "Sumsung"); 
                skladTable.DisplayProductWithLongestStorage(connection);
                skladTable.DisplayAverageQuantityByProductType(connection);

                Console.WriteLine("Робота з базою даних завершена.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка: {ex.Message}");
            }
        }
    }
}