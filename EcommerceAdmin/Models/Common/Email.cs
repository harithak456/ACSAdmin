using EcommerceAdmin.Models.Bal;
using EcommerceAdmin.Models.Entity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace EcommerceAdmin.Models.Common
{
    public class Email
    {
        Bal_Order balOrder = new Bal_Order();
        public int SendConfirmationMail(int Order_ID, string body,string subject)
        {
            Ent_Order ent = new Ent_Order();
            ent = balOrder.SelectOrder(Order_ID);
         
            body = body.Replace("{{ReceivedDate}}", ent.Received_Date);
            body = body.Replace("{{ShippedDate}}", ent.Shipped_Date);
            body = body.Replace("{{DeliveredDate}}", ent.Delivered_Date);
            body = body.Replace("{OrderID}", Order_ID.ToString());
            body = body.Replace("{Total}", ent.Order_Total.ToString());
            body = body.Replace("{Billing_FirstName}", ent.Billing_FirstName);
            body = body.Replace("{Billing_LastName}", ent.Billing_LastName);
            body = body.Replace("{Billing_Address1}", ent.Billing_Address1);
            body = body.Replace("{Billing_Address2}", ent.Billing_Address2);
            body = body.Replace("{Billing_Country}", ent.Billing_Country);
            body = body.Replace("{Billing_Phone}", ent.Billing_Phone);
            body = body.Replace("{Shipping_FirstName}", ent.Shipping_FirstName);
            body = body.Replace("{Shipping_LastName}", ent.Shipping_LastName);
            body = body.Replace("{Shipping_Address1}", ent.Shipping_Address1);
            body = body.Replace("{Shipping_Address2}", ent.Shipping_Address2);
            body = body.Replace("{Shipping_Country}", ent.Shipping_Country);
            body = body.Replace("{Shipping_Phone}", ent.Shipping_Phone);

             int i=SendMail(body, ent.Billing_Email, subject);
            return i;
        }

        public int SendMail(string Mailbody, string MailTo, string subject)
        {
            try
            {
                using (MailMessage mail = new MailMessage())
                {
                    int port = 587;
                    string host = "smtp.yandex.com.tr";
                    string sendmail = "mailsupport@intellilabs.co.in";
                    string password = "admin@123";

                    mail.From = new MailAddress(sendmail, "ACSpareparts.com");
                    mail.To.Add(MailTo);
                    mail.Subject = subject;
                    mail.IsBodyHtml = true;
                    AlternateView htmlView = AlternateView.CreateAlternateViewFromString(Mailbody, null, "text/html");
                    mail.AlternateViews.Add(htmlView);
                    using (SmtpClient emailClient = new SmtpClient(host, port))
                    {
                        System.Net.NetworkCredential userInfo = new System.Net.NetworkCredential(sendmail, password);
                        emailClient.UseDefaultCredentials = false;
                        emailClient.EnableSsl = true;
                        emailClient.DeliveryFormat = SmtpDeliveryFormat.International;
                        emailClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                        if (!string.IsNullOrEmpty(userInfo.UserName.Trim()) && !string.IsNullOrEmpty(userInfo.Password.Trim()))
                        {
                            emailClient.Credentials = userInfo;
                        }
                        emailClient.Send(mail);
                    }

                }
                return 1;

            }
            catch (Exception e)
            {
                return 0;
            }
        }
    }
}