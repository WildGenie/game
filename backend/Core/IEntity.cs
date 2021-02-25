namespace Core
{
	public interface IEntity<T>
	{
		T Id { get; set; }
	}
}