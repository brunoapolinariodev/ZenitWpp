using ZenitWpp.Domain.Common;

namespace ZenitWpp.Domain.Contacts.ValueObjects;

public class PhoneNumber : ValueObject
{
    public string Value { get; }

    private PhoneNumber(string value) => Value = value;

    public static PhoneNumber Create(string value)
    {
        var cleaned = new string(value.Where(char.IsDigit).ToArray());

        if (cleaned.Length < 10 || cleaned.Length > 15)
            throw new ArgumentException("Número de telefone inválido.");

        return new PhoneNumber(cleaned);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;
}
