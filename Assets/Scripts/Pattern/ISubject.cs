public interface ISubject
{
    // Update로 안하는 이유는 Unity의 Update와 겹치기 때문
    public void UpdateObserver();
    public void RegistObserver(IObserver obverser);
    public void RemoveObserver(IObserver obverser);
}
