using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chola_Model;
using Chola_Service;
using Microsoft.Extensions.Options;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Chola_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScanController : ControllerBase
    {
        private IConfiguration _configuration;
        private IHostingEnvironment _hostingEnvironment;
        private readonly IOptions<MySettingsModel> appSettings;
        string ConString;

        public ScanController(IConfiguration configuration, IHostingEnvironment hostingEnvironment, IOptions<MySettingsModel> app)
        {
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
            appSettings = app;
            ConString = appSettings.Value.mysqlcon;
        }
       
        [HttpPost("SaveScanDetails")]
        public ScanResponse_Model SaveScanDtl(Scan_Model  ObjDtls)
        {
            ScanResponse_Model ObjRes = new ScanResponse_Model();
            try
            {
                ObjRes = Scan_Service.SaveScanDetails(ObjDtls, ConString);
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return ObjRes;
        }
        [HttpPost ("SaveScanImage")]
        public ScanResponse_Model SaveScanImageDtls(ScanImage_Model ObjScanImage)
        {
            ScanResponse_Model ObjRes = new ScanResponse_Model();
            try
            {
                ObjRes = Scan_Service.SaveScanImageDetails(ObjScanImage, ConString);
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return ObjRes;
        }
        [HttpPost("SaveImageFolder")]
        public ScanSaveImageFld_Model SaveScanImageFld(ScanSaveImageFld_Model ObjImageFld)
        {
            ScanSaveImageFld_Model ObjRes = new ScanSaveImageFld_Model();
            try
            {
                ObjRes.scanimage_gid = ObjImageFld.scanimage_gid;
                var filename = Path.Combine(Convert.ToString(ObjImageFld.scanimage_gid) + ".txt");
                string webRootPath = _hostingEnvironment.ContentRootPath;
                string folderName = "ScanImages";
                // Set a variable to the Documents path.
                webRootPath = Path.Combine(webRootPath, folderName);
                //byte[] imageBytes = Convert.FromBase64String(objimage.imagefile);
                // Write the string array to a new file named "WriteLines.txt".
                //System.IO.File.WriteAllBytes(webRootPath, imageBytes.ToArray());
                using (StreamWriter outputFile = new StreamWriter(Path.Combine(webRootPath, filename)))
                {
                    outputFile.WriteLine(ObjImageFld.scanImage_dtls);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ObjRes;
        }
        [HttpPost("viewimage")]
        public string viewimage(ScanSaveImageFld_Model objimage)
        {           
            ScanSaveImageFld_Model view = new ScanSaveImageFld_Model();         

            var filename = Path.Combine(Convert.ToString(objimage.scanimage_gid) + ".txt");
            string webRootPath = _hostingEnvironment.ContentRootPath;
            string folderName = "ScanImages";
            // Set a variable to the Documents path.
            webRootPath = Path.Combine(webRootPath, folderName, filename);
            if (System.IO.File.Exists(webRootPath))
            {
                FileStream fileStream = new FileStream(webRootPath, FileMode.Open, FileAccess.ReadWrite);
                using (StreamReader reader = new StreamReader(fileStream))
                {
                    view.scanImage_dtls = reader.ReadLine();
                }
            }

             return JsonConvert.SerializeObject(view, Formatting.Indented);
            //return view;
        }
    }
}
