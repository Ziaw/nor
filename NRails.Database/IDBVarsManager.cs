namespace NRails.Database
{
	public interface IDBVarsManager
	{
		string this[string name] { get; set; }
	}
}