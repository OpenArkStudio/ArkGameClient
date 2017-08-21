using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using ProtoBuf;
using AFTCPClient;
using AFMsg;
using AFCoreEx;

namespace PlayerNetClient
{
   
public class PlayerSender 
{
    PlayerNet mxPlayerNet = null;

    public PlayerSender(PlayerNet clientnet)
    {
        mxPlayerNet = clientnet;
    }

    static public AFMsg.Ident NFToPB(AFCoreEx.AFIDENTID xID)
    {
        AFMsg.Ident xIdent = new AFMsg.Ident();
        xIdent.svrid = xID.nHead64;
        xIdent.index = xID.nData64;

        return xIdent;
    }

    private void SendMsg(AFCoreEx.AFIDENTID xID, AFMsg.EGameMsgID unMsgID, MemoryStream stream)
    {
        AFMsg.MsgBase xData = new AFMsg.MsgBase();
        xData.player_id = NFToPB(xID);
        xData.msg_data = stream.ToArray();

        MemoryStream body = new MemoryStream();
        Serializer.Serialize<AFMsg.MsgBase>(body, xData);

        byte[] bodyByte = body.ToArray();

        mxPlayerNet.mxNet.SendMsg((int)unMsgID, bodyByte);
    }


    public void LoginPB(string strAccount, string strPassword, string strSessionKey)
    {
        if (mxPlayerNet.mbDebugMode)
        {
            mxPlayerNet.mPlayerState = PlayerNet.PLAYER_STATE.E_HAS_PLAYER_ROLELIST;
            //AFCRenderInterface.Instance.LoadScene("SelectScene");
        }
        else
        {
            AFMsg.ReqAccountLogin xData = new AFMsg.ReqAccountLogin();
            xData.account = System.Text.Encoding.Default.GetBytes(strAccount);
            xData.password = System.Text.Encoding.Default.GetBytes(strPassword);
            xData.security_code = System.Text.Encoding.Default.GetBytes(strSessionKey);
            xData.signBuff = System.Text.Encoding.Default.GetBytes("");
            xData.clientVersion = 1;
            xData.loginMode = 0;
            xData.clientIP = 0;
            xData.clientMAC = 0;
            xData.device_info = System.Text.Encoding.Default.GetBytes("");
            xData.extra_info = System.Text.Encoding.Default.GetBytes("");

            MemoryStream stream = new MemoryStream();
            Serializer.Serialize<AFMsg.ReqAccountLogin>(stream, xData);

            SendMsg(new AFCoreEx.AFIDENTID(), AFMsg.EGameMsgID.EGMI_REQ_LOGIN, stream);
        }
    }

    public void RequireWorldList()
    {
        AFMsg.ReqServerList xData = new AFMsg.ReqServerList();
        xData.type = AFMsg.ReqServerListType.RSLT_WORLD_SERVER;

        MemoryStream stream = new MemoryStream();
        Serializer.Serialize<AFMsg.ReqServerList>(stream, xData);

        SendMsg(new AFCoreEx.AFIDENTID(), AFMsg.EGameMsgID.EGMI_REQ_WORLD_LIST, stream);
    }

    public void RequireConnectWorld(int nWorldID)
    {
        AFMsg.ReqConnectWorld xData = new AFMsg.ReqConnectWorld();
        xData.world_id = nWorldID;

        MemoryStream stream = new MemoryStream();
        Serializer.Serialize<AFMsg.ReqConnectWorld>(stream, xData);

        SendMsg(new AFCoreEx.AFIDENTID(), AFMsg.EGameMsgID.EGMI_REQ_CONNECT_WORLD, stream);
    }

    public void RequireVerifyWorldKey(string strAccount, string strKey)
    {
        AFMsg.ReqAccountLogin xData = new AFMsg.ReqAccountLogin();
        xData.account = System.Text.Encoding.Default.GetBytes(strAccount);
        xData.password = System.Text.Encoding.Default.GetBytes("");
        xData.security_code = System.Text.Encoding.Default.GetBytes(strKey);
        xData.signBuff = System.Text.Encoding.Default.GetBytes("");
        xData.clientVersion = 1;
        xData.loginMode = 0;
        xData.clientIP = 0;
        xData.clientMAC = 0;
        xData.device_info = System.Text.Encoding.Default.GetBytes("");
        xData.extra_info = System.Text.Encoding.Default.GetBytes("");

        MemoryStream stream = new MemoryStream();
        Serializer.Serialize<AFMsg.ReqAccountLogin>(stream, xData);

        SendMsg(new AFCoreEx.AFIDENTID(), AFMsg.EGameMsgID.EGMI_REQ_CONNECT_KEY, stream);
    }

    public void RequireServerList()
    {
        AFMsg.ReqServerList xData = new AFMsg.ReqServerList();
        xData.type = AFMsg.ReqServerListType.RSLT_GAMES_ERVER;

        MemoryStream stream = new MemoryStream();
        Serializer.Serialize<AFMsg.ReqServerList>(stream, xData);

        SendMsg(new AFCoreEx.AFIDENTID(), AFMsg.EGameMsgID.EGMI_REQ_WORLD_LIST, stream);
    }

    public void RequireSelectServer(int nServerID)
    {
        AFMsg.ReqSelectServer xData = new AFMsg.ReqSelectServer();
        xData.world_id = nServerID;

        MemoryStream stream = new MemoryStream();
        Serializer.Serialize<AFMsg.ReqSelectServer>(stream, xData);

        SendMsg(new AFCoreEx.AFIDENTID(), AFMsg.EGameMsgID.EGMI_REQ_SELECT_SERVER, stream);
    }

    public void RequireRoleList(string strAccount, int nGameID)
    {
        AFMsg.ReqRoleList xData = new AFMsg.ReqRoleList();
        xData.game_id = nGameID;
        xData.account = UnicodeEncoding.Default.GetBytes(strAccount);

        MemoryStream stream = new MemoryStream();
        Serializer.Serialize<AFMsg.ReqRoleList>(stream, xData);

        SendMsg(new AFCoreEx.AFIDENTID(), AFMsg.EGameMsgID.EGMI_REQ_ROLE_LIST, stream);
    }

    public void RequireCreateRole(string strAccount, string strRoleName, int byCareer, int bySex, int nGameID)
    {
        if (strRoleName.Length >= 20 || strRoleName.Length < 1)
        {
            return;
        }

        AFMsg.ReqCreateRole xData = new AFMsg.ReqCreateRole();
        xData.career = byCareer;
        xData.sex = bySex;
        xData.noob_name = UnicodeEncoding.Default.GetBytes(strRoleName);
        xData.account = UnicodeEncoding.Default.GetBytes(strAccount);
        xData.race = 0;
        xData.game_id = nGameID;

        MemoryStream stream = new MemoryStream();
        Serializer.Serialize<AFMsg.ReqCreateRole>(stream, xData);

        SendMsg(new AFCoreEx.AFIDENTID(), AFMsg.EGameMsgID.EGMI_REQ_CREATE_ROLE, stream);

//         if (mxPlayerNet.mbDebugMode)
//         {
//             AFMsg.AckRoleLiteInfoList xAckBodyData = new AFMsg.AckRoleLiteInfoList();
//             AFMsg.RoleLiteInfo info = new AFMsg.RoleLiteInfo();
// 
//             info.career = byCareer;
//             info.sex = bySex;
//             info.noob_name = xData.noob_name;
//             info.race = xData.race;
//             info.noob_name = xData.noob_name;
//             info.id = new AFMsg.Ident();
//             info.game_id = 1;
//             info.role_level = 1;
//             info.view_record = xData.account = UnicodeEncoding.Default.GetBytes("");
//             info.delete_time = 1;
//             info.reg_time = 1;
//             info.last_offline_time = 1;
//             info.last_offline_ip = 1;
//             xAckBodyData.char_data.Add(info);
// 
//             MemoryStream xAckBodyStream = new MemoryStream();
//             Serializer.Serialize<AFMsg.AckRoleLiteInfoList>(xAckBodyStream, xAckBodyData);
// 
//             AFMsg.MsgBase xAckData = new AFMsg.MsgBase();
//             xAckData.player_id = info.id;
//             xAckData.msg_data = xAckBodyStream.ToArray();
// 
//             MemoryStream xAckAllStream = new MemoryStream();
//             Serializer.Serialize<AFMsg.MsgBase>(xAckAllStream, xAckData);
// 
//             MsgHead head = new MsgHead();
//             head.unMsgID = (UInt16)AFMsg.EGameMsgID.EGMI_ACK_ROLE_LIST;
//             head.unDataLen = (UInt32)xAckAllStream.Length + (UInt32)ConstDefine.NF_PACKET_HEAD_SIZE;
// 
//             mxPlayerNet.mxBinMsgEvent.OnMessageEvent(head, xAckAllStream.ToArray());
//         }
    }

    public void RequireDelRole(AFCoreEx.AFIDENTID objectID, string strAccount, string strRoleName, int nGameID)
    {
        AFMsg.ReqDeleteRole xData = new AFMsg.ReqDeleteRole();
        xData.name = UnicodeEncoding.Default.GetBytes(strRoleName);
        xData.account = UnicodeEncoding.Default.GetBytes(strAccount);
        xData.game_id = nGameID;

        MemoryStream stream = new MemoryStream();
        Serializer.Serialize<AFMsg.ReqDeleteRole>(stream, xData);

        SendMsg(objectID, AFMsg.EGameMsgID.EGMI_REQ_DELETE_ROLE, stream);
    }

    public void RequireEnterGameServer(AFCoreEx.AFIDENTID objectID, string strAccount, string strRoleName, int nServerID)
    {
        AFMsg.ReqEnterGameServer xData = new AFMsg.ReqEnterGameServer();
        xData.name = UnicodeEncoding.Default.GetBytes(strRoleName);
        xData.account = UnicodeEncoding.Default.GetBytes(strAccount);
        xData.game_id = nServerID;
        xData.id = NFToPB(objectID);

        MemoryStream stream = new MemoryStream();
        Serializer.Serialize<AFMsg.ReqEnterGameServer>(stream, xData);

        SendMsg(objectID, AFMsg.EGameMsgID.EGMI_REQ_ENTER_GAME, stream);

//         if (mxPlayerNet.mbDebugMode)
//         {
//             //EGMI_ACK_OBJECT_ENTRY
//             //property
//             //EGMI_ACK_SWAP_SCENE
//             //EGMI_ACK_OBJECT_ENTRY
//             //property
//             float fX = 0.0f;
//             float fY = 0.0f;
//             float fZ = 0.0f;
//             AFIElement xElement = AFCElementManager.Instance.GetElement("1");
//             if (null != xElement)
//             {
//                 string strRelivePos = xElement.QueryString("RelivePos");
//                 string[] sArray = strRelivePos.Split(';');
//                 if (sArray.Length > 0)
//                 {
//                     sArray = sArray[0].Split(',');
//                 }
// 
//                 if (sArray.Length == 3)
//                 {
//                     fX = float.Parse(sArray[0]);
//                     fY = float.Parse(sArray[1]);
//                     fZ = float.Parse(sArray[2]);
//                 }
//             }
//             /////////////////////////////////////////////
//             //mainplayer
//             AFMsg.AckPlayerEntryList xAckMainBodyData = new AFMsg.AckPlayerEntryList();
//             AFMsg.PlayerEntryInfo xInfo = new AFMsg.PlayerEntryInfo();
//             AFMsg.Ident xID = new AFMsg.Ident();
//             xInfo.object_guid = xID;
//             xInfo.x = fX;
//             xInfo.y = fY;
//             xInfo.z = fZ;
//             xInfo.career_type = 1;
//             xInfo.player_state = 1;
//             xInfo.config_id = UnicodeEncoding.Default.GetBytes("");
//             xInfo.scene_id = 1;
//             xInfo.class_id = UnicodeEncoding.Default.GetBytes("Player");
// 
//             xAckMainBodyData.object_list.Add(xInfo);
// 
//             MemoryStream xAckMianPlayerBodyStream = new MemoryStream();
//             Serializer.Serialize<AFMsg.AckPlayerEntryList>(xAckMianPlayerBodyStream, xAckMainBodyData);
// 
//             AFMsg.MsgBase xAckMianPlayerData = new AFMsg.MsgBase();
//             xAckMianPlayerData.player_id = xID;
//             xAckMianPlayerData.msg_data = xAckMianPlayerBodyStream.ToArray();
// 
//             MemoryStream xAckAllStream = new MemoryStream();
//             Serializer.Serialize<AFMsg.MsgBase>(xAckAllStream, xAckMianPlayerData);
// 
//             MsgHead head = new MsgHead();
//             head.unMsgID = (UInt16)AFMsg.EGameMsgID.EGMI_ACK_OBJECT_ENTRY;
//             head.unDataLen = (UInt32)xAckAllStream.Length + (UInt32)ConstDefine.NF_PACKET_HEAD_SIZE;
// 
//             mxPlayerNet.mxBinMsgEvent.OnMessageEvent(head, xAckAllStream.ToArray());
//             /////////////////////////////////////////////
//             //property
// 
// 			AFMsg.ObjectPropertyInt propertyData = new AFMsg.ObjectPropertyInt();
// 
//             PropertyInt xPropertyInt = new PropertyInt();
//             xPropertyInt.property_name = UnicodeEncoding.Default.GetBytes("MOVE_SPEED");
//             xPropertyInt.data = 50000;
//             propertyData.property_list.Add(xPropertyInt);
//             propertyData.player_id = xID;
// 
//             MemoryStream xAckPropertyIntStream = new MemoryStream();
//             Serializer.Serialize<AFMsg.ObjectPropertyInt>(xAckPropertyIntStream, propertyData);
// 
//             AFMsg.MsgBase xPropertyIntMsg = new AFMsg.MsgBase();
//             xPropertyIntMsg.player_id = xID;
//             xPropertyIntMsg.msg_data = xAckPropertyIntStream.ToArray();
// 
//             MemoryStream xAckPropertyIntAllStream = new MemoryStream();
//             Serializer.Serialize<AFMsg.MsgBase>(xAckPropertyIntAllStream, xPropertyIntMsg);
// 
//             MsgHead xAckPropertyhead = new MsgHead();
//             xAckPropertyhead.unMsgID = (UInt16)AFMsg.EGameMsgID.EGMI_ACK_PROPERTY_INT;
//             xAckPropertyhead.unDataLen = (UInt32)xAckPropertyIntAllStream.Length + (UInt32)ConstDefine.NF_PACKET_HEAD_SIZE;
// 
//             mxPlayerNet.mxBinMsgEvent.OnMessageEvent(xAckPropertyhead, xAckPropertyIntAllStream.ToArray());
// 
//             /////////////////////////////////////////////
//             mxPlayerNet.mPlayerState = PlayerNet.PLAYER_STATE.E_PLAYER_GAMEING;
//             //AFCRenderInterface.Instance.LoadScene(1, fX, fY, fZ);
//             /////////////////////////////////////////////
// 
//             //npc
//             AFMsg.AckPlayerEntryList xAckNPCBodyData = new AFMsg.AckPlayerEntryList();
//             for (int i = 0; i < 5; ++i)
//             {
//                 AFMsg.PlayerEntryInfo xNPCInfo = new AFMsg.PlayerEntryInfo();
// 
//                 AFMsg.Ident xNPCID = new AFMsg.Ident();
//                 xNPCID.index = i + 10000;
//                 xNPCInfo.object_guid = xNPCID;
//                 xNPCInfo.x = fX + i;
//                 xNPCInfo.y = fY;
//                 xNPCInfo.z = fZ + i;
//                 xNPCInfo.career_type = 1;
//                 xNPCInfo.player_state = 1;
//                 xNPCInfo.config_id = UnicodeEncoding.Default.GetBytes("");
//                 xNPCInfo.scene_id = 1;
//                 xNPCInfo.class_id = UnicodeEncoding.Default.GetBytes("Player");
// 
//                 xAckNPCBodyData.object_list.Add(xNPCInfo);
//             }
// 
//             MemoryStream xAckNPCBodyStream = new MemoryStream();
//             Serializer.Serialize<AFMsg.AckPlayerEntryList>(xAckNPCBodyStream, xAckNPCBodyData);
// 
//             AFMsg.MsgBase xAckNPCrData = new AFMsg.MsgBase();
//             xAckNPCrData.player_id = xID;
//             xAckNPCrData.msg_data = xAckNPCBodyStream.ToArray();
// 
//             MemoryStream xAckAllNPCStream = new MemoryStream();
//             Serializer.Serialize<AFMsg.MsgBase>(xAckAllNPCStream, xAckNPCrData);
// 
//             MsgHead xNPCHead = new MsgHead();
//             xNPCHead.unMsgID = (UInt16)AFMsg.EGameMsgID.EGMI_ACK_OBJECT_ENTRY;
//             xNPCHead.unDataLen = (UInt32)xAckAllNPCStream.Length + (UInt32)ConstDefine.NF_PACKET_HEAD_SIZE;
// 
//             mxPlayerNet.mxBinMsgEvent.OnMessageEvent(xNPCHead, xAckAllNPCStream.ToArray());
//             //////////////////////////////////////////////
//         }
    }

    public void RequireHeartBeat(AFCoreEx.AFIDENTID objectID)
    {
        AFMsg.ReqHeartBeat xData = new AFMsg.ReqHeartBeat();

        MemoryStream stream = new MemoryStream();
        Serializer.Serialize<AFMsg.ReqHeartBeat>(stream, xData);

        SendMsg(objectID, AFMsg.EGameMsgID.EGMI_STS_HEART_BEAT, stream);
    }

    //有可能是他副本的NPC移动,因此增加64对象ID
    public void RequireMove(AFCoreEx.AFIDENTID objectID, float fX, float fZ)
    {
        AFMsg.ReqAckPlayerMove xData = new AFMsg.ReqAckPlayerMove();
        xData.mover = NFToPB(objectID);
        xData.moveType = 0;

        AFMsg.Position xTargetPos = new AFMsg.Position();
        xTargetPos.x = fX;
        xTargetPos.z = fZ;
        xData.target_pos.Add(xTargetPos);

        MemoryStream stream = new MemoryStream();
        Serializer.Serialize<AFMsg.ReqAckPlayerMove>(stream, xData);

        SendMsg(objectID, AFMsg.EGameMsgID.EGMI_REQ_MOVE, stream);

//         if (mxPlayerNet.mbDebugMode)
//         {
//             AFMsg.MsgBase xAckData = new AFMsg.MsgBase();
//             xAckData.player_id = xData.mover;
//             xAckData.msg_data = stream.ToArray();
// 
//             MemoryStream xAckBody = new MemoryStream();
//             Serializer.Serialize<AFMsg.MsgBase>(xAckBody, xAckData);
// 
//             MsgHead head = new MsgHead();
//             head.unMsgID = (UInt16)AFMsg.EGameMsgID.EGMI_ACK_MOVE;
//             head.unDataLen = (UInt32)xAckBody.Length + (UInt32)ConstDefine.NF_PACKET_HEAD_SIZE;
// 
//             mxPlayerNet.mxBinMsgEvent.OnMessageEvent(head, xAckBody.ToArray());
// 
//         }
    }

    public void RequireMoveImmune(AFCoreEx.AFIDENTID objectID, float fX, float fZ)
    {
        AFMsg.ReqAckPlayerMove xData = new AFMsg.ReqAckPlayerMove();
        xData.mover = NFToPB(objectID);
        xData.moveType = 0;
        AFMsg.Position xTargetPos = new AFMsg.Position();
        xTargetPos.x = fX;
        xTargetPos.z = fZ;
        xData.target_pos.Add(xTargetPos);

        MemoryStream stream = new MemoryStream();
        Serializer.Serialize<AFMsg.ReqAckPlayerMove>(stream, xData);

        SendMsg(objectID, AFMsg.EGameMsgID.EGMI_REQ_MOVE_IMMUNE, stream);

//         if (mxPlayerNet.mbDebugMode)
//         {
//             AFMsg.MsgBase xAckData = new AFMsg.MsgBase();
//             xAckData.player_id = xData.mover;
//             xAckData.msg_data = stream.ToArray();
// 
//             MemoryStream xAckBody = new MemoryStream();
//             Serializer.Serialize<AFMsg.MsgBase>(xAckBody, xAckData);
// 
//             MsgHead head = new MsgHead();
//             head.unMsgID = (UInt16)AFMsg.EGameMsgID.EGMI_ACK_MOVE_IMMUNE;
//             head.unDataLen = (UInt32)xAckBody.Length + (UInt32)ConstDefine.NF_PACKET_HEAD_SIZE;
// 
//             mxPlayerNet.mxBinMsgEvent.OnMessageEvent(head, xAckBody.ToArray());
// 
//         }
    }

    //有可能是他副本的NPC移动,因此增加64对象ID
    public void RequireUseSkill(AFCoreEx.AFIDENTID objectID, string strKillID, AFCoreEx.AFIDENTID nTargetID, float fNowX, float fNowZ, float fTarX, float fTarZ)
    {
        //Debug.Log("RequireUseSkill:" + strKillID);

        AFMsg.Position xNowPos = new AFMsg.Position();
        AFMsg.Position xTarPos = new AFMsg.Position();

        xNowPos.x = fNowX;
        xNowPos.y = 0.0f;
        xNowPos.z = fNowZ;
        xTarPos.x = fTarX;
        xTarPos.y = 0.0f;
        xTarPos.z = fTarZ;

        AFMsg.ReqAckUseSkill xData = new AFMsg.ReqAckUseSkill();
        xData.user = NFToPB(objectID);
        xData.skill_id = UnicodeEncoding.Default.GetBytes(strKillID);
        xData.tar_pos = xTarPos;
        xData.now_pos = xNowPos;

        if (!nTargetID.IsNull())
        {
            AFMsg.EffectData xEffectData = new AFMsg.EffectData();

            xEffectData.effect_ident = NFToPB(nTargetID);
            xEffectData.effect_value = 0;
            xEffectData.effect_rlt = 0;
            xData.effect_data.Add(xEffectData);
        }

        MemoryStream stream = new MemoryStream();
        Serializer.Serialize<AFMsg.ReqAckUseSkill>(stream, xData);

        SendMsg(objectID, AFMsg.EGameMsgID.EGMI_REQ_SKILL_OBJECTX, stream);
    }

    public void RequireUseItem(AFCoreEx.AFIDENTID objectID, AFCoreEx.AFIDENTID nGuid, AFCoreEx.AFIDENTID nTargetID)
    {
        AFMsg.ReqAckUseItem xData = new AFMsg.ReqAckUseItem();
        xData.item_guid = NFToPB(nGuid);

        AFMsg.EffectData xEffectData = new AFMsg.EffectData();

        xEffectData.effect_ident = NFToPB(nTargetID);
        xEffectData.effect_rlt = 0;
        xEffectData.effect_value = 0;

        xData.effect_data.Add(xEffectData);

        MemoryStream stream = new MemoryStream();
        Serializer.Serialize<AFMsg.ReqAckUseItem>(stream, xData);

        SendMsg(objectID, AFMsg.EGameMsgID.EGMI_REQ_ITEM_OBJECT, stream);
    }

    public void RequireChat(AFCoreEx.AFIDENTID objectID, AFCoreEx.AFIDENTID targetID, int nType, string strData)
    {
        AFMsg.ReqAckPlayerChat xData = new AFMsg.ReqAckPlayerChat();
        xData.chat_id = NFToPB(targetID);
        xData.chat_type = (AFMsg.ReqAckPlayerChat.EGameChatType)nType;
        xData.chat_info = UnicodeEncoding.Default.GetBytes(strData);

        MemoryStream stream = new MemoryStream();
        Serializer.Serialize<AFMsg.ReqAckPlayerChat>(stream, xData);

        SendMsg(objectID, AFMsg.EGameMsgID.EGMI_REQ_CHAT, stream);

    }

    public void RequireSwapScene(AFCoreEx.AFIDENTID objectID, int nTransferType, int nSceneID, int nLineIndex)
    {
        AFMsg.ReqAckSwapScene xData = new AFMsg.ReqAckSwapScene();
        xData.transfer_type = (AFMsg.ReqAckSwapScene.EGameSwapType)nTransferType;
        xData.scene_id = nSceneID;
        xData.line_id = nLineIndex;

        MemoryStream stream = new MemoryStream();
        Serializer.Serialize<AFMsg.ReqAckSwapScene>(stream, xData);

        SendMsg(objectID, AFMsg.EGameMsgID.EGMI_REQ_SWAP_SCENE, stream);
    }

    public void RequireProperty(AFCoreEx.AFIDENTID objectID, string strPropertyName, int nValue)
    {
        AFMsg.ReqCommand xData = new AFMsg.ReqCommand();
        xData.control_id = NFToPB(objectID);
        xData.command_id = ReqCommand.EGameCommandType.EGCT_MODIY_PROPERTY;
        xData.command_str_value = UnicodeEncoding.Default.GetBytes(strPropertyName);
        xData.command_value_int = nValue;

        MemoryStream stream = new MemoryStream();
        Serializer.Serialize<AFMsg.ReqCommand>(stream, xData);

        SendMsg(objectID, AFMsg.EGameMsgID.EGMI_REQ_CMD_PROPERTY_INT, stream);
    }

    public void RequireItem(AFCoreEx.AFIDENTID objectID, string strItemName, int nCount)
    {
        AFMsg.ReqCommand xData = new AFMsg.ReqCommand();
        xData.control_id = NFToPB(objectID);
        xData.command_id = ReqCommand.EGameCommandType.EGCT_MODIY_ITEM;
        xData.command_str_value = UnicodeEncoding.Default.GetBytes(strItemName);
        xData.command_value_int = nCount;

        MemoryStream stream = new MemoryStream();
        Serializer.Serialize<AFMsg.ReqCommand>(stream, xData);

        SendMsg(objectID, AFMsg.EGameMsgID.EGMI_REQ_CMD_PROPERTY_INT, stream);
    }

    public void RequireAcceptTask(AFCoreEx.AFIDENTID objectID, string strTaskID)
    {
        AFMsg.ReqAcceptTask xData = new AFMsg.ReqAcceptTask();
        xData.task_id = UnicodeEncoding.Default.GetBytes(strTaskID);

        MemoryStream stream = new MemoryStream();
        Serializer.Serialize<AFMsg.ReqAcceptTask>(stream, xData);

        SendMsg(objectID, AFMsg.EGameMsgID.EGMI_REQ_ACCEPT_TASK, stream);
    }

    public void RequireCompeleteTask(AFCoreEx.AFIDENTID objectID, string strTaskID)
    {
        AFMsg.ReqCompeleteTask xData = new AFMsg.ReqCompeleteTask();
        xData.task_id = UnicodeEncoding.Default.GetBytes(strTaskID);

        MemoryStream stream = new MemoryStream();
        Serializer.Serialize<AFMsg.ReqCompeleteTask>(stream, xData);

        SendMsg(objectID, AFMsg.EGameMsgID.EGMI_REQ_COMPELETE_TASK, stream);
    }

    public void RequirePickUpItem(AFCoreEx.AFIDENTID objectID, AFCoreEx.AFIDENTID nItemID)
    {
        AFMsg.ReqPickDropItem xData = new AFMsg.ReqPickDropItem();
        xData.item_guid = NFToPB(nItemID);

        MemoryStream stream = new MemoryStream();
        Serializer.Serialize<AFMsg.ReqPickDropItem>(stream, xData);

        SendMsg(objectID, AFMsg.EGameMsgID.EGMI_REQ_PICK_ITEM, stream);
    }

    /////////slg////////////////////////////////
    public void RequireBuyBuildingItem(AFCoreEx.AFIDENTID objectID, string strItemID, int nX, int nY, int nZ)
    {
        AFMsg.ReqAckBuyObjectFormShop xData = new AFMsg.ReqAckBuyObjectFormShop();
        xData.config_id = strItemID;
        xData.x = nX;
        xData.y = nY;
        xData.z = nZ;

        MemoryStream stream = new MemoryStream();
        Serializer.Serialize<AFMsg.ReqAckBuyObjectFormShop>(stream, xData);

        SendMsg(objectID, AFMsg.EGameMsgID.EGMI_REQ_BUY_FORM_SHOP, stream);
    }

    public void RequireProductionNPC(AFCoreEx.AFIDENTID objectID, AFCoreEx.AFIDENTID buildingID, string strItemID, int nCount)
    {
        AFMsg.ReqCreateItem xData = new AFMsg.ReqCreateItem();
        xData.object_guid = NFToPB(buildingID);
        xData.config_id = strItemID;
        xData.count = nCount;


        MemoryStream stream = new MemoryStream();
        Serializer.Serialize<AFMsg.ReqCreateItem>(stream, xData);

        SendMsg(objectID, AFMsg.EGameMsgID.EGMI_REQ_CREATE_ITEM, stream);
    }

    public void RequireLvlUpBuilding(AFCoreEx.AFIDENTID objectID, AFCoreEx.AFIDENTID buildingID)
    {
        AFMsg.ReqUpBuildLv xData = new AFMsg.ReqUpBuildLv();
        xData.object_guid = NFToPB(buildingID);



        MemoryStream stream = new MemoryStream();
        Serializer.Serialize<AFMsg.ReqUpBuildLv>(stream, xData);

        SendMsg(objectID, AFMsg.EGameMsgID.EGMI_REQ_UP_BUILD_LVL, stream);
    }

    public void ReqAckMoveBuildObject(AFCoreEx.AFIDENTID objectID, AFCoreEx.AFIDENTID buildingID, float fx, float fy, float fz)
    {
        //申请移动
        AFMsg.ReqAckMoveBuildObject xData = new AFMsg.ReqAckMoveBuildObject();
        xData.object_guid = PlayerSender.NFToPB((AFCoreEx.AFIDENTID)buildingID);
        xData.x = fx;
        xData.y = fy;
        xData.z = fz;

        MemoryStream xStream = new MemoryStream();
        Serializer.Serialize<AFMsg.ReqAckMoveBuildObject>(xStream, xData);

        SendMsg(objectID, AFMsg.EGameMsgID.EGMI_REQ_MOVE_BUILD_OBJECT, xStream);
    }

    public void RequireBuildingOperate(AFCoreEx.AFIDENTID objectID, AFMsg.ESLGFuncType eFunc, AFCoreEx.AFIDENTID buildingID)
    {
        AFMsg.ReqBuildOperate xData = new AFMsg.ReqBuildOperate();
        xData.object_guid = NFToPB(buildingID);
        xData.functype = eFunc;


        MemoryStream stream = new MemoryStream();
        Serializer.Serialize<AFMsg.ReqBuildOperate>(stream, xData);

        SendMsg(objectID, AFMsg.EGameMsgID.EGMI_REQ_BUILD_OPERATE, stream);
    }
}
}