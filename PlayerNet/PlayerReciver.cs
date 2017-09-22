using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.IO;
using AFTCPClient;
using ProtoBuf;
using AFMsg;
using AFCoreEx;

namespace PlayerNetClient
{
	public class PlayerReciver
	{
        public PlayerNet mxPlayerNet ;
        public PlayerReciver(PlayerNet xPlayerNet)
        {
            mxPlayerNet = xPlayerNet;
        }

        ~PlayerReciver()
        {
        }

        static public AFCoreEx.AFIDENTID PBToAF(AFMsg.Ident xID)
        {
            AFCoreEx.AFIDENTID xIdent = new AFCoreEx.AFIDENTID();
            xIdent.nHead32 = xID.svrid;
            xIdent.nData32 = xID.index;

            return xIdent;
        }

        static public AFCoreEx.AFIDataList.Var_Data PBPropertyToData(AFMsg.PropertyPBData xProperty)
        {
            AFCoreEx.AFIDataList.Var_Data xData = new AFCoreEx.AFIDataList.Var_Data();
            xData.nType  = (AFIDataList.VARIANT_TYPE)xProperty.ndataType;
            switch((AFIDataList.VARIANT_TYPE)xProperty.ndataType)
            {
                case AFIDataList.VARIANT_TYPE.VTYPE_BOOLEAN:
                    {
                        xData.mData = xProperty.mbValue;
                    }
                    break;
                case AFIDataList.VARIANT_TYPE.VTYPE_INT:
                    {
                        xData.mData = xProperty.mnValue;
                    }
                    break;

                case AFIDataList.VARIANT_TYPE.VTYPE_INT64:
                    {
                        xData.mData = xProperty.mn64Value;
                    }
                    break;

                case AFIDataList.VARIANT_TYPE.VTYPE_FLOAT:
                    {
                        xData.mData = xProperty.mfValue;
                    }
                    break;

                case AFIDataList.VARIANT_TYPE.VTYPE_DOUBLE:
                    {
                        xData.mData = xProperty.mdValue;
                    }
                    break;

                case AFIDataList.VARIANT_TYPE.VTYPE_STRING:
                    {
                        xData.mData = xProperty.mstrValue;
                    }
                    break;

                case AFIDataList.VARIANT_TYPE.VTYPE_OBJECT:
                    {
                        
                        xData.mData = PBToAF(xProperty.mGuid);
                    }
                    break;

                case AFIDataList.VARIANT_TYPE.VTYPE_POINTER:
                    {
                        
                    }
                    break;

                case AFIDataList.VARIANT_TYPE.VTYPE_USERDATA:
                    { }
                    break;
                case AFIDataList.VARIANT_TYPE.VTYPE_TABLE:
                    { }
                    break;
            }
            return xData;
        }

        static public AFCoreEx.AFIDataList.Var_Data PBRecordToData(AFMsg.RecordPBData xPbrecord, ref int nRow, ref int col)
        {
            AFCoreEx.AFIDataList.Var_Data xData = new AFCoreEx.AFIDataList.Var_Data();
            nRow = xPbrecord.row;
            col = xPbrecord.col;
            switch ((AFIDataList.VARIANT_TYPE)xPbrecord.ndataType)
            {
                case AFIDataList.VARIANT_TYPE.VTYPE_BOOLEAN:
                    {
                        xData.mData = xPbrecord.mbValue;
                    }
                    break;
                case AFIDataList.VARIANT_TYPE.VTYPE_INT:
                    {
                        xData.mData = xPbrecord.mnValue;
                    }
                    break;
                case AFIDataList.VARIANT_TYPE.VTYPE_INT64:
                    {
                        xData.mData = xPbrecord.mn64Value;
                    }
                    break;
                case AFIDataList.VARIANT_TYPE.VTYPE_FLOAT:
                    {
                        xData.mData = xPbrecord.mfValue;
                    }
                    break;

                case AFIDataList.VARIANT_TYPE.VTYPE_DOUBLE:
                    {
                        xData.mData = xPbrecord.mdValue;
                    }
                    break;
                case AFIDataList.VARIANT_TYPE.VTYPE_STRING:
                    {
                        xData.mData = xPbrecord.mstrValue;
                    }
                    break;
                case AFIDataList.VARIANT_TYPE.VTYPE_OBJECT:
                    {
                        xData.mData = PBToAF(xPbrecord.mGuid);
                    }
                    break;
                case AFIDataList.VARIANT_TYPE.VTYPE_POINTER:
                    {

                    }
                    break;
                case AFIDataList.VARIANT_TYPE.VTYPE_USERDATA:
                    { }
                    break;
                case AFIDataList.VARIANT_TYPE.VTYPE_TABLE:
                    { }
                    break;
            }
            return xData;
        }

        public  void OnDisConnect()
        {
            
        }

        public  void OnConnect()
        {

        }
		public void Init() 
		{

            mxPlayerNet.mxNet.RegisteredConnectDelegation(OnConnect);
            mxPlayerNet.mxNet.RegisteredDisConnectDelegation(OnDisConnect);

            mxPlayerNet.mxNet.RegisteredDelegation((int)AFMsg.EGameMsgID.EGMI_ACK_LOGIN, EGMI_ACK_LOGIN);
            mxPlayerNet.mxNet.RegisteredDelegation((int)AFMsg.EGameMsgID.EGMI_ACK_WORLD_LIST, EGMI_ACK_WORLD_LIST);
            mxPlayerNet.mxNet.RegisteredDelegation((int)AFMsg.EGameMsgID.EGMI_EVENT_RESULT, EGMI_EVENT_RESULT);
            mxPlayerNet.mxNet.RegisteredDelegation((int)AFMsg.EGameMsgID.EGMI_ACK_ROLE_LIST, EGMI_ACK_ROLE_LIST);
            mxPlayerNet.mxNet.RegisteredDelegation((int)AFMsg.EGameMsgID.EGMI_ACK_CONNECT_WORLD, EGMI_ACK_CONNECT_WORLD);
            mxPlayerNet.mxNet.RegisteredDelegation((int)AFMsg.EGameMsgID.EGMI_ACK_CONNECT_KEY, EGMI_ACK_CONNECT_KEY);
            mxPlayerNet.mxNet.RegisteredDelegation((int)AFMsg.EGameMsgID.EGMI_ACK_SELECT_SERVER, EGMI_ACK_SELECT_SERVER);
            mxPlayerNet.mxNet.RegisteredDelegation((int)AFMsg.EGameMsgID.EGMI_ACK_SWAP_SCENE, EGMI_ACK_SWAP_SCENE);

            mxPlayerNet.mxNet.RegisteredDelegation((int)AFMsg.EGameMsgID.EGMI_ACK_OBJECT_ENTRY, EGMI_ACK_OBJECT_ENTRY);
            mxPlayerNet.mxNet.RegisteredDelegation((int)AFMsg.EGameMsgID.EGMI_ACK_OBJECT_LEAVE, EGMI_ACK_OBJECT_LEAVE);
            mxPlayerNet.mxNet.RegisteredDelegation((int)AFMsg.EGameMsgID.EGMI_ACK_MOVE, EGMI_ACK_MOVE);
            mxPlayerNet.mxNet.RegisteredDelegation((int)AFMsg.EGameMsgID.EGMI_ACK_MOVE_IMMUNE, EGMI_ACK_MOVE_IMMUNE);

            mxPlayerNet.mxNet.RegisteredDelegation((int)AFMsg.EGameMsgID.EGMI_ACK_PROPERTY_DATA, EGMI_ACK_PROPERTY_DATA);

            mxPlayerNet.mxNet.RegisteredDelegation((int)AFMsg.EGameMsgID.EGMI_ACK_RECORD_DATA, EGMI_ACK_RECORD_DATA);
            mxPlayerNet.mxNet.RegisteredDelegation((int)AFMsg.EGameMsgID.EGMI_ACK_SWAP_ROW, EGMI_ACK_SWAP_ROW);
            mxPlayerNet.mxNet.RegisteredDelegation((int)AFMsg.EGameMsgID.EGMI_ACK_ADD_ROW, EGMI_ACK_ADD_ROW);
            mxPlayerNet.mxNet.RegisteredDelegation((int)AFMsg.EGameMsgID.EGMI_ACK_REMOVE_ROW, EGMI_ACK_REMOVE_ROW);

            mxPlayerNet.mxNet.RegisteredDelegation((int)AFMsg.EGameMsgID.EGMI_ACK_OBJECT_RECORD_ENTRY, EGMI_ACK_OBJECT_RECORD_ENTRY);
            mxPlayerNet.mxNet.RegisteredDelegation((int)AFMsg.EGameMsgID.EGMI_ACK_OBJECT_PROPERTY_ENTRY, EGMI_ACK_OBJECT_PROPERTY_ENTRY);


            mxPlayerNet.mxNet.RegisteredDelegation((int)AFMsg.EGameMsgID.EGMI_ACK_SKILL_OBJECTX, EGMI_ACK_SKILL_OBJECTX);
            mxPlayerNet.mxNet.RegisteredDelegation((int)AFMsg.EGameMsgID.EGMI_ACK_CHAT, EGMI_ACK_CHAT);
		}

        private void ReceiveMsg<T>(MemoryStream stream, ref T xData)
        {
            xData = Serializer.Deserialize<T>(stream);
            if (null == xData)
            {
                return ;
            }
        }

        private void EGMI_EVENT_RESULT(MsgHead head, MemoryStream stream)
        {
            //OnResultMsg
            AFMsg.AckEventResult xResultCode = new AFMsg.AckEventResult();
            ReceiveMsg(stream, ref xResultCode);
            AFMsg.EGameEventCode eEvent = xResultCode.event_code;

            mxPlayerNet.mxNet.DoResultCodeDelegation((int)eEvent);
        }

        private void EGMI_ACK_LOGIN(MsgHead head, MemoryStream stream)
        {
            AFMsg.AckEventResult xData = new AFMsg.AckEventResult();
            ReceiveMsg(stream, ref xData);

            if (EGameEventCode.EGEC_ACCOUNT_SUCCESS == xData.event_code)
            {
                mxPlayerNet.ChangePlayerState(PlayerNet.PLAYER_STATE.E_PLAYER_LOGIN_SUCCESSFUL);    
            }
        }

        private void EGMI_ACK_WORLD_LIST(MsgHead head, MemoryStream stream)
        {
            AFMsg.AckServerList xData = new AFMsg.AckServerList();
            ReceiveMsg(stream, ref xData);

            if (ReqServerListType.RSLT_WORLD_SERVER == xData.type)
            {
                for(int i = 0; i < xData.info.Count; ++i)
                {
                    ServerInfo info = xData.info[i];
                    mxPlayerNet.aWorldList.Add(info);
                }
                mxPlayerNet.ChangePlayerState(PlayerNet.PLAYER_STATE.E_PLAYER_WORLD_LIST_SUCCESSFUL_WAITING_SELECT_WORLD);
            }
            else if (ReqServerListType.RSLT_GAMES_ERVER == xData.type)
            {
                for (int i = 0; i < xData.info.Count; ++i)
                {
                    ServerInfo info = xData.info[i];
                    mxPlayerNet.aServerList.Add(info);
                }
            }
        }

        private void EGMI_ACK_CONNECT_WORLD(MsgHead head, MemoryStream stream)
        {
            mxPlayerNet.mxNet.Disconnect();

            AFMsg.AckConnectWorldResult xData = new AFMsg.AckConnectWorldResult();
            ReceiveMsg(stream, ref xData);

            mxPlayerNet.strKey = System.Text.Encoding.Default.GetString(xData.world_key);
            mxPlayerNet.strWorldIP = System.Text.Encoding.Default.GetString(xData.world_ip);
            mxPlayerNet.nWorldPort = xData.world_port;
            mxPlayerNet.ChangePlayerState(PlayerNet.PLAYER_STATE.E_PLAYER_GET_WORLD_KEY_SUCCESSFUL);
        }

        private void EGMI_ACK_CONNECT_KEY(MsgHead head, MemoryStream stream)
        {
            AFMsg.AckEventResult xData = new AFMsg.AckEventResult();
            ReceiveMsg(stream, ref xData);

            if (xData.event_code == EGameEventCode.EGEC_VERIFY_KEY_SUCCESS)
            {
                //验证成功
                mxPlayerNet.ChangePlayerState(PlayerNet.PLAYER_STATE.E_VERIFY_KEY_SUCCESS_FULL);
                mxPlayerNet.nMainRoleID = PBToAF(xData.event_object);

                //申请世界内的服务器列表
                PlayerSender sender = mxPlayerNet.mxSender;
                if (null != sender)
                {
                    sender.RequireServerList();
                }
            }
        }

        private void EGMI_ACK_SELECT_SERVER(MsgHead head, MemoryStream stream)
        {
            AFMsg.AckEventResult xData = new AFMsg.AckEventResult();
            ReceiveMsg(stream, ref xData);

            if (xData.event_code == EGameEventCode.EGEC_SELECTSERVER_SUCCESS)
            {
                PlayerSender sender = mxPlayerNet.mxSender;
                if (null != sender)
                {
                    sender.RequireRoleList(mxPlayerNet.strAccount, mxPlayerNet.nServerID);
                }
            }
        }
        
        private void EGMI_ACK_ROLE_LIST(MsgHead head, MemoryStream stream)
        {
            AFMsg.AckRoleLiteInfoList xData = new AFMsg.AckRoleLiteInfoList();
            ReceiveMsg(stream, ref xData);

            mxPlayerNet.aCharList.Clear();
            for (int i = 0; i < xData.char_data.Count; ++i)
            {
                AFMsg.RoleLiteInfo info = xData.char_data[i];
                mxPlayerNet.aCharList.Add(info);
            }

            if (PlayerNet.PLAYER_STATE.E_WAIT_SELECT_ROLE != mxPlayerNet.GetPlayerState())
            {

                AFCDataList varList = new AFCDataList();
                varList.AddString("SelectScene");
                AFCLogicEvent.Instance.DoEvent((int)ClientEventDefine.EventDefine_LoadSelectRole, varList);
            }

            mxPlayerNet.ChangePlayerState(PlayerNet.PLAYER_STATE.E_GETROLELIST_SUCCESSFUL);
        }

        private void EGMI_ACK_SWAP_SCENE(MsgHead head, MemoryStream stream)
        {
            mxPlayerNet.ChangePlayerState(PlayerNet.PLAYER_STATE.E_PLAYER_GAMEING);

            AFMsg.ReqAckSwapScene xData = new AFMsg.ReqAckSwapScene();
            ReceiveMsg(stream, ref xData);

            //AFCRenderInterface.Instance.LoadScene(xData.scene_id, xData.x, xData.y, xData.z);

            AFCDataList varList = new AFCDataList();
            varList.AddInt64(xData.scene_id);
            varList.AddFloat(xData.x);
            varList.AddFloat(xData.y);
            varList.AddFloat(xData.z);

            AFCLogicEvent.Instance.DoEvent( (int)ClientEventDefine.EventDefine_Swap_Scene, varList);
        }

        private void EGMI_ACK_OBJECT_ENTRY(MsgHead head, MemoryStream stream)
        {
            AFMsg.AckPlayerEntryList xData = new AFMsg.AckPlayerEntryList();
            ReceiveMsg(stream, ref xData);

            for (int i = 0; i < xData.object_list.Count; ++i)
            {
                AFMsg.PlayerEntryInfo xInfo = xData.object_list[i];

                AFIDataList var = new AFCDataList();
                var.AddString("X");
                var.AddFloat(xInfo.pos.x);
                var.AddString("Y");
                var.AddFloat(xInfo.pos.y);
                var.AddString("Z");
                var.AddFloat(xInfo.pos.z);
                AFIObject xGO = AFCKernel.Instance.CreateObject(PBToAF(xInfo.object_guid), xInfo.scene_id, 0, System.Text.Encoding.Default.GetString(xInfo.class_id), System.Text.Encoding.Default.GetString(xInfo.config_id), var);
                if (null == xGO)
                {
                    continue;
                }
            }
        }

        private void EGMI_ACK_OBJECT_LEAVE(MsgHead head, MemoryStream stream)
		{
            AFMsg.AckPlayerLeaveList xData = new AFMsg.AckPlayerLeaveList();
            ReceiveMsg(stream, ref xData);

            for (int i = 0; i < xData.object_list.Count; ++i)
            {
                AFCKernel.Instance.DestroyObject(PBToAF(xData.object_list[i]));
            }
		}

        private void EGMI_ACK_MOVE(MsgHead head, MemoryStream stream)
        {
            AFMsg.ReqAckPlayerMove xData = new AFMsg.ReqAckPlayerMove();
            ReceiveMsg(stream, ref xData);
            if (xData.target_pos.Count <= 0)
            {
                return;
            }
            float fSpeed = AFCKernel.Instance.QueryPropertyInt(PBToAF(xData.mover), "MOVE_SPEED") / 10000.0f;

            AFCDataList varList = new AFCDataList();
            varList.AddObject(PBToAF(xData.mover));
            varList.AddFloat(xData.target_pos[0].x);
            varList.AddFloat(xData.target_pos[0].y);
            varList.AddFloat(xData.target_pos[0].z);
            varList.AddFloat(fSpeed);

            AFCLogicEvent.Instance.DoEvent( (int)ClientEventDefine.EventDefine_MoveTo, varList);
            
            //AFCRenderInterface.Instance.MoveTo(PBToAF(xData.mover), new Vector3(xData.target_pos[0].x, xData.target_pos[0].y, xData.target_pos[0].z), fSpeed, true);
        }

        private void EGMI_ACK_MOVE_IMMUNE(MsgHead head, MemoryStream stream)
        {
            AFMsg.ReqAckPlayerMove xData = new AFMsg.ReqAckPlayerMove();
            ReceiveMsg(stream, ref xData);
            if (xData.target_pos.Count <= 0)
            {
                return;
            }

            //其实就是jump
            float fSpeed = AFCKernel.Instance.QueryPropertyInt(PBToAF(xData.mover), "MOVE_SPEED") / 10000.0f;
            fSpeed *= 1.5f;

            AFCDataList varList = new AFCDataList();
            varList.AddObject(PBToAF(xData.mover));
            varList.AddFloat(xData.target_pos[0].x);
            varList.AddFloat(xData.target_pos[0].y);
            varList.AddFloat(xData.target_pos[0].z);
            varList.AddFloat(fSpeed);

            AFCLogicEvent.Instance.DoEvent( (int)ClientEventDefine.EVENTDEFINE_MOVE_IMMUNE, varList);

            //AFCRenderInterface.Instance.MoveImmuneBySpeed(PBToAF(xData.mover), new Vector3(xData.target_pos[0].x, xData.target_pos[0].y, xData.target_pos[0].z), fSpeed, true);

        }
        /////////////////////////////////////////////////////////////////////
        private void EGMI_ACK_PROPERTY_DATA(MsgHead head, MemoryStream stream)
		{
			AFMsg.ObjectPropertyPBData propertyData = new AFMsg.ObjectPropertyPBData();
            ReceiveMsg(stream, ref propertyData);

            AFIObject go = AFCKernel.Instance.GetObject(PBToAF(propertyData.player_id));
            AFIPropertyManager propertyManager = go.GetPropertyManager();
			
			for(int i = 0; i < propertyData.property_list.Count; i++)
			{
                AFCoreEx.AFIDataList.Var_Data xData = PBPropertyToData(propertyData.property_list[i]);
                AFIProperty property = propertyManager.GetProperty(System.Text.Encoding.Default.GetString(propertyData.property_list[i].property_name));
                if(null == property)
                {

                    AFIDataList varList = new AFCDataList();
                    varList.AddDataObject(ref xData);

                    property = propertyManager.AddProperty(System.Text.Encoding.Default.GetString(propertyData.property_list[i].property_name), varList);
                }

                property.SetDataObject(ref xData);
			}
		}
		
		private void EGMI_ACK_RECORD_DATA(MsgHead head, MemoryStream stream)
		{
			AFMsg.ObjectRecordPBData recordData = new AFMsg.ObjectRecordPBData();
            ReceiveMsg(stream, ref recordData);

            AFIObject go = AFCKernel.Instance.GetObject(PBToAF(recordData.player_id));
            AFIRecordManager recordManager = go.GetRecordManager();
            AFIRecord record = recordManager.GetRecord(System.Text.Encoding.Default.GetString(recordData.record_name));

            for (int i = 0; i < recordData.record_list.Count; i++)
            {
                int nRow = -1;
                int nCol = -1;
                AFCoreEx.AFIDataList.Var_Data xRowData = PBRecordToData(recordData.record_list[i], ref nRow, ref nCol);
                record.SetDataObject(nRow, nCol, xRowData);
            }
		}
		
		private void EGMI_ACK_SWAP_ROW(MsgHead head, MemoryStream stream)
		{
			AFMsg.ObjectRecordSwap recordData = new AFMsg.ObjectRecordSwap();
            ReceiveMsg(stream, ref recordData);

            AFIObject go = AFCKernel.Instance.GetObject(PBToAF(recordData.player_id));
            AFIRecordManager recordManager = go.GetRecordManager();
            AFIRecord record = recordManager.GetRecord(System.Text.Encoding.Default.GetString(recordData.origin_record_name));


            //目前认为在同一张表中交换吧
            record.SwapRow(recordData.row_origin, recordData.row_target);
        
        }

        private void ADD_ROW(AFCoreEx.AFIDENTID self, string strRecordName, AFMsg.RecordAddRowStruct xAddStruct)
        {
            AFIObject go = AFCKernel.Instance.GetObject(self);
            AFIRecordManager xRecordManager = go.GetRecordManager();


            Hashtable recordVecDesc = new Hashtable();
            Hashtable recordVecData = new Hashtable();

            AFCoreEx.AFCDataList RowList = new AFCDataList();
            AFCoreEx.AFIDataList varListDesc = new AFCDataList();
            for (int k = 0; k < xAddStruct.record_data_list.Count; ++k)
            {
                AFMsg.RecordPBData addIntStruct = (AFMsg.RecordPBData)xAddStruct.record_data_list[k];
                if (addIntStruct.col >= 0)
                {
                    int nRow = -1;
                    int nCol = -1;
                    AFCoreEx.AFIDataList.Var_Data xRowData = PBRecordToData(xAddStruct.record_data_list[k], ref nRow, ref nCol);
                    RowList.AddDataObject(ref xRowData);
                    varListDesc.AddDataObject(ref xRowData);
                }
            }

            AFIRecord xRecord = xRecordManager.GetRecord(strRecordName);
            if (null == xRecord)
            {
                xRecord = xRecordManager.AddRecord(strRecordName, 512, varListDesc);
            }

            xRecord.AddRow(xAddStruct.row, RowList);
        }

        private void EGMI_ACK_ADD_ROW(MsgHead head, MemoryStream stream)
		{
			AFMsg.ObjectRecordAddRow recordData = new AFMsg.ObjectRecordAddRow();
            ReceiveMsg(stream, ref recordData);

            AFIObject go = AFCKernel.Instance.GetObject(PBToAF(recordData.player_id));
            AFIRecordManager recordManager = go.GetRecordManager();

            for (int i = 0; i < recordData.row_data.Count; i++)
            {
                ADD_ROW(PBToAF(recordData.player_id), System.Text.Encoding.Default.GetString(recordData.record_name), recordData.row_data[i]);
            }
		}

        private void EGMI_ACK_REMOVE_ROW(MsgHead head, MemoryStream stream)
		{
			AFMsg.ObjectRecordRemove recordData = new AFMsg.ObjectRecordRemove();
            ReceiveMsg(stream, ref recordData);

            AFIObject go = AFCKernel.Instance.GetObject(PBToAF(recordData.player_id));
            AFIRecordManager recordManager = go.GetRecordManager();
            AFIRecord record = recordManager.GetRecord(System.Text.Encoding.Default.GetString(recordData.record_name));

            for (int i = 0; i < recordData.remove_row.Count; i++)
            {
                record.Remove(recordData.remove_row[i]);
            }
		}

        private void EGMI_ACK_OBJECT_RECORD_ENTRY(MsgHead head, MemoryStream stream)
        {
            AFMsg.MultiObjectRecordList xMultiObjectRecordData = new AFMsg.MultiObjectRecordList();
            ReceiveMsg(stream, ref xMultiObjectRecordData);

            for (int i = 0; i < xMultiObjectRecordData.multi_player_record.Count; i++)
            {
                AFMsg.ObjectRecordList xObjectRecordList = xMultiObjectRecordData.multi_player_record[i];
                for (int j = 0; j < xObjectRecordList.record_list.Count; j++)
                {
                    AFMsg.ObjectRecordBase xObjectRecordBase = xObjectRecordList.record_list[j];
                    for (int k = 0; k < xObjectRecordBase.row_struct.Count; ++k )
                    {
                        AFMsg.RecordAddRowStruct xAddRowStruct = xObjectRecordBase.row_struct[i];

                        ADD_ROW(PBToAF(xObjectRecordList.player_id), System.Text.Encoding.Default.GetString(xObjectRecordBase.record_name), xAddRowStruct);
                    }
                }
            }
        }

        private void EGMI_ACK_OBJECT_PROPERTY_ENTRY(MsgHead head, MemoryStream stream)
        {
            AFMsg.MultiObjectPropertyList xMultiObjectPropertyList = new AFMsg.MultiObjectPropertyList();
            ReceiveMsg(stream, ref xMultiObjectPropertyList);

            for (int i = 0; i < xMultiObjectPropertyList.multi_player_property.Count; i++)
            {
                AFMsg.ObjectPropertyList xPropertyData = xMultiObjectPropertyList.multi_player_property[i];
                AFIObject go = AFCKernel.Instance.GetObject(PBToAF(xPropertyData.player_id));
                AFIPropertyManager xPropertyManager = go.GetPropertyManager();

                for (int j = 0; j < xPropertyData.property_data_list.Count; j++)
                {
                    string strPropertyName = System.Text.Encoding.Default.GetString(xPropertyData.property_data_list[j].property_name);

                    AFCoreEx.AFIDataList.Var_Data  xPropertyValue  = PBPropertyToData(xPropertyData.property_data_list[j]);
                    AFIProperty xProperty = xPropertyManager.GetProperty(strPropertyName);
                    if (null == xProperty)
                    {
                        AFIDataList varList = new AFCDataList();
                        varList.AddDataObject( ref xPropertyValue);

                        xProperty = xPropertyManager.AddProperty(strPropertyName, varList);
                    }

                    xProperty.SetDataObject(ref xPropertyValue);
                }
            }
        }

        //////////////////////////////////
        private void EGMI_ACK_SKILL_OBJECTX(MsgHead head, MemoryStream stream)
        {
            AFMsg.ReqAckUseSkill xReqAckUseSkill = new AFMsg.ReqAckUseSkill();
            ReceiveMsg(stream, ref xReqAckUseSkill);
            AFMsg.Position xNowPos = xReqAckUseSkill.now_pos;
            AFMsg.Position xTarPos = xReqAckUseSkill.tar_pos;

            AFIDataList xObjectList = new AFCDataList();
            AFIDataList xRtlList = new AFCDataList();
            AFIDataList xValueList = new AFCDataList();

            if (xReqAckUseSkill.effect_data.Count <= 0)
            {
                return;
            }

            for (int i = 0; i < xReqAckUseSkill.effect_data.Count; ++i )
            {
                xObjectList.AddObject(PBToAF(xReqAckUseSkill.effect_data[i].effect_ident));
                xRtlList.AddInt64((int)xReqAckUseSkill.effect_data[i].effect_rlt);
                xValueList.AddInt64((int)xReqAckUseSkill.effect_data[i].effect_value);
            }
            

            string strSkillName = System.Text.Encoding.Default.GetString(xReqAckUseSkill.skill_id);
            //Debug.Log("AckUseSkill:" + strSkillName);

            AFCDataList varList = new AFCDataList();
            varList.AddObject(PBToAF(xReqAckUseSkill.user));
            varList.AddFloat(xNowPos.x);
            varList.AddFloat(xNowPos.z);
            varList.AddFloat(xTarPos.x);
            varList.AddFloat(xTarPos.z);

            if (xObjectList.Count() != xRtlList.Count() || xObjectList.Count() != xValueList.Count())
            {
                return;
            }

            varList.AddInt64(xObjectList.Count());
            for (int i = 0; i < xObjectList.Count(); ++i)
            {
                varList.AddObject(xObjectList.ObjectVal(i));
            }

            for (int i = 0; i < xRtlList.Count(); ++i)
            {
                varList.AddInt64(xRtlList.Int64Val(i));
            }

            for (int i = 0; i < xValueList.Count(); ++i)
            {
                varList.AddInt64(xValueList.Int64Val(i));
            }


            AFCLogicEvent.Instance.DoEvent( (int)ClientEventDefine.EVENTDEFINE_USESKILL, varList);

            //AFCRenderInterface.Instance.UseSkill(, strSkillName, xNowPos.x, xNowPos.z, xTarPos.x, xTarPos.z, xObjectList, xRtlList, xValueList);
        }

        private void EGMI_ACK_CHAT(MsgHead head, MemoryStream stream)
        {
            AFMsg.ReqAckPlayerChat xReqAckChat = new AFMsg.ReqAckPlayerChat();
            ReceiveMsg(stream, ref xReqAckChat);

            mxPlayerNet.aChatMsgList.Add(PBToAF(xReqAckChat.chat_id).ToString() + ":" + System.Text.Encoding.Default.GetString(xReqAckChat.chat_info));
        }
    }

}