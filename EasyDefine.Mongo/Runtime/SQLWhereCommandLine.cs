using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace EasyDefine.Mongo.Runtime
{
    public class SQLWhereCommandLine
    {
        public SQLCommandLinkType LinkType { get; set; }

        /// <summary>
        /// 当前句命令
        /// </summary>
        public SQLCommandWordEnum wordEnum { get; set; }

        /// <summary>
        /// 待推演字段
        /// </summary>
        public string Field { get; set; }

        /// <summary>
        /// 待设置的值
        /// </summary>
        public object Value { get; set; }

        public string _id { get; set; }

        public string Command { get; set; }

        public string LinkedId { get; set; }

        /// <summary>
        /// 是否为链接节点
        /// </summary>
        public bool IsLinked
        {
            get
            {
                if (!string.IsNullOrEmpty(LinkedId))
                {
                    return true;
                }
                return false;
            }
        }

        public override string ToString()
        {
            if(!string.IsNullOrEmpty(Command))
            return $@"{_id} >> {Command}";
            return $@"{_id} >> Linked >> {LinkedId}";
        }

        public bool CommandReader() {
            if (!string.IsNullOrEmpty(Command)) {
                var words = Command.Trim().Split(" ");
                if (words.Length == 3)
                {
                    //提取推演字段
                    this.Field = words[0].Trim();
                    //先提取操作符
                    switch (words[1].Trim().ToLower())
                    {
                        case "=": { this.wordEnum = SQLCommandWordEnum.Equal; break; }
                        case ">": { this.wordEnum = SQLCommandWordEnum.Greater; break; }
                        case "<": { this.wordEnum = SQLCommandWordEnum.Lesser; break; }
                        case ">=": { this.wordEnum = SQLCommandWordEnum.GreaterEqual; break; }
                        case "<=": { this.wordEnum = SQLCommandWordEnum.LesserEqual; break; }
                        case "!=": { this.wordEnum = SQLCommandWordEnum.NotEqual; break; }
                        case "<>": { this.wordEnum = SQLCommandWordEnum.NotEqual; break; }
                        case "like": { this.wordEnum = SQLCommandWordEnum.Like; break; }
                        default: { this.wordEnum = SQLCommandWordEnum.Equal; break; }
                    }
                    //提取值
                    this.Value = words[2].Trim();
                    return true;
                }
            }
            return false;
        }

        public FilterDefinition<T> ToBuilder<T, TField>(TField field)
        {
            try
            {
                var _fieldName = this.Field;
                //构造Lambda
                ParameterExpression pe = Expression.Parameter(typeof(T), "e");
                MemberExpression me = Expression.Property(pe, _fieldName);
                ConstantExpression constant = Expression.Constant(field, typeof(TField));
                BinaryExpression body = Expression.Equal(me, constant);
                //Func<T, bool> expression = ExpressReturn;
                Expression<Func<T, TField>> x = Expression.Lambda<Func<T, TField>>(Expression.Property(pe, _fieldName), pe);
                return Builders<T>.Filter.Gte(x, field);
            }
            catch (Exception ex) {
                throw ex;
            }
        }

        private bool ExpressReturn<T>(T t) {
            return true;
        }
    }

    public enum SQLCommandWordEnum {
        Equal, //等于
        GreaterEqual, //大于等于
        LesserEqual, //小于等于
        Like, //相似
        Greater, //大于
        Lesser, //小于
        NotEqual , //不等于
    }

    public enum SQLCommandLinkType {
        And,
        Or
    }
}
