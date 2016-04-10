
using System.Collections;

namespace InGameLogic
{
	public class UnitStateRest : UnitState {

		public UnitStateRest(BattleUnit bu) : base(UnitStateType.Rest, bu)
		{
		}

		public override void Update ()
		{
			base.Update ();
		}
	}
}