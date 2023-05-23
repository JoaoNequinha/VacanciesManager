using System.Net.Mail;

namespace Dashboard.Domain.Models;

public class Email
{
    private readonly MailAddress email;

    public Email(string address) => email = new MailAddress(address);

    public override string ToString() => email.ToString();
}
