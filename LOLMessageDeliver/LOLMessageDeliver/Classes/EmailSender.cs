using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Net.Mail;
using RazorEngine;
using System.Web;

namespace LOLMessageDelivery.Classes
{
    public class EmailSender
    {
        private readonly SqlConnection _connection;
        private readonly DataSet _dataSet = new DataSet();
        private SqlCommand _command;
        private SqlDataAdapter _adapter;
        private EmailModel _template = new EmailModel();
        private SqlTransaction _transaction;

        public EmailSender()
        {
            _connection =
                new SqlConnection(ConfigurationManager.ConnectionStrings["ReliantEmployeePortal"].ConnectionString);
        }

        //public void SendMailMessage(ForgetPassword forgetPassword)
        //{
        //    string userName = forgetPassword.UserName;
        //    string userEmail = forgetPassword.Password;

        //    try
        //    {
        //        _connection.Open();
        //        _transaction = _connection.BeginTransaction();

        //        _command = new SqlCommand
        //        {
        //            CommandText = "SELECT rep_Users.UserID, rep_Persons.PersonFirstName, rep_Persons.PersonLastName,"
        //                            + " rep_Users.UserName, rep_Users.UserPassword, rep_Persons.PersonEmailAddress"
        //                            + " FROM rep_Users, rep_Persons"
        //                            + " WHERE (rep_Users.UserName = @UserName OR @UserName IS NULL) AND"
        //                            + " (rep_Persons.PersonEmailAddress = @PersonEmailAddress OR @PersonEmailAddress IS NULL ) AND"
        //                            + " rep_Users.UserPersonID = rep_Persons.PersonID AND (@UserName IS NOT NULL OR @PersonEmailAddress IS NOT NULL)",
        //            Connection = _connection,
        //            Transaction = _transaction
        //        };

        //        _command.Parameters.Add("@UserName", SqlDbType.NVarChar, 20).Value = string.IsNullOrEmpty(userName) ? DBNull.Value : (object)userName;
        //        //_command.Parameters.Add("@PersonEmailAddress", SqlDbType.NVarChar, 100).Value = string.IsNullOrEmpty(userEmail) ? DBNull.Value : (object)userEmail;
        //        _command.Parameters.Add("@PersonEmailAddress", SqlDbType.NVarChar, 200).Value =
        //         string.IsNullOrEmpty(userEmail) ? DBNull.Value : (object)Cryptography.Encrypt(userEmail);

        //        _adapter = new SqlDataAdapter
        //                       {
        //                           SelectCommand = _command
        //                       };

        //        _adapter.Fill(_dataSet);

        //        if (_dataSet.Tables[0].Rows.Count > 0)
        //        {
        //            _template = new EmailModel
        //                            {
        //                                UserId = int.Parse(_dataSet.Tables[0].Rows[0][0].ToString()),
        //                                FirstName =Cryptography.Decrypt( _dataSet.Tables[0].Rows[0][1].ToString()),
        //                                LastName = Cryptography.Decrypt(_dataSet.Tables[0].Rows[0][2].ToString()),
        //                                LoginName = _dataSet.Tables[0].Rows[0][3].ToString(),
        //                                Password = _dataSet.Tables[0].Rows[0][4].ToString(),
        //                                Email = Cryptography.Decrypt(_dataSet.Tables[0].Rows[0][5].ToString()),
        //                            };

        //            _template.Password = PasswordGenerator.Instance.GenPassword();
        //            _command.CommandText = "UPDATE rep_Users"
        //                                  + " SET rep_Users.UserPassword = @NewPasword,"
        //                                  + " rep_Users.UserLoginTries = 0,"
        //                                  + " rep_Users.UserLocked = 0,"
        //                                  + " rep_Users.UserActive = 1"
        //                                 // + " rep_Users.UserLastLogin = '" + DateTime.UtcNow + "',"
        //                                  + " WHERE rep_Users.UserID = @UserID";

        //            string encrypt = Cryptography.Encrypt(_template.Password, "Sat123TechReliantEKey289482Ja92A");
        //            _command.Parameters.Add("@NewPasword", SqlDbType.NVarChar, 50).Value = encrypt;
        //            _command.Parameters.Add("@UserID", SqlDbType.Int).Value = _template.UserId;
        //            //result.UserLastLogin = DateTime.UtcNow;

        //            _command.ExecuteNonQuery();

        //            Send(_template, _template.Email);

        //            _transaction.Commit();
        //        }
        //        else
        //        {
        //            _transaction.Rollback();
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        _transaction.Rollback();
        //        throw;
        //    }
        //    finally
        //    {
        //        _connection.Close();
        //    }
        //}

		public void SendEmailMessage(EmailMessageModel model)
		{
			var mMailMessage = new MailMessage
								   {
									   From = new MailAddress(model.FromAddress, model.FromFullName)
								   };

			mMailMessage.To.Add(new MailAddress(model.ToAddress, model.ToFullName));

			mMailMessage.Subject = model.Subject;
			mMailMessage.Body = model.Body;

			mMailMessage.Priority = MailPriority.Normal;
			mMailMessage.IsBodyHtml = true;

			var mSmtpClient = new SmtpClient();
			mSmtpClient.Send(mMailMessage);
		}

        //private SqlCommand GetSqlCommand(string userName, string userEmail)
        //{
        //    var cmd = new SqlCommand
        //    {
        //        CommandText = "SELECT rep_Users.UserID, rep_Persons.PersonFirstName, rep_Persons.PersonLastName,"
        //                        + " rep_Users.UserName, rep_Users.UserPassword, rep_Persons.PersonEmailAddress"
        //                        + " FROM rep_Users, rep_Persons"
        //                        + " WHERE (rep_Users.UserName = @UserName OR @UserName IS NULL) AND"
        //                        + " (rep_Persons.PersonEmailAddress = @PersonEmailAddress OR @PersonEmailAddress IS NULL ) AND"
        //                        + " rep_Users.UserPersonID = rep_Persons.PersonID AND (@UserName IS NOT NULL OR @PersonEmailAddress IS NOT NULL)"
        //    };

        //    cmd.Parameters.Add("@UserName", SqlDbType.NVarChar, 20).Value = string.IsNullOrEmpty(userName) ? DBNull.Value : (object)userName;
        //    cmd.Parameters.Add("@PersonEmailAddress", SqlDbType.NVarChar, 100).Value = string.IsNullOrEmpty(userEmail) ? DBNull.Value : (object)userEmail;
        //    return cmd;

        //    //SqlCommand command = null;
        //    //if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(userEmail))
        //    //{
        //    //    command = new SqlCommand
        //    //    {
        //    //        CommandText =
        //    //            "SELECT rep_Users.UserID, rep_Persons.PersonFirstName, rep_Persons.PersonLastName,"
        //    //            + " rep_Users.UserName, rep_Users.UserPassword,rep_Persons.PersonEmailAddress"
        //    //            + " FROM rep_Users, rep_Persons"
        //    //            +
        //    //            " WHERE rep_Users.UserName = @UserName and rep_Persons.PersonEmailAddress = @PersonEmailAddress and"
        //    //            + " rep_Users.UserPersonID = rep_Persons.PersonID",
        //    //        Connection = _connection,
        //    //        Transaction = _transaction
        //    //    };
        //    //    command.Parameters.Add("@UserName", SqlDbType.NVarChar, 20).Value = userName;
        //    //    command.Parameters.Add("@PersonEmailAddress", SqlDbType.NVarChar, 100).Value = userEmail;

        //    //}
        //    //else if (!string.IsNullOrEmpty(userName))
        //    //{
        //    //    command = new SqlCommand
        //    //    {
        //    //        CommandText =
        //    //            "SELECT rep_Users.UserID, rep_Persons.PersonFirstName, rep_Persons.PersonLastName,"
        //    //            + " rep_Users.UserName, rep_Users.UserPassword,rep_Persons.PersonEmailAddress"
        //    //            + " FROM rep_Users, rep_Persons"
        //    //            +
        //    //            " WHERE rep_Users.UserName = @UserName and"
        //    //            + " rep_Users.UserPersonID = rep_Persons.PersonID",
        //    //        Connection = _connection,
        //    //        Transaction = _transaction
        //    //    };
        //    //    command.Parameters.Add("@UserName", SqlDbType.NVarChar, 20).Value = userName;
        //    //}
        //    //else if (!string.IsNullOrEmpty(userEmail))
        //    //{
        //    //    command = new SqlCommand
        //    //    {
        //    //        CommandText =
        //    //             "SELECT rep_Users.UserID, rep_Persons.PersonFirstName, rep_Persons.PersonLastName,"
        //    //             + " rep_Users.UserName, rep_Users.UserPassword,rep_Persons.PersonEmailAddress"
        //    //             + " FROM rep_Users, rep_Persons"
        //    //             +
        //    //             " WHERE rep_Persons.PersonEmailAddress = @PersonEmailAddress and"
        //    //             + " rep_Users.UserPersonID = rep_Persons.PersonID",
        //    //        Connection = _connection,
        //    //        Transaction = _transaction
        //    //    };
        //    //    command.Parameters.Add("@PersonEmailAddress", SqlDbType.NVarChar, 100).Value = userEmail;
        //    //}

        //    //return command;
        //}


        private void Send(EmailModel model, string userEmail)
        {
            var mMailMessage = new MailMessage
                                   {
                                       From = new MailAddress(ConfigurationManager.AppSettings["mailFrom"])
                                   };

            mMailMessage.To.Add(new MailAddress(userEmail));

            mMailMessage.Subject = "Reliant Employee Portal : Your account information";
            mMailMessage.Body = Razor.Parse(EmailTextTemplate.EmailText, model);

            mMailMessage.Priority = MailPriority.Normal;

            var mSmtpClient = new SmtpClient();
            mSmtpClient.Send(mMailMessage);
        }

        public void SendGeneratePassword(EmailModel model, string userEmail)
        {
            var mMailMessage = new MailMessage
            {
                From = new MailAddress(ConfigurationManager.AppSettings["mailFrom"])
            };

            mMailMessage.To.Add(new MailAddress(userEmail));

            mMailMessage.Subject = "Reliant Employee Portal : Your new password for an account";
            mMailMessage.Body = Razor.Parse(EmailTextTemplate.EmailText, model);

            mMailMessage.Priority = MailPriority.Normal;

            var mSmtpClient = new SmtpClient();
            mSmtpClient.Send(mMailMessage);
        }

        public void SendOfferCandidateMail(string body, string userEmail)
        {
            var mail = new MailMessage
            {
                From = new MailAddress(ConfigurationManager.AppSettings["mailFrom"])
            };

            mail.To.Add(new MailAddress(userEmail));

            mail.Subject = "Reliant Employee Portal : Pleased to take you an offer to join the Reliant Rehab team";
          //  mail.Body = body;
			//mail.IsBodyHtml = true;
            mail.Priority = MailPriority.Normal;

			AlternateView htmlView = AlternateView.CreateAlternateViewFromString(body, null, "text/html");
			
			LinkedResource logo = new LinkedResource(HttpContext.Current.Server.MapPath("~/Resources/reliant-logo.png"));
			logo.ContentId = "logo";
			htmlView.LinkedResources.Add(logo);

			mail.AlternateViews.Add(htmlView);

            var mSmtpClient = new SmtpClient();
            mSmtpClient.Send(mail);
		}

		public void SendOfferHRMail(EmailOfferHRModel model, string userEmail)
		{
			var mail = new MailMessage
			{
				From = new MailAddress(ConfigurationManager.AppSettings["mailFrom"])
			};

			mail.To.Add(new MailAddress(userEmail));

			mail.Subject = "Reliant Employee Portal : Your offer approval required";
			mail.Body = Razor.Parse(EmailTextTemplate.EmailOfferHRText, model);
			mail.Priority = MailPriority.Normal;
			var mSmtpClient = new SmtpClient();
			mSmtpClient.Send(mail);
		}

    }
}