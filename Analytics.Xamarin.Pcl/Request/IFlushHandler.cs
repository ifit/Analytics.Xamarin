using System.Threading.Tasks;
using Segment.Model;

namespace Segment.Request
{
	/// <summary>
	/// A component responsibe for flushing an action to the server
	/// </summary>
	public interface IFlushHandler
	{
		/// <summary>
		/// Validates an action and begins the process of flushing it to the server
		/// </summary>
		/// <param name="action">Action.</param>
		Task Process(BaseAction action);
	}
}
