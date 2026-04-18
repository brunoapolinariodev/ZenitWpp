using ZenitWpp.Domain.Common;

namespace ZenitWpp.Domain.Contacts.ValueObjects;

public class Email : ValueObject
{
    public string Value { get; }

    private Email(string value) => Value = value;

    public static Email Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || !value.Contains('@'))
            throw new ArgumentException("E-mail inválido.");

        return new Email(value.Trim().ToLower());
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;
}
