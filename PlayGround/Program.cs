using System;
using System.IO;
using System.Linq;
using Live2k.Core.Model;
using Live2k.Core.Model.Base;
using Live2k.Core.Model.Basic;
using Live2k.Core.Model.Basic.Commodities;
using Live2k.Core.Model.Basic.Relationships;
using Live2k.Core.Serializer;
using Live2k.Core.Utilities;
using Live2k.MongoDb;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace PlayGround
{
    class Program
    {
        private static Mediator _mediator;

        static void Main(string[] args)
        {
            // Login
            LoginOrRegister();

            // Test node reporitory
            TestNodeRepoeitory();
        }

        private static void TestNodeRepoeitory()
        {
            // Add a new node
            NewNode();

            // Get a sdi
            var node = GetNode();
            var sdi = GetNode<SDI>();
            var rev = sdi.Revise();
            rev.Description = "Recently added";

            sdi.Save();
        }

        private static Node GetNode()
        {
            return Node.GetFromDatabase(_mediator, a => a.Label == "SDI-1");
        }

        private static T GetNode<T>() where T: Node
        {
            return Node.GetFromDatabase<T>(_mediator, a => a.Label == "SDI-2");
        }

        private static void NewNode()
        {
            // new SDI
            var sdi = Node.NewNode<SDI>(_mediator, null, "Final try",
                new Tuple<string, object>("DataCode", "ewrew"),
                new Tuple<string, object>("Section", "3333"));
            sdi.Save();

            var comodity = Node.NewNode<Commodity>(_mediator, "Onthefly", "this is a commodity");
            comodity.Save();

            var co = Node.NewNode<ControlObject>(_mediator, "=122453/2132", "ssss");
            co.Save();
        }

        private static void LoginOrRegister()
        {
            // ask if user wants to login or register
            Console.Write("Are you already registered? (Y/N)");
            var answ = Console.ReadKey();
            Console.Clear();
            if (answ.Key == ConsoleKey.Y)
            {
                Login();
            }
            else if (answ.Key == ConsoleKey.N)
            {
                Register();
            }
            else
            {
                Console.WriteLine("Not valid carachter. Press any key to terminate");
                Console.Read();
            }
        }

        private static void Register()
        {
            var user = new User(GetUserInput("Please enter you email address: "));
            user.FirstName = GetUserInput("First name: ");
            user.LastName = GetUserInput("Last name: ");
            user.Birthday = new DateTime(int.Parse(GetUserInput("Birth year: ")),
                int.Parse(GetUserInput("Birth month: ")),
                int.Parse(GetUserInput("Birth day: ")));
            _mediator = new Mediator(user);
        }

        private static string GetUserInput(string message)
        {
            Console.Write(message);
            return Console.ReadLine();
        }

        private static void Login()
        {
            Console.Write("Please enter you email address: ");
            var email = Console.ReadLine();
            _mediator = new Mediator(email);
        }
    }
}
