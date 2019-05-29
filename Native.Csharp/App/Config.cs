using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Native.Csharp.App
{
    /// <summary>
    /// 配置
    /// </summary>
    public class Config
    {

        #region 私有变量
        private List<long> m_canSendGroup = new List<long>();
        private List<long> m_managersQQ = new List<long>();
        private long m_sendGroupMsgDelay = 5000;
        private string m_sendGroupMsgContent = "群消息定时发送测试";

        private const long defaultManagerQQ = 735487435;

        /// <summary>
        /// 已经发送私聊的QQ
        /// </summary>
        private Dictionary<long, List<long>> m_alreadySendQQ = new Dictionary<long, List<long>>();

        /// <summary>
        /// 给群制定消息，不同的群发送不同的消息
        /// </summary>
        private Dictionary<long, string> m_sendGroupMsgDic = new Dictionary<long, string>();


        public List<long> CanSendGroup
        {
            get
            {
                return m_canSendGroup;
            }

            set
            {
                m_canSendGroup = value;
            }
        }

        public List<long> ManagersQQ
        {
            get
            {
                return m_managersQQ;
            }

            set
            {
                m_managersQQ = value;
            }
        }

        public long SendGroupMsgDelay
        {
            get
            {
                return m_sendGroupMsgDelay;
            }

            set
            {
                m_sendGroupMsgDelay = value;
            }
        }

        public string SendGroupMsgContent
        {
            get
            {
                return m_sendGroupMsgContent;
            }

            set
            {
                m_sendGroupMsgContent = value;
            }
        }

        public Dictionary<long, List<long>> AlreadySendQQ
        {
            get
            {
                return m_alreadySendQQ;
            }

            set
            {
                m_alreadySendQQ = value;
            }
        }

        public Dictionary<long, string> SendContentDic
        {
            get
            {
                return m_sendGroupMsgDic;
            }

            set
            {
                m_sendGroupMsgDic = value;
            }
        }
        #endregion
        public Config()
        {
            m_managersQQ.Add(735487435);
            m_managersQQ.Add(2947163687);

            m_sendGroupMsgDic.Add(416554225, "健身健美交流群 测试1");
        }

        /// <summary>
        /// 添加指定群的发送消息
        /// </summary>
        /// <param name="group"></param>
        /// <param name="msg"></param>
        /// <param name="fromQQ"></param>
        public void AddGroupSendMsg(long group,string msg,long fromQQ = defaultManagerQQ)
        {
            bool isExist = false;
            List<Sdk.Cqp.Model.Group> groups = new List<Sdk.Cqp.Model.Group>();
            Common.CqApi.GetGroupList(out groups);

            foreach (Sdk.Cqp.Model.Group item in groups)
            {
                if(item.Id == group)
                {
                    isExist = true;
                    break;
                }
            }

            if (!isExist)
            {
                Common.CqApi.SendPrivateMessage(fromQQ, "机器人还没加入【" + group + "】群");
                return;
            }

            if (m_sendGroupMsgDic.ContainsKey(group))
            {
                m_sendGroupMsgDic[group] = msg;
            }else
            {
                m_sendGroupMsgDic.Add(group, msg);
            }
            QueryGroupSendMsg(fromQQ);
        }

        /// <summary>
        /// 移除指定群的所有发送消息
        /// </summary>
        /// <param name="group"></param>
        /// <param name="fromQQ"></param>
        public void RemoveGroupSendMsg(long group, long fromQQ = defaultManagerQQ)
        {
            foreach (var item in m_sendGroupMsgDic)
            {
                if (item.Key != group)
                    continue;
                m_sendGroupMsgDic.Remove(item.Key);
            }

            for (int i = 0; i < m_sendGroupMsgDic.Count; i++)
            {

            }

            QueryGroupSendMsg(fromQQ);
        }

        /// <summary>
        /// 查询-发送群消息列表
        /// </summary>
        /// <param name="fromQQ"></param>
        public void QueryGroupSendMsg(long fromQQ = defaultManagerQQ)
        {
            StringBuilder strBuilder = new StringBuilder();
            int id = 0;
            strBuilder.Append("查询-发送群消息列表\r\n");
            foreach (var item in m_sendGroupMsgDic)
            {
                strBuilder.Append(id+1);
                strBuilder.Append("  ");
                strBuilder.Append(item.Key);
                strBuilder.Append(" : ");
                strBuilder.Append(item.Value);
                strBuilder.Append("\r\n");
                id++;
            }
            Common.CqApi.SendPrivateMessage(fromQQ, strBuilder.ToString());
        }
    }
}
