using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Web.Mvc;
using quanlytrungtam.Models;
using System.Net;
using System.Text;
using System.IO;

namespace quanlytrungtam.Controllers
{

    public class SendMailController : Controller
    {
        QUANLYTRUNGTAMDUHOCEntities db = new QUANLYTRUNGTAMDUHOCEntities();
        // GET: SendMail
        [HttpGet]
        public ActionResult Index(int? id)
        {

            return View(db.KHACHHANGs.SingleOrDefault(n=>n.MAKH==id));
        }
        [HttpPost]
        public ActionResult Index(EmailViewModel emailViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var isSent = SendEmail(emailViewModel);
            if (!isSent)
            {
                ViewBag.Message = "false";
                return View();
            }

            ViewBag.Message = "true";

            return RedirectToAction("Index","Home");
        }

        private static bool SendEmail(EmailViewModel emailVm)
        {
            emailVm.EmailBody = emailVm.EmailBody;
            emailVm.SenderEmailAddress = System.Configuration.ConfigurationManager.AppSettings["SenderEmail"];
            emailVm.SenderPassword = System.Configuration.ConfigurationManager.AppSettings["SenderPassword"];
            emailVm.SmtpHostServer = System.Configuration.ConfigurationManager.AppSettings["smtpHostServer"];
            emailVm.SmtpPort = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["smtpPort"]);

            try
            {
                var client = new SmtpClient(emailVm.SmtpHostServer, emailVm.SmtpPort)
                {
                    EnableSsl = true,
                    Timeout = 100000,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(emailVm.SenderEmailAddress, emailVm.SenderPassword)
                };
                var mailMessage = new MailMessage
                {
                    IsBodyHtml = true,
                    BodyEncoding = Encoding.UTF8,
                    
                    From = new MailAddress(emailVm.SenderEmailAddress, "Trung Tâm Tư Vấn Du Học Connect")
                };
                mailMessage.To.Add(emailVm.ReceiverEmailAddress);
                mailMessage.Subject = emailVm.EmailSubject;
                mailMessage.Body = emailVm.EmailBody;
                client.Send(mailMessage);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public ActionResult AutoMail(String email, String hoten, String tenlt, string truong, string diachi, string hocphi)
        {

            var EmailBody = @"<a href='https://localhost:44313/HomeClien/Index+'>Học bổng du học Mỹ</a>";
            var SenderEmailAddress = System.Configuration.ConfigurationManager.AppSettings["SenderEmail"];
            var SenderPassword = System.Configuration.ConfigurationManager.AppSettings["SenderPassword"];
            var SmtpHostServer = System.Configuration.ConfigurationManager.AppSettings["smtpHostServer"];
            var SmtpPort = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["smtpPort"]);

            try
            {
                var client = new SmtpClient(SmtpHostServer, SmtpPort)
                {
                    EnableSsl = true,
                    Timeout = 100000,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(SenderEmailAddress, SenderPassword)
                };
                var mailMessage = new MailMessage
                {
                    IsBodyHtml = true,
                    BodyEncoding = Encoding.UTF8,

                    From = new MailAddress(SenderEmailAddress, "Trung Tâm Tư Vấn Du Học Connect")
                };
                using (StreamReader reader = new StreamReader(Server.MapPath("~/Content/Template/sendmail.html")))
                {
                    EmailBody = reader.ReadToEnd();
                }

                EmailBody = EmailBody.Replace("{hoten}", hoten);
                EmailBody = EmailBody.Replace("{tenlt}", tenlt);
                EmailBody = EmailBody.Replace("{truong}", truong);
                EmailBody = EmailBody.Replace("{diachi}", diachi);
                EmailBody = EmailBody.Replace("{hocphi}", hocphi.ToString());
                MailAddress ReceiverEmailAddress = new MailAddress(email, "Trung Tâm Tư Vấn Du Học Connect");
                mailMessage.To.Add(ReceiverEmailAddress);

                mailMessage.Subject = "Đơn xác nhận hồ sơ";
                mailMessage.Body = EmailBody;
                client.Send(mailMessage);


            }
            catch (Exception)
            {
            }
            TempData["result"] = "Duyệt Hồ sơ Thành công !";
            return RedirectToAction("Index", "QuanLyHoSoKH");
        }
    }
}