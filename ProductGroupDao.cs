using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace DZProekt
{
    internal class ProductGroupDao
    {
        private readonly SqlConnection _connection;

        public ProductGroupDao(SqlConnection connection)
        {
            _connection = connection;
        }
        public void CreateTable()
        {
            using SqlCommand command = new() { Connection = _connection };
            command.CommandText = @"CREATE TABLE ProductGroups (
                                    Id          UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
                                    Name        NVARCHAR(50)     NOT NULL,
                                    Pass NTEXT            NOT NULL,
                                    Picture     NVARCHAR(50)     NULL
                                )";
            command.ExecuteNonQuery();
        }
        public List<UserGroups> GetAll()
        {
            using SqlCommand command = new() { Connection = _connection };
            command.CommandText = @"SELECT pg.* FROM GroupUser AS pg WHERE DateDelete IS NULL";
            try
            {
                using SqlDataReader reader = command.ExecuteReader();
                var ProductGroups = new List<UserGroups>();
                while (reader.Read())
                {
                    ProductGroups.Add(new()
                    {
                        Id = reader.GetGuid(0),
                        Name = reader.GetString(1),
                        Pass = reader.GetString(2),
                        Picture = reader.GetString(3),
                    });
                }
                return ProductGroups;
            }
            catch { throw; }
        }

        public void Add(UserGroups productGroup)
        {
            using SqlCommand command = new() { Connection = _connection };
            command.CommandText = @"INSERT INTO GroupUser (Id, Name, Pass, Picture)
                                    VALUES (@id, @name, @pass, @picture)";
            // подготовка запроса -- создание временной хранимой процедуры в СУБД
            command.Prepare();
            // задаём типы и ограничения параметров
            command.Parameters.Add(new SqlParameter("@id", SqlDbType.UniqueIdentifier));
            command.Parameters.Add(new SqlParameter("@name", SqlDbType.NVarChar, 50));
            command.Parameters.Add(new SqlParameter("@pass", SqlDbType.NText));
            command.Parameters.Add(new SqlParameter("@picture", SqlDbType.NVarChar, 50));
            // задаём значение параметров
            command.Parameters[0].Value = productGroup.Id;
            command.Parameters[1].Value = productGroup.Name;
            command.Parameters[2].Value = productGroup.Pass;
            command.Parameters[3].Value = productGroup.Picture;
            // выполняем запрос
            command.ExecuteNonQuery();
        }

        public void Delete(UserGroups productGroup)
        {
            using SqlCommand command = new() { Connection = _connection };
            command.CommandText = @$"UPDATE GroupUser
                                     SET DateDelete = @datetime WHERE Id = '{productGroup.Id}'";
            command.Prepare();
            command.Parameters.Add(new SqlParameter("@datetime", SqlDbType.DateTime));
            command.Parameters[0].Value = DateTime.Now;

            command.ExecuteNonQuery();
        }

        public void Update(UserGroups productGroup)
        {
            using SqlCommand command = new() { Connection = _connection };
            command.CommandText = @$"UPDATE GroupUser
                                     SET Name = @name, Pass = @pass, Picture = @picture WHERE Id = '{productGroup.Id}'";
            command.Prepare();

            command.Parameters.Add(new SqlParameter("@name", SqlDbType.NVarChar, 50));
            command.Parameters.Add(new SqlParameter("@pass", SqlDbType.NText));
            command.Parameters.Add(new SqlParameter("@picture", SqlDbType.NVarChar, 50));

            command.Parameters[0].Value = productGroup.Name;
            command.Parameters[1].Value = productGroup.Pass;
            command.Parameters[2].Value = productGroup.Picture;

            command.ExecuteNonQuery();
        }

        public int GetAllCount()
        {
            using SqlCommand command = new() { Connection = _connection };
            command.CommandText = "SELECT COUNT(*) FROM GroupUser WHERE DateDelete IS NULL";
            return Convert.ToInt32(command.ExecuteScalar());
        }
    }
}
