using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyDefine.Mongo.Runtime
{
    public class SqlWhere
    {
        private string _sql = "";
        private List<string> Words = new List<string>();

        /// <summary>
        /// 开始位置堆栈,先进后出
        /// </summary>
        private Stack<int> beginPos = new Stack<int>();

        /// <summary>
        /// 执行命令队列,先进先出
        /// </summary>
        private Queue<SentencesUnit> TempExecuteSentence = new Queue<SentencesUnit>();

        /// <summary>
        /// 执行分组列表
        /// </summary>
        private List<TempCommandQueue> tempCommandQueues = new List<TempCommandQueue>();

        public SqlWhere(string wherestr)
        {
            _sql = $@"({wherestr})";
        }

        /// <summary>
        /// 先解析为单元
        /// </summary>
        /// <returns></returns>
        public void ToSQLUnit()
        {
            List<string> rets = new List<string>();
            // Step 1 拆解左括号
            foreach (var unit in this._sql.Split(' '))
            {
                if (!string.IsNullOrEmpty(unit))
                {
                    var _unit = unit;
                    while (_unit.IndexOf("(") > -1)
                    {
                        var sub = _unit.Substring(0, _unit.IndexOf("("));
                        if (!string.IsNullOrEmpty(sub))
                        {
                            rets.Add(sub);
                        }
                        rets.Add("(");
                        _unit = _unit.Substring(_unit.IndexOf("(") + 1);
                    }
                    rets.Add(_unit);
                }
            }
            // Step 2 拆解右括号
            foreach (var unit in rets) {
                //没有右括号
                if (unit.IndexOf(")") < 0)
                {
                    Words.Add(unit);
                }
                else {
                    var _unit = unit;
                    while (_unit.IndexOf(")") > -1)
                    {
                        var sub = _unit.Substring(0, _unit.IndexOf(")"));
                        if (!string.IsNullOrEmpty(sub))
                        {
                            Words.Add(sub);
                        }
                        Words.Add(")");
                        _unit = _unit.Substring(_unit.IndexOf(")") + 1);
                    }
                }
            }
            //foreach (var word in Words) {
            //    Console.WriteLine(word);
            //}
        }

        /// <summary>
        /// 生成执行元件
        /// </summary>
        public void ToMongoBuilders<T>()
        {
            FilterDefinition<T> filterDefinition = GetTempBuilder<T>(tempCommandQueues.Last());
            if (filterDefinition != null)
            {
                Console.WriteLine(filterDefinition);
            }
        }

        /// <summary>
        /// 递归读取CommandQueue
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private FilterDefinition<T> GetTempBuilder<T>(TempCommandQueue comm) {
            Console.WriteLine($@"TempCommandQueue>>{comm.GroupId}");
            FilterDefinition<T> filterDefinition = null;
            if (!comm.HasExcuted) //未被调用过
            {
                foreach (var communit in comm.TempLine)
                {
                    if (communit.CommandReader()) //解析自身命令
                    {
                        if (filterDefinition == null)
                        {
                            filterDefinition = RunBuilder<T>(communit);
                        }
                        else
                        {
                            filterDefinition &= RunBuilder<T>(communit);
                            //filterDefinition |= communit.ToBuilder<T, string>("沙");
                        }
                    }
                    else
                    {
                        if (communit.IsLinked)
                        {
                            //调用Link命令，转到其他解析
                            var id = communit.LinkedId;
                            var temp = tempCommandQueues.Where(e => e.GroupId == id).First();
                            var ret_sub_cline = GetTempBuilder<T>(temp);
                            if (ret_sub_cline != null)
                            {
                                (filterDefinition) &= ret_sub_cline;
                            }
                        }
                    }
                }
                comm.HasExcuted = true;
            }
            return filterDefinition;
        }

        /// <summary>
        /// 反射执行RunBuilder
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Field"></param>
        /// <param name="communit"></param>
        private FilterDefinition<T> RunBuilder<T>(SQLWhereCommandLine communit)
        {
            try
            {
                var paramType = typeof(T).GetProperty(communit.Field).PropertyType;
                var mi = typeof(SQLWhereCommandLine).GetMethod("ToBuilder");
                var ret = mi.MakeGenericMethod(new Type[] { typeof(T), paramType }).Invoke(communit, new object[] { Convert.ChangeType(communit.Value, paramType) });
                return (FilterDefinition<T>)ret;
            }
            catch (Exception ex)
            {
                //这里一般的异常是找不到对象中的属性
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine($@"找不到对象(表)中的属性(字段) 或 赋值错误,请查验:{ex.Message}");
                Console.ForegroundColor = ConsoleColor.White;
                throw ex;
            }
        }
        

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="tempCommand"></param>
        private void ExecutedTempCommandQueue(TempCommandQueue tempCommand) {

        }

        /// <summary>
        /// 语句分析
        /// </summary>
        public void Sentence() {
            //循环词组
            for (int i = 0; i < Words.Count; i++)
            {
                if (Words[i] == "(")
                {
                    beginPos.Push(i);
                }
                else if (Words[i] == ")")
                {
                    var endpos = i;
                    var beginpos = beginPos.Count > 0 ? beginPos.Pop() : 0;
                    StringBuilder ExecSentence = new StringBuilder();
                    for (var x = beginpos; x <= endpos; x++)
                    {
                        ExecSentence.Append(Words[x] + " ");
                    }
                    //if (i + 1 < Words.Count)
                    //{
                    //    TempExecuteSentence.Enqueue(Words[i + 1]);
                    //}
                    TempExecuteSentence.Enqueue(new SentencesUnit { Command = ExecSentence.ToString(), BeginPos = beginpos, EndPos = endpos });
                }
            }
        }

        public void GetWhereTrees() {
            List<SentencesUnit> sentences = new List<SentencesUnit>();
            List<string> treelist = new List<string>();
            List<SQLWhereCommandLine> CommandLines = new List<SQLWhereCommandLine>();
            foreach (var e in TempExecuteSentence)
            {
                // Console.WriteLine(e);
                //改判断标识当前新语句e在sentens外围( eeee /....sentence executed ..../ eeee),则替换
                var nodes = sentences.FindAll(s => s.BeginPos > e.BeginPos && s.EndPos < e.EndPos);
                if (nodes.Count > 0)
                {
                    var str = e.Command;
                    foreach (var n in nodes)
                    {
                       str = str.Replace(n.Command, $@" [node{n.Id}] ");
                    }
                    treelist.Add($@"{e.Id} >>" + str);
                    //判断标识当前新语句e在sentens外围,则去除原语句，并加入新语句
                    sentences.RemoveAll(s => s.BeginPos > e.BeginPos && s.EndPos < e.EndPos);
                    sentences.Add(e);

                }
                else
                {
                    treelist.Add($@"{e.Id} >>" + e.Command);
                    //判断标识当前新语句e不在sentens外围,则添加
                    sentences.Add(e);
                }
            }
            //解析树
            //50405199-3f6f-4993-bf1e-3762bca14724 >>( Name like @Name )
            //9415faac-f6c8-4e03-8edb-cddf5a3e56a2 >>( Name = 123 or Age > 25 or  [node50405199-3f6f-4993-bf1e-3762bca14724] )
            //f5a9e106-8584-4df2-9858-c87ecede8002 >>( Name <> 123 or Age <=10 )
            //81557301-79ca-443b-84ab-ebe2949eb8e6 >>(  [node9415faac-f6c8-4e03-8edb-cddf5a3e56a2] or  [nodef5a9e106-8584-4df2-9858-c87ecede8002] and CreateDate > @NOW )

            foreach (var list in treelist) {
                var splitchars = new string[] {"or","and" };
                var _comm = list.Split(">>");

                var Id = _comm[0];

                if (_comm[1].Trim().StartsWith("(") && _comm[1].Trim().EndsWith(")")) {
                    _comm[1] = _comm[1].Replace("(", string.Empty).Replace(")", string.Empty);
                }
                var commands = _comm[1].Trim().Split(splitchars, StringSplitOptions.RemoveEmptyEntries);
                foreach (var comm in commands) {

                    if (comm.Trim().ToLower().StartsWith("[node") && comm.Trim().ToLower().EndsWith("]"))
                    {
                        var linkid = comm.Trim().Replace("[node", string.Empty).Replace("]", string.Empty);

                        CommandLines.Add(new SQLWhereCommandLine { _id = Id.Trim(), LinkedId = linkid.Trim() });
                    }
                    else
                    {
                        CommandLines.Add(new SQLWhereCommandLine { _id = Id.Trim(), Command = comm.Trim() });
                        //thetrees.Add(new WhereTree { ExecuteCode = comm, NodeType = WhereNodeType.AND,_id = _comm[0].Trim() });
                    }
                }
                //Console.WriteLine(list);
            }
            //908892ef-c6ae-4ed5-a3dd-c77c3c4cf2a6  >> Name like @Name
            //7238d3d1-e1a9-470c-9094-e14a3318d12e  >> Name = 123
            //7238d3d1-e1a9-470c-9094-e14a3318d12e  >> Age > 25
            //7238d3d1-e1a9-470c-9094-e14a3318d12e  >> Linked >> 908892ef-c6ae-4ed5-a3dd-c77c3c4cf2a6
            //779e8819-9d1c-4e18-bc16-3b77bd5d262a  >> Name <> 123
            //779e8819-9d1c-4e18-bc16-3b77bd5d262a  >> Age <=10
            //58973e7d-7605-4a37-bd30-d2946a6a6d24  >> Linked >> 7238d3d1-e1a9-470c-9094-e14a3318d12e
            //58973e7d-7605-4a37-bd30-d2946a6a6d24  >> Linked >> 779e8819-9d1c-4e18-bc16-3b77bd5d262a
            //58973e7d-7605-4a37-bd30-d2946a6a6d24  >> CreateDate > @NOW
            //测试输出顺序
            foreach (var _command in CommandLines) {
                Console.WriteLine(_command);
            }
            //执行分组
            foreach (var group in CommandLines.GroupBy(e=>e._id))
            {
                TempCommandQueue tcq = new TempCommandQueue();
                tcq.GroupId = group.FirstOrDefault()._id; //执行组id
                foreach (var commtable in group) {
                    tcq.TempLine.Enqueue(commtable);
                    //只要有一个为Link节点，则整个队列属性为Linked
                    if (commtable.IsLinked)
                    {
                        tcq.HasLinked = true;
                    }
                }
                tempCommandQueues.Add(tcq);
            }
        }

        private string RemoveStringBrackets(string str)
        {
            if (str.Trim().StartsWith("(") && str.Trim().EndsWith(")"))
            {
                var ret= str.Substring(str.IndexOf("(") + 1, str.LastIndexOf(")") - 2).Trim();
                return ret;
            }
            return str;
        }
    }
}
