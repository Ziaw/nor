using System;
using System.Runtime.Serialization;

namespace NRails.Database.Schema
{
	[Serializable]
	public class SchemaChangeException : ApplicationException
	{
		public SchemaChangeException(string cmdText, Exception innerException)
			: base(GetMessage(cmdText, innerException.Message), innerException)
		{}

		private static string GetMessage(string cmdText, string errorMessage)
		{
			return String.Format(cmdText, errorMessage);
		}

		protected SchemaChangeException(SerializationInfo info, StreamingContext context) : base(info, context)
		{}
	}
}
