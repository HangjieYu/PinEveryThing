﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Newtonsoft.Json;
using PinEverything.Entites;
using PinEverything.Services;

namespace PinEverything.Web.ajaxpage
{
    public partial class Index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request["op"] != null)
            {
                string op = Request["op"].ToString();
                switch (op)
                {
                    case "LoadUserDetail":
                        LoadUserDetail();
                        break;
                    case "AddPublic":
                        AddPublic();
                        break;
                    case "LoadPublicList":
                        LoadPublicList();
                        break;
                    case "JoinPublic":
                        JoinPublic();
                        break;
                    case "LoadDialogMsg":
                        LoadDialogMsg();
                        break;
                }
            }
        }

        /// <summary>
        /// 获取用户基本信息
        /// </summary>
        private void LoadUserDetail()
        {
            JavaScriptObject resultObj = new JavaScriptObject();
            if (Session["user"] != null)
            {
                UserInfo userInfo = new UserInfo();
                userInfo = Session["user"] as UserInfo;
                JavaScriptObject userObj = new JavaScriptObject();
                userObj.Add("UserId", userInfo.UserId);
                userObj.Add("Avatar", userInfo.Avatar);
                userObj.Add("UserName", userInfo.UserName);

                resultObj.Add("MSG", "Y");
                resultObj.Add("Users", userObj);
            }
            else
                resultObj.Add("MSG", "N");

            PageResponse(resultObj);
        }

        /// <summary>
        /// 发布信息
        /// </summary>
        private void AddPublic()
        {
            JavaScriptObject resultObj = new JavaScriptObject();
            if (Session["user"] != null)
            {
                string startPlace = Request["startPlace"];
                string destination = Request["destination"];
                string carType = Request["carType"];
                string carColor = Request["carColor"];
                string ownerPhone = Request["ownerPhone"];
                string note = Request["note"];
                int pubType = int.Parse(Request["pubType"].ToString());

                UserInfo userInfo = new UserInfo();
                userInfo = Session["user"] as UserInfo;

                PYTService PYTService = new PYTService();

                bool flag = PYTService.AddPublishInfo(Guid.NewGuid(), userInfo.ProjectId, userInfo.UserId, pubType, 0, "", note, carType, carColor, 4);

                if (flag)
                {
                    resultObj.Add("MSG", "Y");
                    //发布动态
                    APIService APIService = new Services.APIService();
                    string access_token = userInfo.MDToken;
                    string pMsg = startPlace;
                    string title = startPlace;
                    bool postFlag = APIService.postUpdate(access_token, pMsg, title);
                }
                else
                    resultObj.Add("MSG", "N");
            }
            else
                resultObj.Add("MSG", "N");

            PageResponse(resultObj);
        }

        /// <summary>
        /// 获取发布列表
        /// </summary>
        private void LoadPublicList()
        {
            JavaScriptObject resultObj = new JavaScriptObject();
            if (Session["user"] != null)
            {
                int pageIndex = int.Parse(Request["pageIndex"].ToString());
                int pageSize = int.Parse(Request["pageSize"].ToString());

                PYTService PYTService = new PYTService();

                Entites.EntityList<PublishInfo> publishInfoList = new EntityList<PublishInfo>();
                publishInfoList = PYTService.QueryPublishInfo(pageIndex, pageSize);
                List<PublishInfo> publicInfo = new List<PublishInfo>();
                publicInfo = publishInfoList.Table;

                JavaScriptArray publicArr = new JavaScriptArray();
                foreach (PublishInfo publicItem in publicInfo)
                {
                    JavaScriptObject publicObj = new JavaScriptObject();
                    UserInfo userInfo = PYTService.GetUser(Guid.Parse(publicItem.UserId.ToString()));
                    if (userInfo != null)
                    {
                        publicObj.Add("PublishId", publicItem.PublishId);
                        publicObj.Add("PubTitle", publicItem.PubTitle);
                        publicObj.Add("UserId", publicItem.UserId);
                        publicObj.Add("Avatar", userInfo.Avatar);
                        publicObj.Add("UserName", userInfo.UserName);
                        publicObj.Add("Lat", publicItem.Lat);
                        publicObj.Add("Lng", publicItem.Lng);
                        publicArr.Add(publicObj);
                    }
                }
                resultObj.Add("pubList", publicArr);
                resultObj.Add("MSG", "Y");
            }
            else
                resultObj.Add("MSG", "N");

            PageResponse(resultObj);
        }

        /// <summary>
        /// 加入
        /// </summary>
        private void JoinPublic()
        {
            JavaScriptObject resultObj = new JavaScriptObject();
            if (Session["user"] != null)
            {
                string publishId = Request["publishId"];
                if (!string.IsNullOrEmpty(publishId))
                {
                    int joinRole = 1;
                    string Lat = string.Empty;
                    string Lng = string.Empty;

                    UserInfo userInfo = new UserInfo();
                    userInfo = Session["user"] as UserInfo;

                    PYTService PYTService = new PYTService();
                    bool flag = PYTService.JoinPublishInfo(Guid.Parse(publishId), userInfo.UserId, joinRole, Lat, Lng);

                    if (flag)
                    {
                        APIService APIService = new Services.APIService();
                        PublishInfo publicInfo = PYTService.GetPublicInfo(Guid.Parse(publishId));
                        string toUser = publicInfo.UserId.ToString();
                        string msg = "测试忽略";
                        int dialogType = 1;
                        //插入对话表
                        bool dialogFlag = PYTService.AddDialogMsg(Guid.NewGuid(), Guid.Parse(publishId), Guid.NewGuid(), userInfo.UserId, Guid.Parse(toUser), msg, dialogType, Lat, Lng);
                        //发送私信
                        if (dialogFlag)
                            APIService.sendMsg(userInfo.MDToken, toUser, msg, "1");
                        resultObj.Add("MSG", "Y");
                    }
                    else
                        resultObj.Add("MSG", "N");
                }
                else
                    resultObj.Add("MSG", "N");
            }
            else
                resultObj.Add("MSG", "N");

            PageResponse(resultObj);
        }

        /// <summary>
        /// 加载对话信息
        /// </summary>
        private void LoadDialogMsg()
        {
            JavaScriptObject resultObj = new JavaScriptObject();
            if (Session["user"] != null)
            {
                string publishId = Request["publishId"];
                int pageIndex = int.Parse(Request["pageIndex"].ToString());
                int pageSize = int.Parse(Request["pageSize"].ToString());
                if (!string.IsNullOrEmpty(publishId))
                {
                    PYTService PYTService = new PYTService();
                    Entites.EntityList<DialogueInfo> dialogList = new EntityList<DialogueInfo>();
                    dialogList = PYTService.QueryDialogInfo( Guid.Parse(publishId),pageIndex, pageSize);

                    List<DialogueInfo> dialogInfo = new List<DialogueInfo>();
                    dialogInfo = dialogList.Table;

                    JavaScriptArray dialogArr = new JavaScriptArray();
                    foreach (DialogueInfo dialogItem in dialogInfo)
                    {
                        JavaScriptObject publicObj = new JavaScriptObject();
                        UserInfo fromuserInfo = PYTService.GetUser(Guid.Parse(dialogItem.FromUserId.ToString()));
                        UserInfo touserInfo = PYTService.GetUser(Guid.Parse(dialogItem.ToUserId.ToString()));
                        publicObj.Add("PublishId", dialogItem.PublishId);
                        publicObj.Add("Msg", dialogItem.Msg);
                        publicObj.Add("FromUserId", dialogItem.FromUserId);
                        publicObj.Add("ToUserId", dialogItem.ToUserId);
                        if (fromuserInfo != null)
                        {
                            publicObj.Add("FromUserName", fromuserInfo.UserName);
                            publicObj.Add("FromUserAvatar", fromuserInfo.Avatar);
                        }
                        if (touserInfo != null)
                        {
                            publicObj.Add("ToUserName", touserInfo.UserName);
                            publicObj.Add("toUserAvatar", touserInfo.Avatar);
                        }
                        dialogArr.Add(publicObj);
                    }
                    resultObj.Add("dialogList", dialogArr);
                    resultObj.Add("MSG", "Y");
                }
                else
                    resultObj.Add("MSG", "N");
            }
            else
                resultObj.Add("MSG", "N");

            PageResponse(resultObj);
        }

        private void PageResponse(JavaScriptObject resultObj)
        {
            Response.Clear();
            Response.AddHeader("Content-Type", "Application/json");
            string jsonStr = JavaScriptConvert.SerializeObject(resultObj);
            Response.Write(jsonStr);
            Response.End();
        }
    }
}