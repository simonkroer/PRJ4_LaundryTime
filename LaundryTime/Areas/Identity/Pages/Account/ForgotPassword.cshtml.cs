using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Mail;
using System.Text.Encodings.Web;
using System.Text;
using System.Threading.Tasks;
using LaundryTime.Data;
using LaundryTime.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;

namespace LaundryTime.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ForgotPasswordModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private IOptions<EmailAccount> _emailAccount;
        private IOptions<SmsAccount> _smsAccount;

        public ForgotPasswordModel(UserManager<ApplicationUser> userManager, IOptions<EmailAccount> emailAccount, IOptions<SmsAccount> smsAccount)
        {
            _userManager = userManager;
            _emailAccount = emailAccount;
            _smsAccount = smsAccount;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(Input.Email);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return RedirectToPage("./ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please 
                // visit https://go.microsoft.com/fwlink/?LinkID=532713
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = Url.Page(
                    "/Account/ResetPassword",
                    pageHandler: null,
                    values: new { area = "Identity", code },
                    protocol: Request.Scheme);

                MailMessage message = new MailMessage()
                {
                    From = new MailAddress("laundrytimemaster@hotmail.com"), // sender must be a full email address
                    Subject = "Reset Password",
                    IsBodyHtml = true,
                    Body = $"Please reset your password by <a href = '{HtmlEncoder.Default.Encode(callbackUrl)}'> clicking here </a>. <br/> <br/> <img width='100' src='https://t4.ftcdn.net/jpg/03/09/29/23/360_F_309292393_4G7XxgXz5ftKSuSStItdT2ZK1snVEH08.jpg'/> <p>Kind regards</p> <p>Laundry Time</p>",
                    BodyEncoding = System.Text.Encoding.UTF8,
                    SubjectEncoding = System.Text.Encoding.UTF8,
                    To = { user.Email }
                };
                
                SendMail(message);

                return RedirectToPage("./ForgotPasswordConfirmation");
            }

            return Page();
        }

        private void SendMail(MailMessage message)
        {
            using (SmtpClient smtpClient = new SmtpClient()
            {
                Host = "smtp-relay.sendinblue.com",
                Port = 587,
                UseDefaultCredentials = false,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new NetworkCredential(_emailAccount.Value.Login, _emailAccount.Value.Password),
                TargetName = "STARTTLS/smtp-relay.sendinblue.com",
                EnableSsl = false,
            })
                smtpClient.Send(message);
        }
    }
}
