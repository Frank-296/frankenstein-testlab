#nullable disable
using Bogus;

namespace TestLab.Applications.MicrosoftStore.Utilities;

public class Customer
{
    public String Name { get; set; }

    public String LastName { get; set; }

    public String MothersLastName { get; set; }

    public DateTime BirthDate { get; set; }

    public String PlaceOfBirth { get; set; }

    public String Email { get; set; }

    public String Gender { get; set; }

    public String NationalityType { get; set; }

    public String Country { get; set; }

    public String PhoneNumber { get; set; }

    public String TaxRegime { get; set; }

    public String UseOfServiceCfdi { get; set; }

    public String UseOfCfdiOfEquipment { get; set; }

    public String TypeOfAddress { get; set; }

    public String PreferredAddress { get; set; }

    public String TypeOfStreet { get; set; }

    public String Street { get; set; }

    public String OutdoorNumber { get; set; }

    public String ZipCode { get; set; }

    public String MayorOrMunicipality { get; set; }

    public String City { get; set; }

    public String State { get; set; }

    public String Suburb { get; set; }
}

public class DummyData
{
    public static Customer GenerateDummyCustomer(List<DataPool> datapool)
    {
        var dummyCustomer = new Faker<Customer>(locale: "es_MX")
            .RuleFor(x => x.Name, f => f.Person.FirstName.ToUpper())
            .RuleFor(x => x.LastName, f => f.Person.LastName.ToUpper())
            .RuleFor(x => x.MothersLastName, f => f.Name.LastName().ToUpper())
            .RuleFor(x => x.BirthDate, f => f.Person.DateOfBirth)
            .RuleFor(x => x.Email, f => f.Internet.Email())
            .RuleFor(x => x.Gender, f => f.Person.Gender.ToString() == "Female" ? "Femenino" : "Masculino")
            .RuleFor(x => x.PhoneNumber, f => "55" + f.Random.Number(10000000, 99999999).ToString())
            .Generate();

        dummyCustomer.Email = datapool.FirstOrDefault(x => x.Parameter == "Email").Value;
        dummyCustomer.PlaceOfBirth = datapool.FirstOrDefault(x => x.Parameter == "PlaceOfBirth").Value;
        dummyCustomer.NationalityType = datapool.FirstOrDefault(x => x.Parameter == "NationalityType").Value;
        dummyCustomer.TaxRegime = datapool.FirstOrDefault(x => x.Parameter == "TaxRegime").Value;
        dummyCustomer.UseOfServiceCfdi = datapool.FirstOrDefault(x => x.Parameter == "UseOfServiceCFDI").Value;
        dummyCustomer.UseOfCfdiOfEquipment = datapool.FirstOrDefault(x => x.Parameter == "UseOfCFDIOfEquipment").Value;

        var withAddressInformation = Convert.ToBoolean(datapool.FirstOrDefault(x => x.Parameter == "WithAddressInformation").Value);

        if (withAddressInformation)
        {
            dummyCustomer.TypeOfAddress = datapool.FirstOrDefault(x => x.Parameter == "TypeOfAddress").Value;
            dummyCustomer.PreferredAddress = datapool.FirstOrDefault(x => x.Parameter == "PreferredAddress").Value;
            dummyCustomer.TypeOfStreet = datapool.FirstOrDefault(x => x.Parameter == "TypeOfStreet").Value;
            dummyCustomer.Street = datapool.FirstOrDefault(x => x.Parameter == "Street").Value;
            dummyCustomer.OutdoorNumber = datapool.FirstOrDefault(x => x.Parameter == "OutdoorNumber").Value;
            dummyCustomer.ZipCode = datapool.FirstOrDefault(x => x.Parameter == "ZIPCode").Value;
            dummyCustomer.MayorOrMunicipality = datapool.FirstOrDefault(x => x.Parameter == "MayorOrMunicipality").Value;
            dummyCustomer.City = datapool.FirstOrDefault(x => x.Parameter == "City").Value;
            dummyCustomer.State = datapool.FirstOrDefault(x => x.Parameter == "State").Value;
            dummyCustomer.Suburb = datapool.FirstOrDefault(x => x.Parameter == "Suburb").Value;
        }

        return dummyCustomer;
    }
}