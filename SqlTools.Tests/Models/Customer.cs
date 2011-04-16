using System;

namespace SqlTools.Tests.Models
{
	public class Customer
	{
		private int _iD;
		public int ID
		{
			get
			{
				return _iD;
			}
			set
			{
				_iD = value;
			}
		}

		private string _firstName;
		public string FirstName
		{
			get
			{
				return _firstName;
			}
			set
			{
				_firstName = value;
			}
		}
		private string _middleInitial;
		public string MiddleInitial
		{
			get
			{
				return _middleInitial;
			}
			set
			{
				_middleInitial = value;
			}
		}

		private string _lastName;
		public string LastName
		{
			get
			{
				return _lastName;
			}
			set
			{
				_lastName = value;
			}
		}

	}
}
