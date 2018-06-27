using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            var user = new User
            {
                Name = "Marko",
                Surname = "Jelačić",
                ID = new Guid(),
                Point = new Point
                {
                    X = 12,
                    OfLife = new OfLife
                    {
                        Beginning = DateTime.MinValue,
                        End = DateTime.MaxValue,
                        String = "String"
                    },
                    Y = 33
                },
                Items = new List<int> { 1, 2, 3 }
            };

            var root = new XElement("Root", Serialize(user));
            Console.WriteLine(root);
        }

        private static XElement Serialize<T>(T item)
        {
            //Element that is going to be constructed
            var element = new XElement(typeof(T).Name);

            //Variable that will hold XML element that will have its child values appended to it
            var currentElement = element;

            //Helper collection for getting values of certain properties
            var types = new TypeInfo(item);

            //Start the recursion by calling it with the root type
            Recursion(typeof(T));

            void Recursion(Type type)
            {
                var properties = type.GetProperties();

                foreach (var prop in properties)
                {
                    //If a property's type is a value type, add it to the XML
                    if (prop.PropertyType.IsValueType || prop.PropertyType == typeof(string))
                    {
                        if(prop.GetIndexParameters().Length == 0)
                        {
                            //get value from type collection and store it into the node
                            currentElement.Add(new XElement(prop.Name, types.GetValue(prop)));
                        }
                        else
                            //ignore indexer
                            currentElement.Add(new XElement(prop.Name, new XAttribute("indexer", true)));

                    }
                    //If a property is the same type as its declaring type, do nothing to avoid stack overflow exception
                    else if (prop.DeclaringType == prop.PropertyType) { }

                    else //if a property is a reference type...
                    {
                        //add it to the type collection
                        types.AddValue(prop);
                        
                        //parent node of the current property
                        var parentXML = new XElement(prop.Name);
                        //add it to the current node and set it to be current node
                        currentElement.Add(parentXML);
                        currentElement = parentXML;

                        //go through its child properties
                        Recursion(prop.PropertyType);
                    }
                }

                //If the type has no children or if they are all value types or they have the same type as parent, set the current element to the root element
                if (properties.Length == 0 || properties.All(prop => prop.PropertyType.IsValueType || prop.PropertyType == typeof(string) || prop.PropertyType == type))
                    currentElement = element;
            }

            return element;
        }
    }
}
