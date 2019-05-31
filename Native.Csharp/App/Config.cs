using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Native.Csharp.Sdk.Cqp.Model;

namespace Native.Csharp.App
{
    /// <summary>
    /// 配置
    /// </summary>
    public class Config
    {
        #region 私有变量

        /// <summary>
        /// 可以在群里发消息的群列表
        /// </summary>
        private List<long> m_canSendGroup = new List<long>();

        /// <summary>
        /// 不能私聊的群
        /// </summary>
        private List<long> m_cannotSendPrivateGroup = new List<long>();
        /// <summary>
        /// QQ机器人管理员
        /// </summary>
        private List<long> m_managersQQ = new List<long>();
        /// <summary>
        /// 群里发送消息间隔时间
        /// </summary>
        private int m_sendGroupMsgDelay = 5000;

        /// <summary>
        /// 已经发送私聊的QQ
        /// </summary>
        private Dictionary<long, List<long>> m_alreadySendQQ = new Dictionary<long, List<long>>();

        /// <summary>
        /// 给群制定消息，不同的群发送不同的消息
        /// </summary>
        private Dictionary<long, string> m_sendGroupMsgDic = new Dictionary<long, string>();
        /// <summary>
        /// 给群指定私聊成员消息，不同的群私聊的消息不一样
        /// </summary>
        private Dictionary<long, string> m_sendGroupPrivateMsgDic = new Dictionary<long, string>();

        /// <summary>
        /// QQ机器人最高权限管理员列表，可以有多个
        /// </summary>
        private List<long> m_managerGroups = new List<long>();

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

        public int SendGroupMsgDelay
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

        public Dictionary<long, List<long>> AlreadySendQQ
        {
            get { return m_alreadySendQQ; }
            set { m_alreadySendQQ = value; }
        }

        public Dictionary<long, string> SendGroupMsgDic
        {
            get { return m_sendGroupMsgDic; }
            set { m_sendGroupMsgDic = value; }
        }

        public List<long> CannotSendPrivateGroup {
            get {
                return m_cannotSendPrivateGroup;
            }

            set {
                m_cannotSendPrivateGroup = value;
            }
        }

        /// <summary>
        /// 给群指定私聊成员消息，不同的群私聊的消息不一样
        /// </summary>
        public Dictionary<long, string> SendGroupPrivateMsgDic {
            get {
                return m_sendGroupPrivateMsgDic;
            }

            set {
                m_sendGroupPrivateMsgDic = value;
            }
        }

        #endregion
        public Config()
        {
            m_managersQQ.Add(735487435);
            m_managersQQ.Add(2947163687);

            SendGroupMsgDic.Add(416554225, "健身健美交流群 测试1");

            InitManagerGroups();
        }

        private void InitManagerGroups()
        {
            List<Group> groups = new List<Group>();
            Common.CqApi.GetGroupList(out groups);
            long myQQ = Common.CqApi.GetLoginQQ();
            GroupMember groupMember;
            foreach (Group item in groups)
            {
                groupMember = new GroupMember();
                Common.CqApi.GetMemberInfo(item.Id, myQQ, out groupMember, true);
                if (groupMember.PermitType != Sdk.Cqp.Enum.PermitType.None)
                {
                    //我是管理
                    m_managerGroups.Add(item.Id);
                }
            }
        }

        public bool IsManager(long group)
        {
            return m_managerGroups.Contains(group);
        }

        /// <summary>
        /// 添加指定群的发送消息
        /// </summary>
        /// <param name="group"></param>
        /// <param name="msg"></param>
        /// <param name="fromQQ"></param>
        public void AddGroupSendMsg(long group,string msg,long fromQQ)
        {
            bool isExist = false;
            List<Group> groups = new List<Group>();
            Common.CqApi.GetGroupList(out groups);

            foreach (Group item in groups)
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

            if (SendGroupMsgDic.ContainsKey(group))
            {
                SendGroupMsgDic[group] = msg;
            }else
            {
                SendGroupMsgDic.Add(group, msg);
            }
            QueryGroupSendMsg(fromQQ);
        }

        /// <summary>
        /// 移除指定群的所有发送消息
        /// </summary>
        /// <param name="group"></param>
        /// <param name="fromQQ"></param>
        public void RemoveGroupSendMsg(long group, long fromQQ)
        {
            foreach (var item in SendGroupMsgDic)
            {
                if (item.Key != group)
                    continue;
                SendGroupMsgDic.Remove(item.Key);
            }

            QueryGroupSendMsg(fromQQ);
        }

        /// <summary>
        /// 查询-发送群消息列表
        /// </summary>
        /// <param name="fromQQ"></param>
        public void QueryGroupSendMsg(long fromQQ)
        {
            StringBuilder strBuilder = new StringBuilder();
            int id = 0;
            strBuilder.Append("查询-发送群消息列表\r\n");
            foreach (var item in SendGroupMsgDic)
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
