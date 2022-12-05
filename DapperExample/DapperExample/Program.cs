using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using SqlMapper;
using Newtonsoft.Json;

namespace DapperExample
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Dapper ORM Test Window");
            Console.WriteLine("***********************************************************************");
            Console.WriteLine("");
            DapperExampleMethodInsertAndSelect();
            Console.WriteLine("");
            DapperExampleMethodQueryMultiple();
            Console.WriteLine("");
            DapperExampleMethodStoredProcedure();
            Console.WriteLine("");
            DapperExampleMethodStoredProcedureSaveChanges();
            Console.WriteLine("");
            Console.ReadLine();
        }

        public static void DapperExampleMethodInsertAndSelect()
        {
            Console.WriteLine("------------------------");
            Console.WriteLine("DapperExampleMethodInsertAndSelect");
            using (IDbConnection conn = new SqlConnection("Data Source=.;Initial Catalog=ToDoApp;uid=sa;pwd=*;Max Pool Size=500;"))
            {
                
                conn.Execute("insert into ToDoGroups(ToDoGroupName, ToDoGroupDescription, UserId) values (@ToDoGroupName, @ToDoGroupDescription, @UserId)",
                    new ToDoGroups
                    {
                        ToDoGroupName="Test Dapper",
                        ToDoGroupDescription="Dapper ORM Test",
                        UserId=5
                    });
                List<ToDoGroups> todoGroups = conn.Query<ToDoGroups>("select * from ToDoGroups").ToList();
                string todosgroupsStr = JsonConvert.SerializeObject(todoGroups);
                Console.WriteLine(todosgroupsStr);
            }
            Console.WriteLine("----------------------");
            Console.WriteLine("***********************************************************************");

        }

        public static void DapperExampleMethodQueryMultiple()
        {
            Console.WriteLine("------------------------");
            Console.WriteLine("DapperExampleMethodQueryMultiple");
            using (IDbConnection sqlConnection = new SqlConnection("Data Source=.;Initial Catalog=ToDoApp;uid=sa;pwd=*;Max Pool Size=500;"))
            {
                var model = sqlConnection.QueryMultiple("select * from ToDoGroups " +
                "Select * from ToDo");

                var todogroups = model.Read().ToList();
                var todos = model.Read().ToList();
                string todosgroupsStr = JsonConvert.SerializeObject(todogroups);
                string todosStr = JsonConvert.SerializeObject(todos);
                Console.WriteLine(todosgroupsStr);
                Console.WriteLine("---");
                Console.WriteLine(todosStr);
            }
            Console.WriteLine("----------------------");
            Console.WriteLine("***********************************************************************");
        }


        public static void DapperExampleMethodStoredProcedure()
        {
            Console.WriteLine("------------------------");
            Console.WriteLine("DapperExampleMethodStoredProcedure");
            using (IDbConnection sqlConnection = new SqlConnection("Data Source=.;Initial Catalog=ToDoApp;uid=sa;pwd=*;Max Pool Size=500;"))
            {
                var todos = sqlConnection.Query<ToDo>("s_Test_ToDo",  new { ToDoId = 1 }, commandType: System.Data.CommandType.StoredProcedure);
                foreach (ToDo todo in todos)
                {
                    string todoStr = JsonConvert.SerializeObject(todo);
                    Console.WriteLine(todoStr);
                }
            }
            Console.WriteLine("----------------------");
            Console.WriteLine("***********************************************************************");

        }

        public static void DapperExampleMethodStoredProcedureSaveChanges()
        {
            Console.WriteLine("------------------------");
            Console.WriteLine("DapperExampleMethodStoredProcedureSaveChanges");
            using (IDbConnection sqlConnection = new SqlConnection("Data Source=.;Initial Catalog=ToDoApp;uid=sa;pwd=*;Max Pool Size=500;"))
            {
                ToDo newToDo = new ToDo()
                {
                    TodoName="Dapper Test NEW",
                    ToDoDescription="Dapper test to do NEW",
                    IsCompleted=false,
                    UserId=5,
                    LastDatetime=DateTime.Now.AddDays(1)
                };

                var parameter = new DynamicParameters();
                parameter.Add("@Id", newToDo.Id, dbType: DbType.Int32,
                direction: ParameterDirection.InputOutput);

                parameter.Add("@ToDoName", newToDo.TodoName, dbType: DbType.String);
                parameter.Add("@ToDoDescription", newToDo.ToDoDescription, dbType: DbType.String);
                parameter.Add("@IsCompleted", newToDo.IsCompleted, dbType: DbType.Boolean);
                parameter.Add("@UserId", newToDo.UserId, dbType: DbType.Int32);
                parameter.Add("@LastDatetime", newToDo.LastDatetime, dbType: DbType.DateTime);

                sqlConnection.Execute("s_Test_Procedure_Insert", parameter, commandType: CommandType.StoredProcedure);

                //Yeni İnsert edilen Yoruma ait ID çekilir.  
                newToDo.Id = parameter.Get<int>("@Id");
                Console.WriteLine(newToDo.Id);
                Console.WriteLine("----------------------");
                Console.WriteLine("***********************************************************************");
            }
        }
    }
}
