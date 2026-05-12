using System.Runtime.CompilerServices;
using StromsdalKok.Domain.ValueObjects;

namespace StromsdalKok.Domain.Entities;

public class Customer : BaseDomain
{
    public FullName Name { get; private set; }
    public Email Email { get; private set; }
    public string Phone { get; private set; }
    public string Address { get; private set; }

    internal Customer(int id, FullName name, Email email,
    string phone, string address)
    {
        Id = id;
        Name = name;
        Email = email;
        Phone = phone;
        Address = address;
    }

    private Customer(FullName name, Email email, string phone, string address)
    {
        Name = name;
        Email = email;
        Phone = phone;
        Address = address;
    }

    public static Customer Create(FullName name,
        Email email, string phone, string address)
    {
        return new Customer
        (
            name,
            email,
            phone,
            address
        );
    }

    public void Update(FullName name,
        Email email, string phone, string address)
    {
        Name = name;
        Email = email;
        Phone = phone;
        Address = address;
    }
}