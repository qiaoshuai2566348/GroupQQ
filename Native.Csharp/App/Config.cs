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

        public long SendGroupMsgDelay { get => m_sendGroupMsgDelay; set => m_sendGroupMsgDelay = value; }
        public string SendGroupMsgContent { get => m_sendGroupMsgContent; set => m_sendGroupMsgContent = value; }
        #endregion

        Config()
        {
            m_managersQQ.Add(735457435);
            m_managersQQ.Add(2947163687);
        }
    }
}
