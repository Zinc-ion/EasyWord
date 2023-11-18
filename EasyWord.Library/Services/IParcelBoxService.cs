namespace EasyWord.Library.Services;

//存储服务 xj实现
public interface IParcelBoxService {
    string Put(object o);

    object Get(string ticket);
}