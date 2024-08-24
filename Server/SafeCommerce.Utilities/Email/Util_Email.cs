/* 
 * Provides utilities for sending email notifications. This class includes methods for sending 
 * different types of emails, such as OTP and password reset emails, using the SendGrid service.
 */

using SendGrid;
using SendGrid.Helpers.Mail;

namespace SafeCommerce.Utilities.Email;

/// <summary>
/// Provides utilities for email operations, particularly for sending emails via SendGrid.
/// </summary>
public static class Util_Email
{
    /// <summary>
    /// Sends a One Time Password (OTP) email to a specified recipient.
    /// </summary>
    /// <param name="toEmail">The recipient's email address.</param>
    /// <param name="userFullName">The full name of the recipient.</param>
    /// <param name="OTP">The One Time Password to be sent.</param>
    /// <returns>A task representing the asynchronous operation of sending an email.</returns>
    public static async Task<Response>
    SendOTP_Email
    (
        string toEmail,
        string userFullName,
        string OTP
    )
    {
        var apiKey = Environment.GetEnvironmentVariable("SendGridKey_SafeCommerce");
        var client = new SendGridClient(apiKey);
        var from = new EmailAddress("klevis.mema@ulb.be", "Klevis Mema"); // add it in the app settings . json
        var subject = "Your one time password";
        var to = new EmailAddress(toEmail, userFullName);
        var plainTextContent = OTP;
        var htmlContent =
        $$"""
         <!DOCTYPE html>
         <html>

         <head>
           <title></title>
           <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
           <meta name="viewport" content="width=device-width, initial-scale=1">
           <meta http-equiv="X-UA-Compatible" content="IE=edge" />
           <style type="text/css">
               @media screen {
                   @font-face {
                       font-family: 'Lato';
                       font-style: normal;
                       font-weight: 400;
                       src: local('Lato Regular'), local('Lato-Regular'), url(https://fonts.gstatic.com/s/lato/v11/qIIYRU-oROkIk8vfvxw6QvesZW2xOQ-xsNqO47m55DA.woff) format('woff');
                   }

                   @font-face {
                       font-family: 'Lato';
                       font-style: normal;
                       font-weight: 700;
                       src: local('Lato Bold'), local('Lato-Bold'), url(https://fonts.gstatic.com/s/lato/v11/qdgUG4U09HnJwhYI-uK18wLUuEpTyoUstqEm5AMlJo4.woff) format('woff');
                   }

                   @font-face {
                       font-family: 'Lato';
                       font-style: italic;
                       font-weight: 400;
                       src: local('Lato Italic'), local('Lato-Italic'), url(https://fonts.gstatic.com/s/lato/v11/RYyZNoeFgb0l7W3Vu1aSWOvvDin1pK8aKteLpeZ5c0A.woff) format('woff');
                   }

                   @font-face {
                       font-family: 'Lato';
                       font-style: italic;
                       font-weight: 700;
                       src: local('Lato Bold Italic'), local('Lato-BoldItalic'), url(https://fonts.gstatic.com/s/lato/v11/HkF_qI1x_noxlxhrhMQYELO3LdcAZYWl9Si6vvxL-qU.woff) format('woff');
                   }
               }

               /* CLIENT-SPECIFIC STYLES */
               body,
               table,
               td,
               a {
                   -webkit-text-size-adjust: 100%;
                   -ms-text-size-adjust: 100%;
               }

               table,
               td {
                   mso-table-lspace: 0pt;
                   mso-table-rspace: 0pt;
               }

               img {
                   -ms-interpolation-mode: bicubic;
               }

               /* RESET STYLES */
               img {
                   border: 0;
                   height: auto;
                   line-height: 100%;
                   outline: none;
                   text-decoration: none;
               }

               table {
                   border-collapse: collapse !important;
               }

               body {
                   height: 100% !important;
                   margin: 0 !important;
                   padding: 0 !important;
                   width: 100% !important;
               }

               /* iOS BLUE LINKS */
               a[x-apple-data-detectors] {
                   color: inherit !important;
                   text-decoration: none !important;
                   font-size: inherit !important;
                   font-family: inherit !important;
                   font-weight: inherit !important;
                   line-height: inherit !important;
               }

               /* MOBILE STYLES */
               @media screen and (max-width:600px) {
                   h1 {
                       font-size: 32px !important;
                       line-height: 32px !important;
                   }
               }

               /* ANDROID CENTER FIX */
               div[style*="margin: 16px 0;"] {
                   margin: 0 !important;
               }
           </style>
         </head>

         <body style="background-color: #f4f4f4; margin: 0 !important; padding: 0 !important;">
           <!-- HIDDEN PREHEADER TEXT -->
           <div style="display: none; font-size: 1px; color: #fefefe; line-height: 1px; font-family: 'Lato', Helvetica, Arial, sans-serif; max-height: 0px; max-width: 0px; opacity: 0; overflow: hidden;"> 
           </div>
           <table border="0" cellpadding="0" cellspacing="0" width="100%">
               <!-- LOGO -->
               <tr>
                   <td bgcolor="#2E5077" align="center">
                       <table border="0" cellpadding="0" cellspacing="0" width="100%" style="max-width: 600px;">
                           <tr>
                               <td align="center" valign="top" style="padding: 40px 10px 40px 10px;"> </td>
                           </tr>
                       </table>
                   </td>
               </tr>
               <tr>
                   <td bgcolor="#2E5077" align="center" style="padding: 0px 10px 0px 10px;">
                       <table border="0" cellpadding="0" cellspacing="0" width="100%" style="max-width: 600px;">
                           <tr>
                               <td bgcolor="#ffffff" align="center" valign="top" style="padding: 40px 20px 20px 20px; border-radius: 4px 4px 0px 0px; color: #111111; font-family: 'Lato', Helvetica, Arial, sans-serif; font-size: 48px; font-weight: 400; letter-spacing: 4px; line-height: 48px;">
                                   <h1 style="font-size: 48px; font-weight: 400; margin: 2;"> Welcome {{userFullName}}!</h1>
                               </td>
                           </tr>
                       </table>
                   </td>
               </tr>
               <tr>
                   <td bgcolor="#f4f4f4" align="center" style="padding: 0px 10px 0px 10px;">
                       <table border="0" cellpadding="0" cellspacing="0" width="100%" style="max-width: 600px;">
                           <tr>
                               <td bgcolor="#ffffff" align="left" style="padding: 20px 30px 40px 30px; color: #666666; font-family: 'Lato', Helvetica, Arial, sans-serif; font-size: 18px; font-weight: 400; line-height: 25px;">
                                   <p style="margin: 0;">We received a request for a one-time password. Here's your OTP:  </p>
                                   <span style="width: 100%; text-align: center; display: block; margin-top: 30px; color: rgb(46, 80, 119);letter-spacing: 20px;font-size: 25px;">{{OTP}}</span>
                               </td>
                           </tr>
                           <tr>
                               <td bgcolor="#ffffff" align="left" style="padding: 0px 30px 20px 30px; color: #666666; font-family: 'Lato', Helvetica, Arial, sans-serif; font-size: 18px; font-weight: 400; line-height: 25px;">
                                   <p style="margin: 0;">If you have any questions, just reply to this email&mdash;we're always happy to help out.</p>
                               </td>
                           </tr>
                           <tr>
                               <td bgcolor="#ffffff" align="left" style="padding: 0px 30px 40px 30px; border-radius: 0px 0px 4px 4px; color: #666666; font-family: 'Lato', Helvetica, Arial, sans-serif; font-size: 18px; font-weight: 400; line-height: 25px;">
                                   <p style="margin: 0;">Cheers,<br>Safe Commerce Team</p>
                               </td>
                           </tr>
                       </table>
                   </td>
               </tr>
           </table>
         </body>

         </html>
         """;

        var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);

        return await client.SendEmailAsync(msg);
    }
    /// <summary>
    /// Sends an email to a user with a link to reset their password.
    /// </summary>
    /// <param name="toEmail">The recipient's email address.</param>
    /// <param name="userFullName">The full name of the recipient.</param>
    /// <param name="route">The URL route to the password reset page.</param>
    /// <returns>A task representing the asynchronous operation of sending an email.</returns>
    public static async Task<Response>
    SendForgotPassordTokenEmail
    (
        string toEmail,
        string userFullName,
        string route
    )
    {
        var apiKey = Environment.GetEnvironmentVariable("SendGridKey_SafeCommerce");
        var client = new SendGridClient(apiKey);
        var from = new EmailAddress("klevis.mema@ulb.be", "Klevis Mema");
        var subject = "Your link to reset your passord : ";
        var to = new EmailAddress(toEmail, userFullName);
        var plainTextContent = route;
        var htmlContent =
        $$"""
           <!DOCTYPE html>
         <html>

         <head>
             <title></title>
             <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
             <meta name="viewport" content="width=device-width, initial-scale=1">
             <meta http-equiv="X-UA-Compatible" content="IE=edge" />
             <style type="text/css">
                 @media screen {
                     @font-face {
                         font-family: 'Lato';
                         font-style: normal;
                         font-weight: 400;
                         src: local('Lato Regular'), local('Lato-Regular'), url(https://fonts.gstatic.com/s/lato/v11/qIIYRU-oROkIk8vfvxw6QvesZW2xOQ-xsNqO47m55DA.woff) format('woff');
                     }

                     @font-face {
                         font-family: 'Lato';
                         font-style: normal;
                         font-weight: 700;
                         src: local('Lato Bold'), local('Lato-Bold'), url(https://fonts.gstatic.com/s/lato/v11/qdgUG4U09HnJwhYI-uK18wLUuEpTyoUstqEm5AMlJo4.woff) format('woff');
                     }

                     @font-face {
                         font-family: 'Lato';
                         font-style: italic;
                         font-weight: 400;
                         src: local('Lato Italic'), local('Lato-Italic'), url(https://fonts.gstatic.com/s/lato/v11/RYyZNoeFgb0l7W3Vu1aSWOvvDin1pK8aKteLpeZ5c0A.woff) format('woff');
                     }

                     @font-face {
                         font-family: 'Lato';
                         font-style: italic;
                         font-weight: 700;
                         src: local('Lato Bold Italic'), local('Lato-BoldItalic'), url(https://fonts.gstatic.com/s/lato/v11/HkF_qI1x_noxlxhrhMQYELO3LdcAZYWl9Si6vvxL-qU.woff) format('woff');
                     }
                 }

                 /* CLIENT-SPECIFIC STYLES */
                 body,
                 table,
                 td,
                 a {
                     -webkit-text-size-adjust: 100%;
                     -ms-text-size-adjust: 100%;
                 }

                 table,
                 td {
                     mso-table-lspace: 0pt;
                     mso-table-rspace: 0pt;
                 }

                 img {
                     -ms-interpolation-mode: bicubic;
                 }

                 /* RESET STYLES */
                 img {
                     border: 0;
                     height: auto;
                     line-height: 100%;
                     outline: none;
                     text-decoration: none;
                 }

                 table {
                     border-collapse: collapse !important;
                 }

                 body {
                     height: 100% !important;
                     margin: 0 !important;
                     padding: 0 !important;
                     width: 100% !important;
                 }

                 /* iOS BLUE LINKS */
                 a[x-apple-data-detectors] {
                     color: inherit !important;
                     text-decoration: none !important;
                     font-size: inherit !important;
                     font-family: inherit !important;
                     font-weight: inherit !important;
                     line-height: inherit !important;
                 }

                 /* MOBILE STYLES */
                 @media screen and (max-width:600px) {
                     h1 {
                         font-size: 32px !important;
                         line-height: 32px !important;
                     }
                 }

                 /* ANDROID CENTER FIX */
                 div[style*="margin: 16px 0;"] {
                     margin: 0 !important;
                 }
             </style>
         </head>

         <body style="background-color: #f4f4f4; margin: 0 !important; padding: 0 !important;">
             <!-- HIDDEN PREHEADER TEXT -->
             <div style="display: none; font-size: 1px; color: #fefefe; line-height: 1px; font-family: 'Lato', Helvetica, Arial, sans-serif; max-height: 0px; max-width: 0px; opacity: 0; overflow: hidden;"> 
                We noticed you forgot your password. To reset it, please click the button below:
             </div>
             <table border="0" cellpadding="0" cellspacing="0" width="100%">
                 <!-- LOGO -->
                 <tr>
                     <td bgcolor="#2E5077" align="center">
                         <table border="0" cellpadding="0" cellspacing="0" width="100%" style="max-width: 600px;">
                             <tr>
                                 <td align="center" valign="top" style="padding: 40px 10px 40px 10px;"> </td>
                             </tr>
                         </table>
                     </td>
                 </tr>
                 <tr>
                     <td bgcolor="#2E5077" align="center" style="padding: 0px 10px 0px 10px;">
                         <table border="0" cellpadding="0" cellspacing="0" width="100%" style="max-width: 600px;">
                             <tr>
                                 <td bgcolor="#ffffff" align="center" valign="top" style="padding: 40px 20px 20px 20px; border-radius: 4px 4px 0px 0px; color: #111111; font-family: 'Lato', Helvetica, Arial, sans-serif; font-size: 48px; font-weight: 400; letter-spacing: 4px; line-height: 48px;">
                                     <h1 style="font-size: 48px; font-weight: 400; margin: 2;">{{userFullName}}!</h1> <img src="https://cdn-icons-png.flaticon.com/512/4992/4992489.png" width="125" height="120" style="display: block; border: 0px;" />
                                 </td>
                             </tr>
                         </table>
                     </td>
                 </tr>
                 <tr>
                     <td bgcolor="#f4f4f4" align="center" style="padding: 0px 10px 0px 10px;">
                         <table border="0" cellpadding="0" cellspacing="0" width="100%" style="max-width: 600px;">
                             <tr>
                                 <td bgcolor="#ffffff" align="left" style="padding: 20px 30px 40px 30px; color: #666666; font-family: 'Lato', Helvetica, Arial, sans-serif; font-size: 18px; font-weight: 400; line-height: 25px;">
                                     <p style="margin: 0;">We noticed you forgot your password. To reset it, please click the button below: </p>
                                 </td>
                             </tr>
                             <tr>
                                 <td bgcolor="#ffffff" align="left">
                                     <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                         <tr>
                                             <td bgcolor="#ffffff" align="center" style="padding: 20px 30px 60px 30px;">
                                                 <table border="0" cellspacing="0" cellpadding="0">
                                                     <tr>
                                                         <td align="center" style="border-radius: 3px;" bgcolor="#2E5077"><a href="{{route}}" target="_blank" style="font-size: 20px; font-family: Helvetica, Arial, sans-serif; color: #ffffff; text-decoration: none; color: #ffffff; text-decoration: none; padding: 15px 25px; border-radius: 2px; border: 1px solid #2E5077; display: inline-block;">Reset Password</a></td>
                                                     </tr>
                                                 </table>
                                             </td>
                                         </tr>
                                     </table>
                                 </td>
                             </tr> <!-- COPY -->
                             <tr>
                                 <td bgcolor="#ffffff" align="left" style="padding: 0px 30px 0px 30px; color: #666666; font-family: 'Lato', Helvetica, Arial, sans-serif; font-size: 18px; font-weight: 400; line-height: 25px;">
                                     <p style="margin: 0;">If that doesn't work, copy and paste the following link in your browser:</p>
                                 </td>
                             </tr> <!-- COPY -->
                             <tr>
                                 <td bgcolor="#ffffff" align="left" style="padding: 20px 30px 20px 30px; color: #666666; font-family: 'Lato', Helvetica, Arial, sans-serif; font-size: 18px; font-weight: 400; line-height: 25px;">
                                     <p style="margin: 0;"><a href="#" target="_blank" style="color:#2E5077;">{{route}}</a></p>
                                 </td>
                             </tr>
                             <tr>
                                 <td bgcolor="#ffffff" align="left" style="padding: 0px 30px 20px 30px; color: #666666; font-family: 'Lato', Helvetica, Arial, sans-serif; font-size: 18px; font-weight: 400; line-height: 25px;">
                                     <p style="margin: 0;">If you have any questions, just reply to this email&mdash;we're always happy to help out.</p>
                                 </td>
                             </tr>
                             <tr>
                                 <td bgcolor="#ffffff" align="left" style="padding: 0px 30px 40px 30px; border-radius: 0px 0px 4px 4px; color: #666666; font-family: 'Lato', Helvetica, Arial, sans-serif; font-size: 18px; font-weight: 400; line-height: 25px;">
                                     <p style="margin: 0;">Cheers,<br>Safe Commerce Team</p>
                                 </td>
                             </tr>
                         </table>
                     </td>
                 </tr>
             </table>
         </body>

         </html>
         """;

        var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);

        return await client.SendEmailAsync(msg);
    }
    /// <summary>
    /// Sends an email to activate a user account.
    /// </summary>
    /// <param name="toEmail">The recipient's email address.</param>
    /// <param name="userFullName">The full name of the recipient.</param>
    /// <param name="route">The URL route to the account activation page.</param>
    /// <returns>A task representing the asynchronous operation of sending an email.</returns>
    public static async Task<Response>
    SendActivateAccountEmail
    (
       string toEmail,
       string userFullName,
       string route
    )
    {
        var apiKey = Environment.GetEnvironmentVariable("SendGridKey_SafeCommerce");
        var client = new SendGridClient(apiKey);
        var from = new EmailAddress("klevis.mema@ulb.be", "Klevis Mema");
        var subject = "Your link to activate your account : ";
        var to = new EmailAddress(toEmail, userFullName);
        var plainTextContent = route;
        var htmlContent =
        $$"""
         <!DOCTYPE html>
         <html>
         
         <head>
           <title></title>
           <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
           <meta name="viewport" content="width=device-width, initial-scale=1">
           <meta http-equiv="X-UA-Compatible" content="IE=edge" />
           <style type="text/css">
               @media screen {
                   @font-face {
                       font-family: 'Lato';
                       font-style: normal;
                       font-weight: 400;
                       src: local('Lato Regular'), local('Lato-Regular'), url(https://fonts.gstatic.com/s/lato/v11/qIIYRU-oROkIk8vfvxw6QvesZW2xOQ-xsNqO47m55DA.woff) format('woff');
                   }
         
                   @font-face {
                       font-family: 'Lato';
                       font-style: normal;
                       font-weight: 700;
                       src: local('Lato Bold'), local('Lato-Bold'), url(https://fonts.gstatic.com/s/lato/v11/qdgUG4U09HnJwhYI-uK18wLUuEpTyoUstqEm5AMlJo4.woff) format('woff');
                   }
         
                   @font-face {
                       font-family: 'Lato';
                       font-style: italic;
                       font-weight: 400;
                       src: local('Lato Italic'), local('Lato-Italic'), url(https://fonts.gstatic.com/s/lato/v11/RYyZNoeFgb0l7W3Vu1aSWOvvDin1pK8aKteLpeZ5c0A.woff) format('woff');
                   }
         
                   @font-face {
                       font-family: 'Lato';
                       font-style: italic;
                       font-weight: 700;
                       src: local('Lato Bold Italic'), local('Lato-BoldItalic'), url(https://fonts.gstatic.com/s/lato/v11/HkF_qI1x_noxlxhrhMQYELO3LdcAZYWl9Si6vvxL-qU.woff) format('woff');
                   }
               }
         
               /* CLIENT-SPECIFIC STYLES */
               body,
               table,
               td,
               a {
                   -webkit-text-size-adjust: 100%;
                   -ms-text-size-adjust: 100%;
               }
         
               table,
               td {
                   mso-table-lspace: 0pt;
                   mso-table-rspace: 0pt;
               }
         
               img {
                   -ms-interpolation-mode: bicubic;
               }
         
               /* RESET STYLES */
               img {
                   border: 0;
                   height: auto;
                   line-height: 100%;
                   outline: none;
                   text-decoration: none;
               }
         
               table {
                   border-collapse: collapse !important;
               }
         
               body {
                   height: 100% !important;
                   margin: 0 !important;
                   padding: 0 !important;
                   width: 100% !important;
               }
         
               /* iOS BLUE LINKS */
               a[x-apple-data-detectors] {
                   color: inherit !important;
                   text-decoration: none !important;
                   font-size: inherit !important;
                   font-family: inherit !important;
                   font-weight: inherit !important;
                   line-height: inherit !important;
               }
         
               /* MOBILE STYLES */
               @media screen and (max-width:600px) {
                   h1 {
                       font-size: 32px !important;
                       line-height: 32px !important;
                   }
               }
         
               /* ANDROID CENTER FIX */
               div[style*="margin: 16px 0;"] {
                   margin: 0 !important;
               }
           </style>
         </head>
         
         <body style="background-color: #f4f4f4; margin: 0 !important; padding: 0 !important;">
           <!-- HIDDEN PREHEADER TEXT -->
           <div style="display: none; font-size: 1px; color: #fefefe; line-height: 1px; font-family: 'Lato', Helvetica, Arial, sans-serif; max-height: 0px; max-width: 0px; opacity: 0; overflow: hidden;"> 
              We noticed you forgot your password. To reset it, please click the button below:
           </div>
           <table border="0" cellpadding="0" cellspacing="0" width="100%">
               <!-- LOGO -->
               <tr>
                     <td bgcolor="#2E5077" align="center">
                           <table border="0" cellpadding="0" cellspacing="0" width="100%" style="max-width: 600px;">
                               <tr>
                                   <td align="center" valign="top" style="padding: 40px 10px 40px 10px;"> </td>
                               </tr>
                           </table>
                     </td>
               </tr>
               <tr>
                   <td bgcolor="#2E5077" align="center" style="padding: 0px 10px 0px 10px;">
                       <table border="0" cellpadding="0" cellspacing="0" width="100%" style="max-width: 600px;">
                           <tr>
                               <td bgcolor="#ffffff" align="center" valign="top" style="padding: 40px 20px 20px 20px; border-radius: 4px 4px 0px 0px; color: #111111; font-family: 'Lato', Helvetica, Arial, sans-serif; font-size: 48px; font-weight: 400; letter-spacing: 4px; line-height: 48px;">
                                   <h1 style="font-size: 48px; font-weight: 400; margin: 2;"> Hello {{userFullName}}!</h1> <img src=" https://img.icons8.com/clouds/100/000000/handshake.png" width="125" height="120" style="display: block; border: 0px;" />
                               </td>
                           </tr>
                       </table>
                   </td>
               </tr>
               <tr>
                   <td bgcolor="#f4f4f4" align="center" style="padding: 0px 10px 0px 10px;">
                       <table border="0" cellpadding="0" cellspacing="0" width="100%" style="max-width: 600px;">
                           <tr>
                               <td bgcolor="#ffffff" align="left" style="padding: 20px 30px 40px 30px; color: #666666; font-family: 'Lato', Helvetica, Arial, sans-serif; font-size: 18px; font-weight: 400; line-height: 25px;">
                                   <p style="margin: 0;">We noticed you want to return to us. To reactivate your account, please click the button below:  </p>
                               </td>
                           </tr>
                            <tr>
                                 <td bgcolor="#ffffff" align="left">
                                     <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                         <tr>
                                             <td bgcolor="#ffffff" align="center" style="padding: 20px 30px 60px 30px;">
                                                 <table border="0" cellspacing="0" cellpadding="0">
                                                     <tr>
                                                         <td align="center" style="border-radius: 3px;" bgcolor="#2E5077"><a href="{{route}}" target="_blank" style="font-size: 20px; font-family: Helvetica, Arial, sans-serif; color: #ffffff; text-decoration: none; color: #ffffff; text-decoration: none; padding: 15px 25px; border-radius: 2px; border: 1px solid #2E5077; display: inline-block;">Activate Account</a></td>
                                                     </tr>
                                                 </table>
                                             </td>
                                         </tr>
                                     </table>
                                 </td>
                             </tr> <!-- COPY -->
                             <tr>
                                 <td bgcolor="#ffffff" align="left" style="padding: 0px 30px 0px 30px; color: #666666; font-family: 'Lato', Helvetica, Arial, sans-serif; font-size: 18px; font-weight: 400; line-height: 25px;">
                                     <p style="margin: 0;">If that doesn't work, copy and paste the following link in your browser:</p>
                                 </td>
                             </tr> <!-- COPY -->
                             <tr>
                                 <td bgcolor="#ffffff" align="left" style="padding: 20px 30px 20px 30px; color: #666666; font-family: 'Lato', Helvetica, Arial, sans-serif; font-size: 18px; font-weight: 400; line-height: 25px;">
                                     <p style="margin: 0;"><a href="#" target="_blank" style="color:#2E5077;">{{route}}</a></p>
                                 </td>
                             </tr>
                             <tr>
                                 <td bgcolor="#ffffff" align="left" style="padding: 0px 30px 20px 30px; color: #666666; font-family: 'Lato', Helvetica, Arial, sans-serif; font-size: 18px; font-weight: 400; line-height: 25px;">
                                     <p style="margin: 0;">If you have any questions, just reply to this email&mdash;we're always happy to help out.</p>
                                 </td>
                             </tr>
                             <tr>
                                 <td bgcolor="#ffffff" align="left" style="padding: 0px 30px 40px 30px; border-radius: 0px 0px 4px 4px; color: #666666; font-family: 'Lato', Helvetica, Arial, sans-serif; font-size: 18px; font-weight: 400; line-height: 25px;">
                                     <p style="margin: 0;">Cheers,<br>Safe Commerce Team</p>
                                 </td>
                             </tr>
                       </table>
                   </td>
               </tr>
           </table>
         </body>
         
         </html>

         """;

        var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);

        return await client.SendEmailAsync(msg);
    }
    /// <summary>
    /// Sends an email to confirm a user's email address.
    /// </summary>
    /// <param name="toEmail">The recipient's email address.</param>
    /// <param name="route">The URL route to the email confirmation page.</param>
    /// <param name="userFullName">The full name of the recipient.</param>
    /// <returns>A task representing the asynchronous operation of sending an email.</returns>
    public static async Task<Response>
    SendEmailForEmailConfirmation
    (
        string toEmail,
        string route,
        string userFullName
    )
    {
        var apiKey = Environment.GetEnvironmentVariable("SendGridKey_SafeCommerce");
        var client = new SendGridClient(apiKey);
        var from = new EmailAddress("klevis.mema@ulb.be", "Klevis Mema");
        var subject = "Your link to confirm email : ";
        var to = new EmailAddress(toEmail, userFullName);
        var plainTextContent = route;
        var htmlContent =
        $$"""
         <!DOCTYPE html>
         <html>
         
         <head>
           <title></title>
           <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
           <meta name="viewport" content="width=device-width, initial-scale=1">
           <meta http-equiv="X-UA-Compatible" content="IE=edge" />
           <style type="text/css">
               @media screen {
                   @font-face {
                       font-family: 'Lato';
                       font-style: normal;
                       font-weight: 400;
                       src: local('Lato Regular'), local('Lato-Regular'), url(https://fonts.gstatic.com/s/lato/v11/qIIYRU-oROkIk8vfvxw6QvesZW2xOQ-xsNqO47m55DA.woff) format('woff');
                   }
         
                   @font-face {
                       font-family: 'Lato';
                       font-style: normal;
                       font-weight: 700;
                       src: local('Lato Bold'), local('Lato-Bold'), url(https://fonts.gstatic.com/s/lato/v11/qdgUG4U09HnJwhYI-uK18wLUuEpTyoUstqEm5AMlJo4.woff) format('woff');
                   }
         
                   @font-face {
                       font-family: 'Lato';
                       font-style: italic;
                       font-weight: 400;
                       src: local('Lato Italic'), local('Lato-Italic'), url(https://fonts.gstatic.com/s/lato/v11/RYyZNoeFgb0l7W3Vu1aSWOvvDin1pK8aKteLpeZ5c0A.woff) format('woff');
                   }
         
                   @font-face {
                       font-family: 'Lato';
                       font-style: italic;
                       font-weight: 700;
                       src: local('Lato Bold Italic'), local('Lato-BoldItalic'), url(https://fonts.gstatic.com/s/lato/v11/HkF_qI1x_noxlxhrhMQYELO3LdcAZYWl9Si6vvxL-qU.woff) format('woff');
                   }
               }
         
               /* CLIENT-SPECIFIC STYLES */
               body,
               table,
               td,
               a {
                   -webkit-text-size-adjust: 100%;
                   -ms-text-size-adjust: 100%;
               }
         
               table,
               td {
                   mso-table-lspace: 0pt;
                   mso-table-rspace: 0pt;
               }
         
               img {
                   -ms-interpolation-mode: bicubic;
               }
         
               /* RESET STYLES */
               img {
                   border: 0;
                   height: auto;
                   line-height: 100%;
                   outline: none;
                   text-decoration: none;
               }
         
               table {
                   border-collapse: collapse !important;
               }
         
               body {
                   height: 100% !important;
                   margin: 0 !important;
                   padding: 0 !important;
                   width: 100% !important;
               }
         
               /* iOS BLUE LINKS */
               a[x-apple-data-detectors] {
                   color: inherit !important;
                   text-decoration: none !important;
                   font-size: inherit !important;
                   font-family: inherit !important;
                   font-weight: inherit !important;
                   line-height: inherit !important;
               }
         
               /* MOBILE STYLES */
               @media screen and (max-width:600px) {
                   h1 {
                       font-size: 32px !important;
                       line-height: 32px !important;
                   }
               }
         
               /* ANDROID CENTER FIX */
               div[style*="margin: 16px 0;"] {
                   margin: 0 !important;
               }
           </style>
         </head>
         
         <body style="background-color: #f4f4f4; margin: 0 !important; padding: 0 !important;">
           <!-- HIDDEN PREHEADER TEXT -->
           <div style="display: none; font-size: 1px; color: #fefefe; line-height: 1px; font-family: 'Lato', Helvetica, Arial, sans-serif; max-height: 0px; max-width: 0px; opacity: 0; overflow: hidden;"> 
           </div>
           <table border="0" cellpadding="0" cellspacing="0" width="100%">
               <!-- LOGO -->
               <tr>
                     <td bgcolor="#2E5077" align="center">
                           <table border="0" cellpadding="0" cellspacing="0" width="100%" style="max-width: 600px;">
                               <tr>
                                   <td align="center" valign="top" style="padding: 40px 10px 40px 10px;"> </td>
                               </tr>
                           </table>
                     </td>
               </tr>
               <tr>
                   <td bgcolor="#2E5077" align="center" style="padding: 0px 10px 0px 10px;">
                       <table border="0" cellpadding="0" cellspacing="0" width="100%" style="max-width: 600px;">
                           <tr>
                               <td bgcolor="#ffffff" align="center" valign="top" style="padding: 40px 20px 20px 20px; border-radius: 4px 4px 0px 0px; color: #111111; font-family: 'Lato', Helvetica, Arial, sans-serif; font-size: 48px; font-weight: 400; letter-spacing: 4px; line-height: 48px;">
                                   <h1 style="font-size: 48px; font-weight: 400; margin: 2;"> Hello {{userFullName}}!</h1>
                               </td>
                           </tr>
                       </table>
                   </td>
               </tr>
               <tr>
                   <td bgcolor="#f4f4f4" align="center" style="padding: 0px 10px 0px 10px;">
                       <table border="0" cellpadding="0" cellspacing="0" width="100%" style="max-width: 600px;">
                           <tr>
                               <td bgcolor="#ffffff" align="left" style="padding: 20px 30px 40px 30px; color: #666666; font-family: 'Lato', Helvetica, Arial, sans-serif; font-size: 18px; font-weight: 400; line-height: 25px;">
                                   <p style="margin: 0;">We noticed you want to change your email address, please click the button below in order to confirm it:  </p>
                               </td>
                           </tr>
                            <tr>
                                 <td bgcolor="#ffffff" align="left">
                                     <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                         <tr>
                                             <td bgcolor="#ffffff" align="center" style="padding: 20px 30px 60px 30px;">
                                                 <table border="0" cellspacing="0" cellpadding="0">
                                                     <tr>
                                                         <td align="center" style="border-radius: 3px;" bgcolor="#2E5077"><a href="{{route}}" target="_blank" style="font-size: 20px; font-family: Helvetica, Arial, sans-serif; color: #ffffff; text-decoration: none; color: #ffffff; text-decoration: none; padding: 15px 25px; border-radius: 2px; border: 1px solid #2E5077; display: inline-block;">Confirm</a></td>
                                                     </tr>
                                                 </table>
                                             </td>
                                         </tr>
                                     </table>
                                 </td>
                             </tr> <!-- COPY -->
                             <tr>
                                 <td bgcolor="#ffffff" align="left" style="padding: 0px 30px 0px 30px; color: #666666; font-family: 'Lato', Helvetica, Arial, sans-serif; font-size: 18px; font-weight: 400; line-height: 25px;">
                                     <p style="margin: 0;">If that doesn't work, copy and paste the following link in your browser:</p>
                                 </td>
                             </tr> <!-- COPY -->
                             <tr>
                                 <td bgcolor="#ffffff" align="left" style="padding: 20px 30px 20px 30px; color: #666666; font-family: 'Lato', Helvetica, Arial, sans-serif; font-size: 18px; font-weight: 400; line-height: 25px;">
                                     <p style="margin: 0;"><a href="#" target="_blank" style="color:#2E5077;">{{route}}</a></p>
                                 </td>
                             </tr>
                             <tr>
                                 <td bgcolor="#ffffff" align="left" style="padding: 0px 30px 20px 30px; color: #666666; font-family: 'Lato', Helvetica, Arial, sans-serif; font-size: 18px; font-weight: 400; line-height: 25px;">
                                     <p style="margin: 0;">If you have any questions, just reply to this email&mdash;we're always happy to help out.</p>
                                 </td>
                             </tr>
                             <tr>
                                 <td bgcolor="#ffffff" align="left" style="padding: 0px 30px 40px 30px; border-radius: 0px 0px 4px 4px; color: #666666; font-family: 'Lato', Helvetica, Arial, sans-serif; font-size: 18px; font-weight: 400; line-height: 25px;">
                                     <p style="margin: 0;">Cheers,<br>Safe Commerce Team</p>
                                 </td>
                             </tr>
                       </table>
                   </td>
               </tr>
           </table>
         </body>
         
         </html>

         """;

        var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);

        return await client.SendEmailAsync(msg);
    }
    /// <summary>
    /// Sends an email to confirm a user's registration.
    /// </summary>
    /// <param name="toEmail">The recipient's email address.</param>
    /// <param name="route">The URL route to the registration confirmation page.</param>
    /// <param name="userFullName">The full name of the recipient.</param>
    /// <returns>A task representing the asynchronous operation of sending an email.</returns>
    public static async Task<Response>
    SendEmailForRegistrationConfirmation
    (
       string toEmail,
       string route,
       string userFullName
    )
    {
        var apiKey = Environment.GetEnvironmentVariable("SendGridKey_SafeCommerce");
        var client = new SendGridClient(apiKey);
        var from = new EmailAddress("klevis.mema@ulb.be", "Klevis Mema");
        var subject = "Your link to confirm email : ";
        var to = new EmailAddress(toEmail, userFullName);
        var plainTextContent = route;
        var htmlContent =
        $$"""
         <!DOCTYPE html>
         <html>
         
         <head>
           <title></title>
           <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
           <meta name="viewport" content="width=device-width, initial-scale=1">
           <meta http-equiv="X-UA-Compatible" content="IE=edge" />
           <style type="text/css">
               @media screen {
                   @font-face {
                       font-family: 'Lato';
                       font-style: normal;
                       font-weight: 400;
                       src: local('Lato Regular'), local('Lato-Regular'), url(https://fonts.gstatic.com/s/lato/v11/qIIYRU-oROkIk8vfvxw6QvesZW2xOQ-xsNqO47m55DA.woff) format('woff');
                   }
         
                   @font-face {
                       font-family: 'Lato';
                       font-style: normal;
                       font-weight: 700;
                       src: local('Lato Bold'), local('Lato-Bold'), url(https://fonts.gstatic.com/s/lato/v11/qdgUG4U09HnJwhYI-uK18wLUuEpTyoUstqEm5AMlJo4.woff) format('woff');
                   }
         
                   @font-face {
                       font-family: 'Lato';
                       font-style: italic;
                       font-weight: 400;
                       src: local('Lato Italic'), local('Lato-Italic'), url(https://fonts.gstatic.com/s/lato/v11/RYyZNoeFgb0l7W3Vu1aSWOvvDin1pK8aKteLpeZ5c0A.woff) format('woff');
                   }
         
                   @font-face {
                       font-family: 'Lato';
                       font-style: italic;
                       font-weight: 700;
                       src: local('Lato Bold Italic'), local('Lato-BoldItalic'), url(https://fonts.gstatic.com/s/lato/v11/HkF_qI1x_noxlxhrhMQYELO3LdcAZYWl9Si6vvxL-qU.woff) format('woff');
                   }
               }
         
               /* CLIENT-SPECIFIC STYLES */
               body,
               table,
               td,
               a {
                   -webkit-text-size-adjust: 100%;
                   -ms-text-size-adjust: 100%;
               }
         
               table,
               td {
                   mso-table-lspace: 0pt;
                   mso-table-rspace: 0pt;
               }
         
               img {
                   -ms-interpolation-mode: bicubic;
               }
         
               /* RESET STYLES */
               img {
                   border: 0;
                   height: auto;
                   line-height: 100%;
                   outline: none;
                   text-decoration: none;
               }
         
               table {
                   border-collapse: collapse !important;
               }
         
               body {
                   height: 100% !important;
                   margin: 0 !important;
                   padding: 0 !important;
                   width: 100% !important;
               }
         
               /* iOS BLUE LINKS */
               a[x-apple-data-detectors] {
                   color: inherit !important;
                   text-decoration: none !important;
                   font-size: inherit !important;
                   font-family: inherit !important;
                   font-weight: inherit !important;
                   line-height: inherit !important;
               }
         
               /* MOBILE STYLES */
               @media screen and (max-width:600px) {
                   h1 {
                       font-size: 32px !important;
                       line-height: 32px !important;
                   }
               }
         
               /* ANDROID CENTER FIX */
               div[style*="margin: 16px 0;"] {
                   margin: 0 !important;
               }
           </style>
         </head>
         
         <body style="background-color: #f4f4f4; margin: 0 !important; padding: 0 !important;">
           <!-- HIDDEN PREHEADER TEXT -->
           <div style="display: none; font-size: 1px; color: #fefefe; line-height: 1px; font-family: 'Lato', Helvetica, Arial, sans-serif; max-height: 0px; max-width: 0px; opacity: 0; overflow: hidden;"> 
           </div>
           <table border="0" cellpadding="0" cellspacing="0" width="100%">
               <!-- LOGO -->
               <tr>
                     <td bgcolor="#2E5077" align="center">
                           <table border="0" cellpadding="0" cellspacing="0" width="100%" style="max-width: 600px;">
                               <tr>
                                   <td align="center" valign="top" style="padding: 40px 10px 40px 10px;"> </td>
                               </tr>
                           </table>
                     </td>
               </tr>
               <tr>
                   <td bgcolor="#2E5077" align="center" style="padding: 0px 10px 0px 10px;">
                       <table border="0" cellpadding="0" cellspacing="0" width="100%" style="max-width: 600px;">
                           <tr>
                               <td bgcolor="#ffffff" align="center" valign="top" style="padding: 40px 20px 20px 20px; border-radius: 4px 4px 0px 0px; color: #111111; font-family: 'Lato', Helvetica, Arial, sans-serif; font-size: 48px; font-weight: 400; letter-spacing: 4px; line-height: 48px;">
                                   <h1 style="font-size: 48px; font-weight: 400; margin: 2;"> Welcome to Safe Commerce {{userFullName}}!</h1> <img src=" https://img.icons8.com/clouds/100/000000/handshake.png" width="125" height="120" style="display: block; border: 0px;" />
                               </td>
                           </tr>
                       </table>
                   </td>
               </tr>
               <tr>
                   <td bgcolor="#f4f4f4" align="center" style="padding: 0px 10px 0px 10px;">
                       <table border="0" cellpadding="0" cellspacing="0" width="100%" style="max-width: 600px;">
                           <tr>
                               <td bgcolor="#ffffff" align="left" style="padding: 20px 30px 40px 30px; color: #666666; font-family: 'Lato', Helvetica, Arial, sans-serif; font-size: 18px; font-weight: 400; line-height: 25px;">
                                   <p style="margin: 0;">Please click the button below in order to confirm your registration :  </p>
                               </td>
                           </tr>
                            <tr>
                                 <td bgcolor="#ffffff" align="left">
                                     <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                         <tr>
                                             <td bgcolor="#ffffff" align="center" style="padding: 20px 30px 60px 30px;">
                                                 <table border="0" cellspacing="0" cellpadding="0">
                                                     <tr>
                                                         <td align="center" style="border-radius: 3px;" bgcolor="#2E5077"><a href="{{route}}" target="_blank" style="font-size: 20px; font-family: Helvetica, Arial, sans-serif; color: #ffffff; text-decoration: none; color: #ffffff; text-decoration: none; padding: 15px 25px; border-radius: 2px; border: 1px solid #2E5077; display: inline-block;">Confirm</a></td>
                                                     </tr>
                                                 </table>
                                             </td>
                                         </tr>
                                     </table>
                                 </td>
                             </tr> <!-- COPY -->
                             <tr>
                                 <td bgcolor="#ffffff" align="left" style="padding: 0px 30px 0px 30px; color: #666666; font-family: 'Lato', Helvetica, Arial, sans-serif; font-size: 18px; font-weight: 400; line-height: 25px;">
                                     <p style="margin: 0;">If that doesn't work, copy and paste the following link in your browser:</p>
                                 </td>
                             </tr> <!-- COPY -->
                             <tr>
                                 <td bgcolor="#ffffff" align="left" style="padding: 20px 30px 20px 30px; color: #666666; font-family: 'Lato', Helvetica, Arial, sans-serif; font-size: 18px; font-weight: 400; line-height: 25px;">
                                     <p style="margin: 0;"><a href="#" target="_blank" style="color:#2E5077;">{{route}}</a></p>
                                 </td>
                             </tr>
                             <tr>
                                 <td bgcolor="#ffffff" align="left" style="padding: 0px 30px 20px 30px; color: #666666; font-family: 'Lato', Helvetica, Arial, sans-serif; font-size: 18px; font-weight: 400; line-height: 25px;">
                                     <p style="margin: 0;">If you have any questions, just reply to this email&mdash;we're always happy to help out.</p>
                                 </td>
                             </tr>
                             <tr>
                                 <td bgcolor="#ffffff" align="left" style="padding: 0px 30px 40px 30px; border-radius: 0px 0px 4px 4px; color: #666666; font-family: 'Lato', Helvetica, Arial, sans-serif; font-size: 18px; font-weight: 400; line-height: 25px;">
                                     <p style="margin: 0;">Cheers,<br>Safe Commerce Team</p>
                                 </td>
                             </tr>
                       </table>
                   </td>
               </tr>
           </table>
         </body>
         
         </html>

         """;

        var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);

        return await client.SendEmailAsync(msg);
    }
    /// <summary>
    /// Sends an email to the user after he generated his keys reminding him the hint he used
    /// </summary>
    /// <param name="toEmail">The recipient's email address.</param>
    /// <param name="userFullName">The full name of the recipient.</param>
    /// <returns>A task representing the asynchronous operation of sending an email.</returns>
    public static async Task<Response>
    SendEmailForHintAfterKeysGenerated
    (
       string hint,
       string toEmail,
       string userFullName
    )
    {
        var apiKey = Environment.GetEnvironmentVariable("SendGridKey_SafeCommerce");
        var client = new SendGridClient(apiKey);
        var from = new EmailAddress("klevis.mema@ulb.be", "Klevis Mema");
        var subject = "Your hint";
        var to = new EmailAddress(toEmail, userFullName);
        var plainTextContent = hint;
        var htmlContent =
        $$"""
         <!DOCTYPE html>
         <html>
         
         <head>
           <title></title>
           <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
           <meta name="viewport" content="width=device-width, initial-scale=1">
           <meta http-equiv="X-UA-Compatible" content="IE=edge" />
           <style type="text/css">
               @media screen {
                   @font-face {
                       font-family: 'Lato';
                       font-style: normal;
                       font-weight: 400;
                       src: local('Lato Regular'), local('Lato-Regular'), url(https://fonts.gstatic.com/s/lato/v11/qIIYRU-oROkIk8vfvxw6QvesZW2xOQ-xsNqO47m55DA.woff) format('woff');
                   }
         
                   @font-face {
                       font-family: 'Lato';
                       font-style: normal;
                       font-weight: 700;
                       src: local('Lato Bold'), local('Lato-Bold'), url(https://fonts.gstatic.com/s/lato/v11/qdgUG4U09HnJwhYI-uK18wLUuEpTyoUstqEm5AMlJo4.woff) format('woff');
                   }
         
                   @font-face {
                       font-family: 'Lato';
                       font-style: italic;
                       font-weight: 400;
                       src: local('Lato Italic'), local('Lato-Italic'), url(https://fonts.gstatic.com/s/lato/v11/RYyZNoeFgb0l7W3Vu1aSWOvvDin1pK8aKteLpeZ5c0A.woff) format('woff');
                   }
         
                   @font-face {
                       font-family: 'Lato';
                       font-style: italic;
                       font-weight: 700;
                       src: local('Lato Bold Italic'), local('Lato-BoldItalic'), url(https://fonts.gstatic.com/s/lato/v11/HkF_qI1x_noxlxhrhMQYELO3LdcAZYWl9Si6vvxL-qU.woff) format('woff');
                   }
               }
         
               /* CLIENT-SPECIFIC STYLES */
               body,
               table,
               td,
               a {
                   -webkit-text-size-adjust: 100%;
                   -ms-text-size-adjust: 100%;
               }
         
               table,
               td {
                   mso-table-lspace: 0pt;
                   mso-table-rspace: 0pt;
               }
         
               img {
                   -ms-interpolation-mode: bicubic;
               }
         
               /* RESET STYLES */
               img {
                   border: 0;
                   height: auto;
                   line-height: 100%;
                   outline: none;
                   text-decoration: none;
               }
         
               table {
                   border-collapse: collapse !important;
               }
         
               body {
                   height: 100% !important;
                   margin: 0 !important;
                   padding: 0 !important;
                   width: 100% !important;
               }
         
               /* iOS BLUE LINKS */
               a[x-apple-data-detectors] {
                   color: inherit !important;
                   text-decoration: none !important;
                   font-size: inherit !important;
                   font-family: inherit !important;
                   font-weight: inherit !important;
                   line-height: inherit !important;
               }
         
               /* MOBILE STYLES */
               @media screen and (max-width:600px) {
                   h1 {
                       font-size: 32px !important;
                       line-height: 32px !important;
                   }
               }
         
               /* ANDROID CENTER FIX */
               div[style*="margin: 16px 0;"] {
                   margin: 0 !important;
               }
           </style>
         </head>
         
         <body style="background-color: #f4f4f4; margin: 0 !important; padding: 0 !important;">
           <div style="display: none; font-size: 1px; color: #fefefe; line-height: 1px; font-family: 'Lato', Helvetica, Arial, sans-serif; max-height: 0px; max-width: 0px; opacity: 0; overflow: hidden;"> 
           </div>
           <table border="0" cellpadding="0" cellspacing="0" width="100%">
               <!-- LOGO -->
               <tr>
                     <td bgcolor="#2E5077" align="center">
                           <table border="0" cellpadding="0" cellspacing="0" width="100%" style="max-width: 600px;">
                               <tr>
                                   <td align="center" valign="top" style="padding: 40px 10px 40px 10px;"> </td>
                               </tr>
                           </table>
                     </td>
               </tr>
               <tr>
                   <td bgcolor="#2E5077" align="center" style="padding: 0px 10px 0px 10px;">
                       <table border="0" cellpadding="0" cellspacing="0" width="100%" style="max-width: 600px;">
                           <tr>
                               <td bgcolor="#ffffff" align="center" valign="top" style="padding: 40px 20px 20px 20px; border-radius: 4px 4px 0px 0px; color: #111111; font-family: 'Lato', Helvetica, Arial, sans-serif; font-size: 48px; font-weight: 400; letter-spacing: 4px; line-height: 48px;">
                                   <h1 style="font-size: 48px; font-weight: 400; margin: 2;"> Hello {{userFullName}}! this is your hint : "{{hint}}"</h1> <img src="https://cdn.pixabay.com/photo/2013/07/12/14/48/dialog-148815_1280.png" width="125" height="120" style="display: block; border: 0px;" />
                               </td>
                           </tr>
                       </table>
                   </td>
               </tr>
           </table>
         </body>
         
         </html>

         """;

        var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);

        return await client.SendEmailAsync(msg);
    }
}