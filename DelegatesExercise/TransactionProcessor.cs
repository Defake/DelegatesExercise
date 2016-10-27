using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace DelegatesExercise 
{
	public class TransactionProcessor 
	{

		protected Func<TransactionRequest, bool> Check;
		protected Func<TransactionRequest, Transaction> Register;
		protected Action<Transaction> Save;

		public TransactionProcessor(Func<TransactionRequest, bool> checkFunc, 
									Func<TransactionRequest, Transaction> registerFunc, 
									Action<Transaction> saveAction)
		{
			Check = checkFunc;
			Register = registerFunc;
			Save = saveAction;
		}

		public Transaction Process(TransactionRequest request) 
		{
			if (!Check(request))
				throw new ArgumentException();
			var result = Register(request);
			Save(result);
			return result;
		}

	}

	public class TransactionRequest {

	}
}
