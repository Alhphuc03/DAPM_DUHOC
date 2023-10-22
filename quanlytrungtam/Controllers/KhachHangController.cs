using quanlytrungtam.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace quanlytrungtam.Controllers
{
    public class KhachHangController : Controller
    {
        QUANLYTRUNGTAMDUHOCEntities db = new QUANLYTRUNGTAMDUHOCEntities();
        // GET: KhachHang
        public ActionResult QLKhachHang()
        {
            return View(db.KHACHHANGs.ToList());
        }

        public ActionResult ThemKhachHang()
        {
            return View();
        }

        // POST: KhachHang/ThemKhachHang
        [HttpPost]
        public ActionResult ThemKhachHang(KHACHHANG khachHang)
        {
            if (ModelState.IsValid)
            {

                // Thêm khách hàng vào cơ sở dữ liệu
                db.KHACHHANGs.Add(khachHang);
                db.SaveChanges();

                return RedirectToAction("QLKhachHang");
            }

            // Nếu ModelState không hợp lệ, quay lại trang ThêmKhachHang để hiển thị lỗi
            return View(khachHang);
        }


        public ActionResult SuaKhachHang(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            KHACHHANG khachHang = db.KHACHHANGs.Find(id);

            if (khachHang == null)
            {
                return HttpNotFound();
            }

            return View(khachHang);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SuaKhachHang(KHACHHANG khachHang)
        {
            if (ModelState.IsValid)
            {
                db.Entry(khachHang).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("QLKhachHang");
            }

            return View(khachHang);
        }

        public ActionResult XoaKhachHang(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            KHACHHANG khachHang = db.KHACHHANGs.Find(id);

            if (khachHang == null)
            {
                return HttpNotFound();
            }

            return View(khachHang);
        }

        [HttpPost, ActionName("XoaKhachHang")]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmXoaKhachHang(int id)
        {
            KHACHHANG khachHang = db.KHACHHANGs.Find(id);

            if (khachHang == null)
            {
                return HttpNotFound();
            }

            db.KHACHHANGs.Remove(khachHang);
            db.SaveChanges();

            return RedirectToAction("QLKhachHang");
        }


        public ActionResult CapTaiKhoan(string hoTen, string email)
        {
            // Extract the username from the EMAIL of LICHHENTUVAN
            string username = email.Split('@')[0];
            // Generate a random password with the required format.
            string password = GenerateRandomPassword();

            KHACHHANG newKhachHang = new KHACHHANG
            {
                HOTEN = hoTen,
                Email = email,
                TAIKHOAN = username,
                MATKHAU = password,
                ROLES = 2,
                // Set other properties as needed
            };

            db.KHACHHANGs.Add(newKhachHang);
            db.SaveChanges();

            var lichHen = db.LICHHENTUVANs.FirstOrDefault(lh => lh.HOTEN == hoTen && lh.Email == email);

            if (lichHen != null)
            {
                lichHen.TRANGTHAI = "Đã cấp";
                db.SaveChanges();
            }

            TempData["SuccessMsg"] = "Cấp tài khoản thành công";
            // Gửi email chứa thông tin tài khoản
            SendAccountDetailsEmail(email, username, password);
            return RedirectToAction("Index", "QuanLyLichTuVan");
        }

        // Gửi email với thông tin tài khoản
        private void SendAccountDetailsEmail(string email, string username, string password)
        {
            string emailSubject = "Thông tin tài khoản của bạn";
            string emailBody = @"
            <html>
                <head>
                    <style>
                        /* Định dạng CSS cho email */
                        body {
                            font-family: Arial, sans-serif;
                            background-color: #f4f4f4;
                        }
                        .container {
                            background-color: #ffffff;
                            max-width: 600px;
                            margin: 0 auto;
                            padding: 20px;
                        }
                        h2 {
                            color: #333;
                        }
                        p {
                            color: #666;
                        }
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <h2>Xin chào,</h2>
                        <p>Dưới đây là thông tin tài khoản của bạn :</p>
                        <ul>
                            <li><strong>Tài khoản:</strong> " + username + @"</li>
                            <li><strong>Mật khẩu:</strong> " + password + @"</li>
                        </ul>
                        <p>Bạn có thể dùng tài khoản bên trên để đăng nhập vào website tư vấn du học của chúng tôi và nộp hồ sơ.</p>
                        <p>Cảm ơn bạn đã sử dụng dịch vụ của chúng tôi.</p>
                    </div>
                </body>
            </html>";

            string smtpHostServer = System.Configuration.ConfigurationManager.AppSettings["smtpHostServer"];
            int smtpPort = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["smtpPort"]);
            string senderEmail = System.Configuration.ConfigurationManager.AppSettings["SenderEmail"];
            string senderPassword = System.Configuration.ConfigurationManager.AppSettings["SenderPassword"];

            try
            {
                var client = new SmtpClient(smtpHostServer, smtpPort)
                {
                    EnableSsl = true,
                    Timeout = 100000,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(senderEmail, senderPassword)
                };

                var mailMessage = new MailMessage
                {
                    IsBodyHtml = true,
                    BodyEncoding = Encoding.UTF8,
                    From = new MailAddress(senderEmail, "Trung Tâm Tư Vấn Du Học VIETDINH")
                };
                mailMessage.To.Add(email);
                mailMessage.Subject = emailSubject;
                mailMessage.Body = emailBody;

                client.Send(mailMessage);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi gửi email: " + ex.Message);
            }
        }



        private string GenerateRandomPassword()
        {
            // Define the character sets for lowercase, uppercase, digits, and special characters.
            string lowercaseChars = "abcdefghijklmnopqrstuvwxyz";
            string uppercaseChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string digitChars = "0123456789";
            string specialChars = "!@#$%&*";

            // Create a random object for generating random numbers.
            Random random = new Random();

            // Create a StringBuilder to build the password.
            StringBuilder passwordBuilder = new StringBuilder();

            // Add one lowercase character.
            passwordBuilder.Append(lowercaseChars[random.Next(lowercaseChars.Length)]);

            // Add one uppercase character.
            passwordBuilder.Append(uppercaseChars[random.Next(uppercaseChars.Length)]);

            // Add one digit.
            passwordBuilder.Append(digitChars[random.Next(digitChars.Length)]);

            // Add five random characters (either lowercase, uppercase, digit, or special character).
            for (int i = 0; i < 5; i++)
            {
                string randomSet = lowercaseChars + uppercaseChars + digitChars + specialChars;
                passwordBuilder.Append(randomSet[random.Next(randomSet.Length)]);
            }

            // Shuffle the characters to make the password more random.
            string password = new string(passwordBuilder.ToString().ToCharArray().OrderBy(s => (random.Next() % 2) == 0).ToArray());

            return password;
        }

    }
}