namespace LOLMessageDelivery.Classes
{
        public static class EmailTextTemplate
        {
            public static readonly string EmailText =
                "Hello @Model.FirstName @Model.LastName!\n\nYour new account password for Reliant Employee Portal.\nLogin @Model.LoginName\nNew password @Model.Password\n\n" +
                "Please save this message in secure place.\nBest wishes,\n The Reliant Employee Portal team.";

/*
            public static readonly string EmailOfferCandidateText =
                "Dear @Model.CandidateFullName!\n\nWe are pleased to take you an offer to join the Reliant Rehab team.\n Please click the link below to view your offer online."+
                "\nhttp://newcandidates.reliant-rehab.com/offerview.aspx?offerID=@Model.NewGuid\n\nSincerely,\n\nReliant Rehab";
*/

			public static readonly string EmailOfferHRText =
@"Date: @Model.Date
Candidate: @Model.CandidateFullName
Recruiter: @Model.OfferByFullName
Override: @Model.OverrideFullName
Amount: @Model.Amount @Model.AmountType";


		}
}
