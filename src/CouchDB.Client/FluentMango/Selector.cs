using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace CouchDB.Client.FluentMango
{
    public class Selector
    {
        public string Field { get; private set; }
        public object Value { get; private set; }
        public SelectorOperator? SelectorOp { get; set; }
        public CombinationOperator? CombinationOp { get; set; }

        public Selector(string field, object value)
        {
            this.Field = field;
            this.Value = value;
        }

        public Selector(string field, SelectorOperator selectorOperator, object value)
        {
            this.Field = field;
            this.SelectorOp = selectorOperator;
            this.Value = value;
            ValidateSelectorOperator();
        }

        public Selector(string field, CombinationOperator combinationOperator, object value)
        {
            this.Field = field;
            this.CombinationOp = combinationOperator;
            this.Value = value;
            ValidateCombinationOperator();
        }

        private void ValidateSelectorOperator()
        {
            if (!SelectorOp.HasValue)
                throw new ArgumentException("The selector operator must be filled");

            if (SelectorOp.Value == SelectorOperator.In || SelectorOp.Value == SelectorOperator.Nin || SelectorOp.Value == SelectorOperator.Mod)
            {
                // enforce value be array
                if (Value.GetType().BaseType != typeof(System.Array))
                    throw new ArgumentException("The value of type In, Nin and Mod must be array");
            }

            if (SelectorOp.Value == SelectorOperator.Size)
            {
                // enforce value be array
                if (Value.GetType() != typeof(System.Int16) && Value.GetType() != typeof(System.Int32) && Value.GetType() != typeof(System.Int64))
                    throw new ArgumentException("The value of type size must be numeric");
            }

            if (SelectorOp.Value == SelectorOperator.Regex)
            {
                // enforce value be array
                if (Value.GetType() != typeof(System.String))
                    throw new ArgumentException("The value of type regex must be string");
            }

            if (SelectorOp.Value == SelectorOperator.Exists)
            {
                // enforce value be array
                if (Value.GetType() != typeof(System.Boolean))
                    throw new ArgumentException("The value of type must be boolean");
            }

            if (SelectorOp.Value == SelectorOperator.Type)
            {
                // enforce value be array
                if (Value.GetType() != typeof(System.String) && Value.GetType() != typeof(TypeValues))
                    throw new ArgumentException("The value of type must be string or TypeValues");

                if (Value.GetType() == typeof(System.String))
                {
                    TypeValues res;
                    if (!Enum.TryParse<TypeValues>(Value.ToString(), true, out res))
                    {
                        throw new ArgumentException("The value of type must a string of TypeValues");
                    }
                }
            }
        }

        private void ValidateCombinationOperator()
        {
            if (!CombinationOp.HasValue)
                throw new ArgumentException("The combination operator must be filled");

            if (CombinationOp.Value == CombinationOperator.And || CombinationOp.Value == CombinationOperator.Or || CombinationOp.Value == CombinationOperator.Nor || CombinationOp.Value == CombinationOperator.All)
            {
                // enforce value be array
                if (Value.GetType().BaseType != typeof(System.Array))
                    throw new ArgumentException("The value of type And, Or, Nor, and All must be array");
            }

            //if (CombinationOp.Value == CombinationOperator.Not || CombinationOp.Value == CombinationOperator.ElementMatch || CombinationOp.Value == CombinationOperator.AllMatch)
            //{
            //    // enforce value be array
            //    if (Value.GetType() != typeof(Selector))
            //        throw new ArgumentException("The value of type size must be selector");
            //}
        }

        public override string ToString()
        {
            var obj = new JObject();

            if (!this.SelectorOp.HasValue && !this.CombinationOp.HasValue)
            {
                obj.Add(this.Field, JToken.FromObject(this.Value));
            }

            else if (this.SelectorOp.HasValue)
            {
                var opeval = new JObject();
                if (this.Value != null)
                    opeval.Add(SelectorOperatorString(SelectorOp.Value), SelectorOperatorValue(SelectorOp.Value, this.Value));
                else
                    opeval.Add(SelectorOperatorString(SelectorOp.Value), null);

                obj.Add(this.Field, opeval);
            }
            else if (CombinationOp.HasValue)
            {
                if (CombinationOp.Value == CombinationOperator.Not)
                {
                    var j = new Selector(this.Field, JToken.FromObject(this.Value));
                    obj.Add("$not", JObject.Parse(j.ToString()));
                }
                else if (CombinationOp.Value == CombinationOperator.Or || CombinationOp.Value == CombinationOperator.And)
                {
                    JArray arr = new JArray();
                    foreach (var item in (IEnumerable<Selector>)this.Value)
                    {
                        var obj1 = JObject.Parse(item.ToString());
                        arr.Add(obj1);
                    }

                    if (CombinationOp.Value == CombinationOperator.Or)
                    {
                        obj.Add("$or", arr);
                    }
                    else if (CombinationOp.Value == CombinationOperator.And)
                    {
                        obj.Add("$and", arr);
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                }
                else if (CombinationOp.Value == CombinationOperator.Nor)
                {
                    JArray arr = new JArray();
                    foreach (var item in (IEnumerable<Selector>)this.Value)
                    {
                        var obj2 = JObject.Parse(item.ToString());
                        arr.Add(obj2);
                    }
                    obj.Add("$nor", arr);
                }
                else if (CombinationOp.Value == CombinationOperator.All)
                {
                    var sel2 = new Selector("$all", this.Value);
                    obj.Add(this.Field, JToken.Parse(sel2.ToString()));
                }
                else if (CombinationOp.Value == CombinationOperator.ElementMatch)
                {
                    var sel2 = new Selector("$elemMatch", SelectorOperator.Equals, this.Value);
                    obj.Add(this.Field, JToken.Parse(sel2.ToString()));
                }
                else if (CombinationOp.Value == CombinationOperator.AllMatch)
                {
                    var sel2 = new Selector("$allMatch", SelectorOperator.Equals, this.Value);
                    obj.Add(this.Field, JToken.Parse(sel2.ToString()));
                }
            }
            else
            {
                throw new NotImplementedException();
            }

            return obj.ToString();
        }

        private JToken SelectorOperatorValue(SelectorOperator selectorOperator, object value)
        {
            if (selectorOperator == SelectorOperator.Exists)
            {
                return JToken.FromObject(this.Value.ToString().ToLower());
            }
            else
            {
                return JToken.FromObject(this.Value);
            }
        }

        private string SelectorOperatorString(SelectorOperator selectorOperator)
        {
            switch (selectorOperator)
            {
                case FluentMango.SelectorOperator.LessThan:
                    return "$lt";
                case FluentMango.SelectorOperator.LessEqualsThan:
                    return "$lte";
                case FluentMango.SelectorOperator.Equals:
                    return "$eq";
                case FluentMango.SelectorOperator.NotEquals:
                    return "$ne";
                case FluentMango.SelectorOperator.GreaterEqualsThan:
                    return "$gte";
                case FluentMango.SelectorOperator.GreaterThan:
                    return "$gt";
                case FluentMango.SelectorOperator.Exists:
                    return "$exists";
                case FluentMango.SelectorOperator.Type:
                    return "$type";
                case FluentMango.SelectorOperator.In:
                    return "$in";
                case FluentMango.SelectorOperator.Nin:
                    return "$nin";
                case FluentMango.SelectorOperator.Size:
                    return "$size";
                case FluentMango.SelectorOperator.Mod:
                    return "$mod";
                case FluentMango.SelectorOperator.Regex:
                    return "$regex";
                default:
                    return null;
            }
        }
    }
}
