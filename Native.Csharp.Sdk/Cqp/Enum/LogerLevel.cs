using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Native.Csharp.Sdk.Cqp.Enum
{
	/// <summary>
	/// 日志等级
	/// </summary>
	public enum LogerLevel
	{
		/// <summary>
		/// 调试
		/// </summary>
		Debug = 0,
		/// <summary>
		/// 信息
		/// </summary>
		Info = 10,
		/// <summary>
		/// 信息_成功
		/// </summary>
		Info_Success = 11,
		/// <summary>
		/// 信息_接收
		/// </summary>
		Info_Receive = 12,
		/// <summary>
		/// 信息_发送
		/// </summary>
		Info_Send = 13,
		/// <summary>
		/// 警告
		/// </summary>
		Warning = 20,
		/// <summary>
		/// 错误
		/// </summary>
		Error = 30,
		/// <summary>
		/// 严重错误
		/// </summary>
		Fatal = 40
	}
    //https://d.cqp.me/index.php?title=Pro/%E5%BC%80%E5%8F%91/Error&direction=prev&oldid=672
    public enum SendError
    {
        /// <summary>
        /// 成功
        /// </summary>
        Success = 0,
        /// <summary>
        /// 请求发送失败
        /// </summary>
        RequsetError = -1,
        /// <summary>
        /// 未收到服务器回复，可能未发送成功
        /// </summary>
        NotRecive = -2,
        /// <summary>
        /// 消息过长或为空
        /// </summary>
        MessageIsError = -3,
        /// <summary>
        /// 消息解析过程异常
        /// </summary>
        MessageDEcodeError = -4,
        /// <summary>
        /// 账号不在该群内，消息无法发送
        /// </summary>
        NotInGroup = -9,
        /// <summary>
        /// 由于未知原因，操作失败
        /// </summary>
        UnknownUError = -11,
        /// <summary>
        /// 群未开启匿名发言功能，或匿名账号被禁言
        /// </summary>
        IsAnonymous = -15,
        /// <summary>
        /// 账号不在群内或网络错误，无法退出/解散该群
        /// </summary>
        NotInGroupCanQuit = -16,
        /// <summary>
        /// 账号为群主，无法退出该群
        /// </summary>
        IsOwner = -17,
        /// <summary>
        /// 账号非群主，无法解散该群
        /// </summary>
        IsNoOwner = -18,
        /// <summary>
        /// 临时消息未建立或已失效
        /// </summary>
        TemporaryMaessageNotEstablished = -19,
        /// <summary>
        /// 找不到与目标QQ的关系，消息无法发送  
        /// </summary>
        NotFindContact = -23,
        /// <summary>
        /// 被禁言
        /// </summary>
        IsBan = -34,
        /// <summary>
        /// 参数错误或权限不足   
        /// </summary>
        InsufficientPermissions = -39,

    }
}
