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
        private long m_managerQQ = 735487435;
        private List<long> m_managersQQ = new List<long>();

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

        public long ManagerQQ
        {
            get
            {
                return m_managerQQ;
            }

            set
            {
                m_managerQQ = value;
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
        #endregion

        Config()
        {
            m_managersQQ.Add(735457435);
            m_managersQQ.Add(2947163687);
        }
    }
}
