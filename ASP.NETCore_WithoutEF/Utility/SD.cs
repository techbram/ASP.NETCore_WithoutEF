namespace ASP.NETCore_WithoutEF.Utility
{
	public class SD
	{
		//Order Status
		public const string StatusPending = "Pending";
		public const string StatusApproved = "Approved";
		public const string StatusInProcess = "Processing";
		public const string StatusShipped = "Shipped";
		public const string StatusCencelled = "Cencelled";
		public const string StatusRefunded = "Refunded";

		//Payment Status
		public const string PaymentStatusPending = "Pending";
		public const string PaymentStatusApproved = "Approved";
		public const string PaymentStatusDelayedPayment = "ApprovedForDelayedPayment";
		public const string PaymentStatusRejected = "Rejected";
	}
}
