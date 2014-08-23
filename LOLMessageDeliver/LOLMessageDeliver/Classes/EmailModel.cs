using System;

namespace LOLMessageDelivery.Classes
{
    public class EmailModel
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string LoginName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }

    public class EmailMessageModel
    {
        public string FromFullName { get; set; }
        public string FromAddress { get; set; }
        public string ToFullName { get; set; }
        public string ToAddress { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }

    public class EmailOfferHRModel
    {
        public string NewGuid { get; set; }

		public string Date { get; set; }
		public string CandidateFullName { get; set; }
		public string OfferByFullName  { get; set; }
		public string OverrideFullName { get; set; }
		public string Amount { get; set; }
		public string AmountType { get; set; }
	}

}