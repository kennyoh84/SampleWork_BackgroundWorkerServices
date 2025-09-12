﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VCheckListenerWorker.Lib.Object;

namespace VCheckListenerWorker.Lib.Logic.HL7.V26
{
    public class AckRepository
    {
        /// <summary>
        ///  Generate Ack Message
        /// </summary>
        /// <param name="sIMessage"></param>
        /// <returns></returns>
        public static String GenerateAcknowlegeMessage(NHapi.Base.Model.IMessage sIMessage)
        {
            try
            {
                NHapi.Model.V26.Message.ORU_R01 sRU_R01 = (NHapi.Model.V26.Message.ORU_R01)sIMessage;

                Message response = new Message();

                // ------------- Message Header ------------//
                Segment msh = new Segment("MSH");
                msh.Field(1, sRU_R01.MSH.FieldSeparator.Value);
                msh.Field(2, sRU_R01.MSH.EncodingCharacters.Value);
                msh.Field(3, sRU_R01.MSH.SendingApplication.NamespaceID.Value + "^" +
                             sRU_R01.MSH.SendingApplication.UniversalID.Value + "^" +
                             sRU_R01.MSH.SendingApplication.UniversalIDType.Value);
                msh.Field(4, sRU_R01.MSH.SendingFacility.NamespaceID.Value);
                msh.Field(5, sRU_R01.MSH.ReceivingApplication.NamespaceID.Value);
                msh.Field(6, sRU_R01.MSH.ReceivingFacility.NamespaceID.Value);
                msh.Field(7, DateTime.Now.ToString("yyyyMMddhhmmsszzz"));
                msh.Field(9, "ACK^R01^ACK");
                msh.Field(10, Guid.NewGuid().ToString());
                msh.Field(11, sRU_R01.MSH.ProcessingID.ProcessingID.Value);
                msh.Field(12, sRU_R01.MSH.VersionID.VersionID.Value);
                response.Add(msh);

                // ------------- Message Acknowledgement ---------------------//
                Segment msa = new Segment("MSA");
                msa.Field(1, NHapi.Base.AcknowledgmentCode.CA.ToString());
                msa.Field(2, sRU_R01.MSH.MessageControlID.Value.ToString());
                response.Add(msa);

                StringBuilder frame = new StringBuilder();
                frame.Append((char)0x0b);
                frame.Append(response.SerializeMessage());
                frame.Append((char)0x1c);
                frame.Append((char)0x0d);

                return frame.ToString();
            }
            catch (Exception ex)
            {
                return String.Empty;
            }
        }
    }
}
