using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace ConsoleApp1
{
    class TypeInfo
    {
        private Dictionary<Type, IList> values = new Dictionary<Type, IList>();

        public TypeInfo(object root)
        {
            values.Add(root.GetType(), new ArrayList { root });
        }

        private object GetValue(Type type)
        {
            if (!values.ContainsKey(type))
                return null;

            var typeList = values[type];

            return typeList[typeList.Count - 1];
        }

        private void AddValue(Type type, object value)
        {
            if (!values.ContainsKey(type))
                values.Add(type, new ArrayList());

            var typeList = values[type];
            typeList.Add(value);
        }

        public void AddValue(PropertyInfo property)
        {
            AddValue(property.PropertyType, property.GetValue(GetValue(property.DeclaringType)));
        }

        public object GetValue(PropertyInfo property)
        {
            var parent = GetValue(property.DeclaringType);

            if (parent == null)
                return null;

            return property.GetValue(parent);
        }
    }
}
