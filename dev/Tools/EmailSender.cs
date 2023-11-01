/* Copyright (C) 2023 Emeric Delacroix - All Rights Reserved
 * You may use, distribute and modify this code under the
 * terms of the MagicAppCopyright license.
 *
 * You should have received a copy of the MagicAppCopyright license with
 * this file. If not, please write to: delacroix.emeric@gmail.com
 */

using System.Security.Claims;

using Orchestre.API.Tools;
using MimeKit;
using MailKit.Net.Smtp;
using MimeKit.Text;
using MailKit.Security;
using Microsoft.Extensions.Configuration;

namespace MagicAppAPI.Tools
{
	public class EmailSender
	{
		#region Constants

		/// <summary>Personal email.</summary>
		public static string EMAIL_SENDER = "delacroix.emeric@gmail.com";

		// This password was generated on gmail, see https://support.google.com/accounts/answer/185833 for further details
		private static string EMAIL_SENDER_APPLICATION_PASSWORD = "tata zjxs qqiq hhxy";

		/// <summary>SMTP used to send emails.</summary>
		private static string CURRENT_SMTP = "smtp.gmail.com";

		/// <summary>Port used to send emails.</summary>
		private static int CURRENT_PORT = 587;

		/// <summary>Account validation base link.</summary>
		private static string VERIFY_LINK = "www.magic-app-online.fr/verify/";

		/// <summary>Current website security ("http://" or "https://").</summary>
		private static string CURRENT_WEBSITE_SECURITY = "http://";

		private static string EMAIL_SIGNATURE = "<p>Cordialement</p><p>Le Webmaster<br />Emeric Delacroix</p>";

		#endregion Constants

		/// <summary>Sends an account validation email to the specific email.</summary>
		/// <param name="email">Recipient email.</param>
		public static void SendEmailVerificationMessage(string email, string emailSignature)
		{
			string subject = "[Magic The Gathering Online] Validation du compte";
			string link = GenerateLink(VERIFY_LINK, email);
			string body = @"<html>
                <head>
                    <style>
                        .centered {
                            padding-left:100px;
                        }
                    </style>
                </head>
                <body>
                        <p>Bienvenue sur le site Magic the Gathering Online !</p>
                        <p>Veuillez valider votre compte en cliquant sur le lien suivant : </p>";
			body += $"  <p class=\"centered\"><a href={link}>Valider mon compte</a></p>";
			body += $"  {(emailSignature != null ? emailSignature : EMAIL_SIGNATURE)}";
			body += @"</ body>
            </ html > ";

			SendEmails(new List<string> { email }, subject, body);
		}

		public static void SendResetPasswordMessage(string email, string password, string emailSignature)
		{
			string subject = "[Magic The Gathering Online] Réinitialisation du mot de passe";

			string body = @"<html>
                <head>
                </head>
                <body>";
			body += $"  <p>Vous avez effectué une demande de réinitialisation de mot de passe</p>";
			body += $"  <p>Votre mot de passe temporaire est : {password}</p>";
			body += @"  <p>Pensez à le modifier dans l'espace ""Mon Compte / Mes identifiants"".</p>";
			body += $"  {(emailSignature != null ? emailSignature : EMAIL_SIGNATURE)}";
			body += @"</ body>
            </ html > ";

			SendEmails(new List<string> { email }, subject, body);
		}

		/// <summary>Sends an email to the specific emails.</summary>
		/// <param name="emails">Recipient emails.</param>
		/// <param name="subject">Email subject.</param>
		/// <param name="body">Email content.</param>
		private static void SendEmails(List<string> emails, string subject, string body)
		{
			if (emails.Count == 0)
				return;

			var msg = new MimeMessage();

			msg.From.Add(MailboxAddress.Parse(EMAIL_SENDER));
			if (emails.Count > 1)
			{
				foreach (var email in emails)
					msg.Bcc.Add(MailboxAddress.Parse(email));
			}
			else
			{
				msg.To.Add(MailboxAddress.Parse(emails[0]));
			}
			msg.Subject = subject;
			msg.Body = new TextPart(TextFormat.Html) { Text = body };

			// send email
			using var smtp = new SmtpClient();
			smtp.Connect(CURRENT_SMTP, CURRENT_PORT, false);
			smtp.Authenticate(EMAIL_SENDER, EMAIL_SENDER_APPLICATION_PASSWORD);

			smtp.Send(msg);
			smtp.Disconnect(true);
		}

		/// <summary>Generates account validation link.</summary>
		/// <param name="baseLink">Account validation base link.</param>
		/// <param name="email">User email.</param>
		/// <returns>The generated link.</returns>
		private static string GenerateLink(string baseLink, string email)
		{
			List<Claim> claims = new List<Claim>
			{
				new Claim (ClaimTypes.Email, email)
			};
			string token = TokenGenerator.GenerateJwtToken(claims);
			return CURRENT_WEBSITE_SECURITY + baseLink + token;
		}
	}
}
