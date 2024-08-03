using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;
using Chola_Model;
using System.Data;
using Google.Protobuf.WellKnownTypes;

namespace Chola_DataModel
{
    public class ScanDataModel
    {
        private MySqlConnection con;
        MySqlTransaction mysqltrans;
        public DataConnection MysqlCon;

        public ScanResponse_Model DMLScanDetails(Scan_Model ObjScandetails, string ConString)
        {
           
            int retdtls = 0;
            ScanResponse_Model ObjResponse = new ScanResponse_Model();
            try
            {
                MysqlCon = new DataConnection(ConString);

                if (MysqlCon.con != null && MysqlCon.con.State == ConnectionState.Closed)
                {
                    MysqlCon.con.Open();
                    mysqltrans = MysqlCon.con.BeginTransaction();
                }

                MySqlCommand cmd = new MySqlCommand("sp_arms_ins_scan", MysqlCon.con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("in_scan_gid", MySqlDbType.Int32).Value = ObjScandetails.scan_gid;
                cmd.Parameters.Add("in_scan_packet_gid", MySqlDbType.Int32).Value = ObjScandetails.scan_packet_gid;
                cmd.Parameters.Add("in_scan_reader_txt", MySqlDbType.VarChar).Value = ObjScandetails.scan_reader_text;
                cmd.Parameters.Add("in_scan_chq_no", MySqlDbType.VarChar).Value = ObjScandetails.scan_chq_no;
                cmd.Parameters.Add("in_scan_micr_code", MySqlDbType.VarChar).Value = ObjScandetails.scan_micr_code;
                cmd.Parameters.Add("in_scan_tran_code", MySqlDbType.VarChar).Value = ObjScandetails.scan_tran_code;
                cmd.Parameters.Add("in_scan_base_code", MySqlDbType.VarChar).Value = ObjScandetails.scan_base_code;
                cmd.Parameters.Add("in_scan_chq_type", MySqlDbType.VarChar).Value = ObjScandetails.scan_chq_type;
                cmd.Parameters.Add("in_scan_chq_gid", MySqlDbType.Int32).Value = ObjScandetails.scan_chq_gid;
                cmd.Parameters.Add("in_scan_remark", MySqlDbType.VarChar).Value = ObjScandetails.scan_remarks;
                cmd.Parameters.Add(new MySqlParameter("out_scan_gid", MySqlDbType.Int32)).Direction = ParameterDirection.Output;
                cmd.Parameters.Add(new MySqlParameter("out_msg", MySqlDbType.VarChar)).Direction = ParameterDirection.Output;
                cmd.Parameters.Add(new MySqlParameter("out_result", MySqlDbType.Int32)).Direction = ParameterDirection.Output;
                retdtls = cmd.ExecuteNonQuery();

                ObjResponse.Out_scan_gid = (Int32)cmd.Parameters["out_scan_gid"].Value;
                ObjResponse.Out_msg = (string)cmd.Parameters["out_msg"].Value;
                ObjResponse.Out_result = (Int32)cmd.Parameters["out_result"].Value;

                if (MysqlCon.con.State == ConnectionState.Open)
                {
                    mysqltrans.Commit();
                    MysqlCon.con.Close();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ObjResponse;
        }
        public ScanResponse_Model SaveScanImage(ScanImage_Model ObjDetails,string ConString)
        {
           
            int retdtls = 0;
            ScanResponse_Model ObjResponse = new ScanResponse_Model();
            try
            {
                MysqlCon = new DataConnection(ConString);

                if (MysqlCon.con != null && MysqlCon.con.State == ConnectionState.Closed)
                {
                    MysqlCon.con.Open();
                    mysqltrans = MysqlCon.con.BeginTransaction();
                }

                MySqlCommand cmd = new MySqlCommand("sp_arms_ins_scanimage", MysqlCon.con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("in_scanimage_gid", MySqlDbType.Int32).Value = ObjDetails.scanimage_gid;
                cmd.Parameters.Add("in_scanimage_scan_gid", MySqlDbType.Int32).Value = ObjDetails.scamimage_scan_gid;
                cmd.Parameters.Add("in_scanimage_type", MySqlDbType.VarChar).Value = ObjDetails.scanimage_type;
                cmd.Parameters.Add("in_scanimage_side", MySqlDbType.VarChar).Value = ObjDetails.scanimage_side;                
                cmd.Parameters.Add(new MySqlParameter("out_scanimage_gid", MySqlDbType.Int32)).Direction = ParameterDirection.Output;
                cmd.Parameters.Add(new MySqlParameter("out_scan_gid", MySqlDbType.Int32)).Direction = ParameterDirection.Output;
                cmd.Parameters.Add(new MySqlParameter("out_msg", MySqlDbType.VarChar)).Direction = ParameterDirection.Output;
                cmd.Parameters.Add(new MySqlParameter("out_result", MySqlDbType.Int32)).Direction = ParameterDirection.Output;
                retdtls = cmd.ExecuteNonQuery();

                ObjResponse.Out_scanimage_gid = (Int32)cmd.Parameters["out_scanimage_gid"].Value;
                ObjResponse.Out_scan_gid = (Int32)cmd.Parameters["out_scan_gid"].Value;
                ObjResponse.Out_msg = (string)cmd.Parameters["out_msg"].Value;
                ObjResponse.Out_result = (Int32)cmd.Parameters["out_result"].Value;

                if (MysqlCon.con.State == ConnectionState.Open)
                {
                    mysqltrans.Commit();
                    MysqlCon.con.Close();
                }

            }
            catch(Exception ex)
            {
                throw ex;
            }
            return ObjResponse;
        }
    }
}
