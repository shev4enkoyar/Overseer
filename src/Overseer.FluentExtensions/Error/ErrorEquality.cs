namespace Overseer.FluentExtensions.Error;

public partial struct Error : IEquatable<Error>
{
    public bool Equals(Error other) => Message == other.Message && Equals(Exception, other.Exception);

    public override bool Equals(object? obj) => obj is Error other && Equals(other);

    public override int GetHashCode() => HashCode.Combine(Message, Exception);

    public static bool operator ==(Error left, Error right) => left.Equals(right);

    public static bool operator !=(Error left, Error right) => !left.Equals(right);
}
