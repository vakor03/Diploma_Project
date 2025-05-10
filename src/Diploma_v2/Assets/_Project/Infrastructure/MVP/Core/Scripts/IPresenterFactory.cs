namespace _Project.Infrastructure.MVP.Core {
    public interface IPresenterFactory {
        public PresenterBehaviour CreatePresenterForView(ViewBehaviour view);
    }
}