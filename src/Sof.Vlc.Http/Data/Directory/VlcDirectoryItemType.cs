using System.Xml.Serialization;

namespace Sof.Vlc.Http.Data
{
	/// <summary>
	/// Whether the item is a folder or file.
	/// </summary>
	public enum VlcDirectoryItemType
	{
		[XmlEnum("dir")]
		Directory,
		[XmlEnum("file")]
		File,
	}
}
