using Native.Csharp.App.Interface;
using Native.Csharp.App.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using static Native.Csharp.App.Model.SqliteData;

namespace Native.Csharp.App.Event
{
	public class Event_FriendMessage : IEvent_FriendMessage
	{
		#region --公开方法--
		/// <summary>
		/// Type=201 好友已添加<para/>
		/// 处理好友已经添加事件
		/// </summary>
		/// <param name="sender">事件的触发对象</param>
		/// <param name="e">事件的附加参数</param>
		public void ReceiveFriendIncrease (object sender, FriendIncreaseEventArgs e)
		{
			// 本子程序会在酷Q【线程】中被调用，请注意使用对象等需要初始化(CoInitialize,CoUninitialize)。
			// 这里处理消息



			e.Handled = false;   // 关于返回说明, 请参见 "Event_FriendMessage.ReceiveFriendMessage" 方法
		}

		/// <summary>
		/// Type=301 收到好友添加请求<para/>
		/// 处理收到的好友添加请求
		/// </summary>
		/// <param name="sender">事件的触发对象</param>
		/// <param name="e">事件的附加参数</param>
		public void ReceiveFriendAddRequest (object sender, FriendAddRequestEventArgs e)
		{
            // 本子程序会在酷Q【线程】中被调用，请注意使用对象等需要初始化(CoInitialize,CoUninitialize)。
            // 这里处理消息

            Common.CqApi.SetFriendAddRequest("机器人添加", Sdk.Cqp.Enum.ResponseType.PASS, "机器人添加");
            e.Handled = true;
            return;
		}

        private StringBuilder handlFriendMessageResult = new StringBuilder();
        /// <summary>
        /// Type=21 好友消息<para/>
        /// 处理收到的好友消息
        /// </summary>
        /// <param name="sender">事件的触发对象</param>
        /// <param name="e">事件的附加参数</param>
        public void ReceiveFriendMessage (object sender, PrivateMessageEventArgs e)
		{
            // 本子程序会在酷Q【线程】中被调用，请注意使用对象等需要初始化(CoInitialize,CoUninitialize)。
            // 这里处理消息

            // 本子程序会在酷Q【线程】中被调用，请注意使用对象等需要初始化(CoInitialize,CoUninitialize)。
            // 这里处理消息

            bool isManager = false;

            foreach (long item in Common.Config.ManagersQQ)
            {
                Common.CqApi.AddLoger(Sdk.Cqp.Enum.LogerLevel.Info, "FriendMessage", item.ToString());
                if (item == e.FromQQ)
                    isManager = true;
            }
            Common.CqApi.AddLoger(Sdk.Cqp.Enum.LogerLevel.Info, "FriendMessage", isManager.ToString());
            if (isManager)
            {
                ManagerXmRobat(sender, e);
                return;
            }
            e.Handled = false;
			// e.Handled 相当于 原酷Q事件的返回值
			// 如果要回复消息，请调用api发送，并且置 true - 截断本条消息，不再继续处理 //注意：应用优先级设置为"最高"(10000)时，不得置 true
			// 如果不回复消息，交由之后的应用/过滤器处理，这里置 false  - 忽略本条消息
		}
        #endregion

        #region Export

        private List<string> commandList = new List<string>();

        /// <summary>
        /// 735487435 管理xm机器人
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ManagerXmRobat(object sender, PrivateMessageEventArgs e)
        {
            handlFriendMessageResult = new StringBuilder();

            try
            {
                if ((e.Msg == "指令" || e.Msg == "help" ) && Common.Config.ManagersQQ.Contains(e.FromQQ))
                {
                    PrintCommond(e);
                    return;
                }
                if (e.Msg == "开启计时器")
                {
                    isStop = false;
                    StartSendGroupMsg(e);
                }
                else if (e.Msg == "结束计时器")
                {
                    isStop = true;
                }
                else if (e.Msg.Contains("设置群发消息"))
                {
                    string splitStr = ":";
                    Common.Config.SendGroupMsgContent = e.Msg.Split(splitStr.ToCharArray())[0];
                    handlFriendMessageResult.Append("设置群发消息成功，当前群发消息为【");
                    handlFriendMessageResult.Append(Common.Config.SendGroupMsgContent);
                    handlFriendMessageResult.Append("】");
                }

                else if (e.Msg.Contains("设置计时器时间:"))
                {
                    int time = 999;
                    time = int.Parse(e.Msg.Split(':')[1]);

                    if (time == 999)
                    {
                        handlFriendMessageResult.Append("设置发送时间失败，请检查格式【设置计时器时间:10000】英文分隔符");
                    }
                    else
                    {
                        Common.Config.SendGroupMsgDelay = time;
                        handlFriendMessageResult.Append("设置发送时间成功，当前时间为");
                        handlFriendMessageResult.Append(Common.Config.SendGroupMsgDelay);
                    }
                }
                else if (e.Msg == "获取群列表")
                {
                    int id = 0;
                    List<Sdk.Cqp.Model.Group> groupList = new List<Sdk.Cqp.Model.Group>();
                    Common.CqApi.GetGroupList(out groupList);
                    if (groupList.Count > 0)
                    {
                        foreach (var item in groupList)
                        {
                            id++;
                            handlFriendMessageResult.Append(id);
                            handlFriendMessageResult.Append(":群号：");
                            handlFriendMessageResult.Append(item.Id);
                            handlFriendMessageResult.Append(":群名称：");
                            handlFriendMessageResult.Append(item.Name);
                            handlFriendMessageResult.Append("\r\n");
                        }
                    }
                }
                else if (e.Msg.Contains("获取群成员"))
                {
                    int groupId = 0;
                    List<Sdk.Cqp.Model.GroupMember> groupMembers = new List<Sdk.Cqp.Model.GroupMember>();
                    int optionResult = Common.CqApi.GetMemberList(groupId, out groupMembers);
                    if (optionResult == 0)
                    {
                        int count = 0;
                        foreach (var item in groupMembers)
                        {
                            if (int.Parse(item.Level) < 15)
                                break;
                            handlFriendMessageResult.Append(item.QQId);
                            if (count <= groupMembers.Count)
                                handlFriendMessageResult.Append(",");
                            count++;
                        }
                    }
                    else
                    {
                        handlFriendMessageResult.Append("获取群成员列表失败:");
                        handlFriendMessageResult.Append(groupId);
                        return;
                    }
                }
                else if (e.Msg.Contains("添加发送群"))
                {
                    long group = 999;
                    try
                    {
                        group = long.Parse(e.Msg.Split(':')[1]);
                    }
                    catch
                    {
                    }
                    if (group == 999)
                    {
                        handlFriendMessageResult.Append("添加发送群失败，请检查格式【添加发送群:10000】英文分隔符");
                    }
                    else
                    {
                        InsertCanSendGroup(e, group);
                        return;

                    }
                }
                else if (e.Msg.Contains("删除发送群"))
                {
                    long group = 999;

                    string splitStr = ":";
                    group = long.Parse(e.Msg.Split(splitStr.ToCharArray())[1]);

                    if (group == 999)
                    {
                        handlFriendMessageResult.Append("删除发送群失败，请检查格式【添加发送群:10000】英文分隔符");
                    }
                    else
                    {
                        RemoveCanSendGroup(e, group);
                        return;

                    }
                }
                else if (e.Msg.Contains("查询发送群") || e.Msg == "6")
                {
                    PrintCanSendGroup(e);
                    return;
                }
                else if (e.Msg == "createdb")
                {
                    CreateDB db = new CreateDB();
                    db.dbPath = "";
                    db.dbName = "MySqliteDB";

                    SQLiteHelper.CreateDB(db);
                }
                else if (e.Msg.Contains("AddGroupSendMsg"))
                {
                    //AddGroupSendMsg:123456:发送测试
                    string[] strArray = e.Msg.Split(':');
                    long group = long.Parse(strArray[1]);
                    string msg = strArray[2];
                    Common.Config.AddGroupSendMsg(group, msg, e.FromQQ);
                    return;
                }
                else if (e.Msg.Contains("RemoveSendMsg"))
                {
                    //RemoveGroupSendMsg:123456:发送测试
                    string[] strArray = e.Msg.Split(':');
                    long group = long.Parse(strArray[1]);
                    Common.Config.RemoveGroupSendMsg(group, e.FromQQ);
                    return;
                }
                else if (e.Msg.Contains("QueryGroupSendMsg"))
                {
                    Common.Config.QueryGroupSendMsg();
                }

                if (handlFriendMessageResult.Length <= 0)
                {
                    return;
                }
                int result = Common.CqApi.SendPrivateMessage(e.FromQQ, handlFriendMessageResult.ToString());
            }
            catch (Exception ex)
            {
                Common.CqApi.SendPrivateMessage(e.FromQQ, ex.Message);
            }
        }

        private Thread sendGroupMsg;
        private bool isStop = false;
        private List<long> canSendGroupList = new List<long>();
        private void StartSendGroupMsg(PrivateMessageEventArgs e)
        {
            sendGroupMsg = new Thread(() => SendGroupMsg());
            sendGroupMsg.IsBackground = true;
            sendGroupMsg.Start();
            Thread.Sleep(Common.Config.SendGroupMsgDelay);
            StopSendGroupMsg();
            if (!isStop) { StartSendGroupMsg(e); return; }
            if (isStop)
            {
                Common.CqApi.SendPrivateMessage(e.FromQQ, "计时器已经结束");
            }
        }

        private void SendGroupMsg()
        {
            Dictionary<long, string> groups = Common.Config.SendGroupMsgDic;

            foreach (KeyValuePair<long,string> item in groups)
            {
                int result = Common.CqApi.SendGroupMessage(item.Key, item.Value);
                if(result != 0)
                {
                    if(result == -34)
                    {
                        //发送失败-被禁言或者被踢了
                        Common.Config.RemoveGroupSendMsg(item.Key);
                    }
                }
            }
        }

        private void StopSendGroupMsg()
        {
            sendGroupMsg.Abort();
        }

        private void InsertCanSendGroup(PrivateMessageEventArgs e,long group)
        {
            StringBuilder result = new StringBuilder();
            if (canSendGroupList.Contains(group))
            {
                result.Append("您已添加【");
                result.Append(group);
                result.Append("】请不要重复添加");
                Common.CqApi.SendPrivateMessage(e.FromQQ, result.ToString());
                return;
            }
            if (canSendGroupList == null)
            {
                canSendGroupList = new List<long>();
            }
            canSendGroupList.Add(group);
            if (canSendGroupList.Contains(group))
            {
                result.Append("添加成功");
                PrintCanSendGroup(e);
            }
            else
            {
                result.Append("添加发送群失败，请检查格式【添加发送群:10000】英文分隔符");
            }
            Common.CqApi.SendPrivateMessage(e.FromQQ, result.ToString());
        }
        private void RemoveCanSendGroup(PrivateMessageEventArgs e,long group)
        {
            foreach (var item in canSendGroupList)
            {
                if (item == group)
                {
                    canSendGroupList.Remove(group);
                    break;
                }
            }
            PrintCanSendGroup(e);
            Common.CqApi.SendPrivateMessage(e.FromQQ, "删除发送群成功");
        }
        private void PrintCanSendGroup(PrivateMessageEventArgs e)
        {
            StringBuilder result = new StringBuilder();
            result.Append("当前群列表  ");
            result.Append(canSendGroupList.Count());
            foreach (var item in canSendGroupList)
            {
                result.Append("\r\n【");
                result.Append(item);
                result.Append("】");
            }
            Common.CqApi.SendPrivateMessage(e.FromQQ, result.ToString());
        }

        private void PrintCommond(PrivateMessageEventArgs e)
        {
            StringBuilder command = new StringBuilder();
            command.Append("【指令，打印指令列表】【1】\r\n");
            command.Append("【开始计时器】【2】\r\n");
            command.Append("【结束计时器】【3】\r\n");
            command.Append("【添加发送群:123456（英文分隔符）】【4】\r\n");
            command.Append("【删除发送群:123456（英文分隔符）】【5】\r\n");
            command.Append("【查询发送群】【6】\r\n");
            command.Append("【】【8】\r\n");
            command.Append("【】【9】\r\n");
            command.Append("【】【10】\r\n");
            command.Append("【】【11】\r\n");
            command.Append("【】【12】\r\n");
            command.Append("【】【13】\r\n");
            command.Append("【】【14】\r\n");
            command.Append("【】【15】\r\n");
            command.Append("【】【16】\r\n");
            Common.CqApi.SendPrivateMessage(e.FromQQ, command.ToString());
        }
        #endregion
    }
}
