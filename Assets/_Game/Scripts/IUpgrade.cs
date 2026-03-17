

public interface IUpgrade
{

    string Name { get;}
    float Cost { get;}
    string Description { get;}

    void ApplyUpgrade();
}
