using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace DelegatesExercise 
{
	public class UniversalProcessor<TObj, TReq> 
	{
		protected Func<TReq, bool> Check;
		protected Func<TReq, TObj> Register;
		protected Action<TObj> Save;

		public UniversalProcessor(Func<TReq, bool> checkFunc,
									Func<TReq, TObj> registerFunc,
									Action<TObj> saveAction) {
			Check = checkFunc;
			Register = registerFunc;
			Save = saveAction;
		}

		public TObj Process(TReq request) {
			if (!Check(request))
				throw new ArgumentException();
			var result = Register(request);
			Save(result);
			return result;
		}

	}

}
