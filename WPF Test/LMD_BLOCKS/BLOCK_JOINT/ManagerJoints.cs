using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Test.LMD_BLOCKS.BLOCK_JOINT
{
	public class ManagerJoints
	{
		#region variables
		private List<BASE_BLOCK> joints;
		#endregion
		#region ManagerJoints
		public ManagerJoints()
		{
			this.joints = new List<BASE_BLOCK>();
		}
		#endregion
		#region Add
		public void Add(System.Windows.UIElement el1, System.Windows.UIElement el2)
		{
			BASE_BLOCK bb = new BASE_BLOCK();
		//	bb.element_begin = el1;
		//	bb.element_end = el2;

			joints.Add(bb);
		}
		#endregion
		#region Remove
		public void Remove(BASE_BLOCK joint)
		{
			joints.Remove(joint);
		}
		public void Remove(int index)
		{
			joints.RemoveAt(index);
		}
		#endregion
		#region GetAllJoints
		public List<BASE_BLOCK> GetAllJoints()
		{
			return this.joints;
		}
		#endregion
	}
}
