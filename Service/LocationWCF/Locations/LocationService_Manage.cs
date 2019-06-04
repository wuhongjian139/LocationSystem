﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using BLL;
using DbModel.Location.AreaAndDev;
using DbModel.Tools;
using Location.BLL.ServiceHelpers;
using Location.Model.DataObjects.ObjectAddList;
using Location.TModel.FuncArgs;
using Location.TModel.Location;
using Location.TModel.Location.AreaAndDev;
using Location.TModel.Location.Data;
using Location.TModel.Location.Obsolete;
using Location.TModel.Location.Alarm;
using Location.TModel.Location.Person;
using Location.TModel.LocationHistory.Data;
using LocationServices.Converters;
using LocationServices.Tools;
using LocationWCFService;
using LocationWCFService.ServiceHelper;
using ConfigArg = Location.TModel.Location.AreaAndDev.ConfigArg;
using DevInfo = Location.TModel.Location.AreaAndDev.DevInfo;
using KKSCode = Location.TModel.Location.AreaAndDev.KKSCode;
using Post = Location.TModel.Location.AreaAndDev.Post;
using Dev_DoorAccess = Location.TModel.Location.AreaAndDev.Dev_DoorAccess;
using TModel.BaseData;
using TModel.Location.Manage;
using System.Configuration;
using LocationServer.Tools;
using System.Security.Cryptography;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace LocationServices.Locations
{
    //人员相关的接口
    public partial class LocationService : ILocationService, IDisposable
    {
        public static List<LoginInfo> loginInfos = new List<LoginInfo>();

        public static RemoteEndpointMessageProperty GetClientEndPoint()
        {
            //提供方法执行的上下文环境
            OperationContext context = OperationContext.Current;
            //获取传进的消息属性
            MessageProperties properties = context.IncomingMessageProperties;
            //获取消息发送的远程终结点IP和端口
            RemoteEndpointMessageProperty endpoint = properties[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;
            return endpoint;
        }

        public LoginInfo Login(LoginInfo info)
        {

            RemoteEndpointMessageProperty endpoint = GetClientEndPoint();
            if (endpoint != null)
            {
                info.ClientIp = endpoint.Address;
                info.ClientPort = endpoint.Port;
            }
            db.Users.Login(info);
            ShowLogEx(">>>>> Login !!!!!!!!!!!!!!!!!!!! :" + info.Session);
            return info;
        }


        public LoginInfo Logout(LoginInfo info)
        {
            ShowLogEx(">>>>> Logout !!!!!!!!!!!!!!!!!!!! :" + info.Session);
            var login = loginInfos.Find(i => i.Session == info.Session);
            if (login == null)
            {
                info.Result = false;
            }
            else
            {
                info.Result = true;
                loginInfos.Remove(login);
            }

            info.Session = "";
            return info;
        }

        public LoginInfo KeepLive(LoginInfo info)
        {
            var login = loginInfos.Find(i => i.Session == info.Session);
            if (login == null)
            {
                info.Result = false;
            }
            else
            {
                info.Result = true;
                login.LiveTime = DateTime.Now;
            }
            return info;
        }
        

        public VersionInfo GetVersionInfo()
        {
            VersionInfo version = new VersionInfo();
            version.Version = ConfigurationManager.AppSettings["VersionCode"];
            version.LocationURL = ConfigurationManager.AppSettings["LocationPackageURL"];
            return version;
        }

        public void DebugMessage(string msg)
        {
            ShowLogEx(msg);
        }
    }
}
