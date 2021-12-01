using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace DropDownControls {

	/// <summary>
	/// Represents a group with an associated priority.
	/// </summary>
	/// <remarks>
	/// This type is intended to be used in conjunction with <see cref="PriorityComparer"/>.
	/// </remarks>
	public class PriorityGroup : IEquatable<PriorityGroup>, IComparable {

		/// <summary>
		/// Represents a group with an empty heading and default priority.
		/// </summary>
		public static readonly PriorityGroup Empty = new PriorityGroup("");

		/// <summary>
		/// Gets the heading text for the group.
		/// </summary>
		public string Heading { get; private set; }
		/// <summary>
		/// Gets the priority of the group (lower = more important).
		/// </summary>
		public int Priority { get; private set; }

		/// <summary>
		/// Initialises a new instance of the <see cref="PriorityGroup"/> class using the specified values.
		/// </summary>
		/// <param name="heading">Heading text for the group.</param>
		/// <param name="priority">Priority of the group (lower = more important).</param>
		public PriorityGroup(string heading, int priority = 1) {
			Heading = heading ?? "";
			Priority = priority;
		}

		/// <summary>
		/// Returns the hash code for the group.
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode() {
			return Priority.GetHashCode() ^ Heading.GetHashCode();
		}

		/// <summary>
		/// Determines whether the specified object is equal to the current group.
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj) {
			PriorityGroup that = obj as PriorityGroup;
			if (that != null)
				return Equals(that);
			else
				return base.Equals(obj);
		}

		/// <summary>
		/// Determines whether the specified group is equal to the current group.
		/// </summary>
		/// <param name="that"></param>
		/// <returns></returns>
		public bool Equals(PriorityGroup that) {
			return (this.Priority == that.Priority) && this.Heading.Equals(that.Heading);
		}

		/// <summary>
		/// Returns the heading text for the group.
		/// </summary>
		/// <returns></returns>
		public override string ToString() {
			return Heading;
		}

		int IComparable.CompareTo(object obj) {
			return PriorityComparer.Compare(this, obj, Comparer.Default);
		}
	}

	/// <summary>
	/// Custom <see cref="IComparer"/> implementation that compares <see cref="PriorityGroup"/> values.
	/// </summary>
	public class PriorityComparer : IComparer {

		private IComparer _fallback;

		/// <summary>
		/// Initialises a new instance of the <see cref="PriorityComparer"/> class using default values.
		/// </summary>
		public PriorityComparer() {
			_fallback = Comparer.Default;
		}

		/// <summary>
		/// Initialises a new instance of the <see cref="PriorityComparer"/> class using the specified fallback comparer.
		/// </summary>
		/// <param name="fallback">Implementation of <see cref="IComparer"/> to use when comparing values of other types.</param>
		public PriorityComparer(IComparer fallback) {
			if (fallback == null) throw new ArgumentNullException("fallback");
			_fallback = fallback;
		}

		/// <summary>
		/// Compares two objects and returns a value indicating whether one is less than, equal to or greater than the other.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		public int Compare(object x, object y) {
			return Compare(x, y, _fallback);
		}

		/// <summary>
		/// Compares two objects and returns a value indicating whether one is less than, equal to or greater than the other.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="fallback"></param>
		/// <returns></returns>
		internal static int Compare(object x, object y, IComparer fallback) {
			object headingX = x;
			object headingY = y;
			int priorityX = 1;
			int priorityY = 1;

			PriorityGroup gX = x as PriorityGroup;
			if (gX != null) {
				headingX = gX.Heading;
				priorityX = gX.Priority;
			}

			PriorityGroup gY = y as PriorityGroup;
			if (gY != null) {
				headingY = gY.Heading;
				priorityY = gY.Priority;
			}

			int result = fallback.Compare(priorityX, priorityY);
			if (result == 0)
				return fallback.Compare(headingX, headingY);
			else
				return result;
		}
	}
}
