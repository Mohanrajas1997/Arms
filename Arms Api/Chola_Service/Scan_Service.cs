using System;
using System.Collections.Generic;
using System.Text;
using Chola_Model;
using Chola_DataModel;


namespace Chola_Service
{
    public class Scan_Service
    {
        public static ScanResponse_Model SaveScanDetails(Scan_Model ObjScandetails, string ConString)
        {

            ScanResponse_Model ObjResponse = new ScanResponse_Model();
            try
            {
                ScanDataModel objDataModel = new ScanDataModel();
                ObjResponse = objDataModel.DMLScanDetails(ObjScandetails, ConString);

                return ObjResponse;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static ScanResponse_Model SaveScanImageDetails(ScanImage_Model ObjScanImage,string Constring)
        {
            ScanResponse_Model ObjResponse = new ScanResponse_Model();
            try
            {
                ScanDataModel ObjDataModel = new ScanDataModel();
                ObjResponse = ObjDataModel.SaveScanImage(ObjScanImage, Constring);
                return ObjResponse;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
