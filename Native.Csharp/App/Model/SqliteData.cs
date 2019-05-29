using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Native.Csharp.App.Model
{
    public class SqliteData
    {
        public class UserInfo : EventArgsBase
        {
            public int QQ { get; set; }
            public string NickName { get; set; }
        }

        public class CreateDB : EventArgsBase
        {
            /// <summary>
            /// 数据库名称
            /// </summary>
            public string dbName { get;set;}
            /// <summary>
            /// 数据库
            /// </summary>
            public string dbPath { get; set; }
        }
    }
}
