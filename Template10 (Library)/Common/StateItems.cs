﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template10.Common
{
    public class StateItems : Dictionary<StateItemKey, Object>
    {

        public KeyValuePair<StateItemKey, Object> Add(Type type, string key, object value)
        {
            if (Contains(type, key))
                throw new ArgumentException("Same type+key exists.");
            var stateKey = new StateItemKey(type, key);
            this.Add(stateKey, value);
            var item = new KeyValuePair<StateItemKey, object>(stateKey, value);
            return item;
        }



        public void Remove(Type type) // <-- would not be called oftenly, okey to linear search
        {
            var willRemove = this.Keys.Where(x => x.Type == type).ToList();
            willRemove.ForEach(x => this.Remove(x));
        }

        public void Remove(Type type, string key) //
        {
            this.Remove(new StateItemKey(type, key));
        }

        //this method is not a good idea in any senioro cos values can always be dupe,
        //and the eqcomparer is fixed, boxed values might cause unexpected result.
        //Hope this can be changed to    `public void Remove(Func<KVP<StateItemKey,object>,bool> condition>)`        
        public void Remove(object value)
        {
            var willRemove = this.Values.Where(x => x == value).ToList();
            willRemove.ForEach(x => this.Remove(x));
        }


        //not a good idea just like `public void Remove(object value)`  cause lack of eqcomparer
        //Hope can be changed to    `public void  Contains(Type type, string key,Func<object,bool> valueCondition>)`  
        public bool Contains(Type type, string key, object value)
        {
            object tryGetValue;
            if (this.TryGetValue(new StateItemKey(type, key), out tryGetValue))
            {
                return tryGetValue == value;
            }
            return false;

        }


        public bool Contains(Type type, string key)
        {
            return this.ContainsKey(new StateItemKey(type, key));

        }

        //Hope can be changed to    public void  Contains(Func<KVP<StateItemKey,object>,bool> condition>)  
        public bool Contains(object value)
        {
            return this.ContainsValue(value);

        }
        //public  bool Contains(StateItemKey stateItemKey)
        //{
        //    return this.ContainsKey(stateItemKey);
        //}

        public T Get<T>(string key)
        {
            object tryGetValue;
            if (this.TryGetValue(new StateItemKey(typeof(T), key), out tryGetValue))
            {
                return (T)tryGetValue;
            }
            throw new KeyNotFoundException();

        }

        public bool TryGet<T>(string key, out T value)
        {
            object tryGetValue;
            var found = this.TryGetValue(new StateItemKey(typeof(T), key), out tryGetValue);
            value = (T)tryGetValue;
            return found;
        }
    }
}
