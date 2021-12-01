using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Defines required behaviour for a time picker source control.
/// </summary>
public interface ITimePicker {

	/// <summary>
	/// Gets or sets the time of day value.
	/// </summary>
	TimeSpan? Value { get; set; }
}
