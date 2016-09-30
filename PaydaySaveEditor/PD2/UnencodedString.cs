using System;

namespace PaydaySaveEditor.PD2
{
	public class UnencodedString
	{
		public String Value { get; set; }
		public UnencodedString(String value) { this.Value = value; }
		public override String ToString() { return Value; }
	}
}
