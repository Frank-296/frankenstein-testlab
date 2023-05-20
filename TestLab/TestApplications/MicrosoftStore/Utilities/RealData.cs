#nullable disable
namespace TestLab.Applications.MicrosoftStore.Utilities;

public class RealData
{
    public static Customer GenerateRealCustomer(List<DataPool> datapool)
    {
        var customer = new Customer
        {
            Name = datapool.FirstOrDefault(x => x.Parameter == "Name").Value.ToUpper(),
            LastName = datapool.FirstOrDefault(x => x.Parameter == "LastName").Value.ToUpper(),
            MothersLastName = datapool.FirstOrDefault(x => x.Parameter == "MothersLastName").Value.ToUpper(),
            BirthDate = DateTime.Parse(datapool.FirstOrDefault(x => x.Parameter == "BirthDate").Value),
            Gender = datapool.FirstOrDefault(x => x.Parameter == "Gender").Value,
            PhoneNumber = datapool.FirstOrDefault(x => x.Parameter == "PhoneNumber").Value,
            Email = datapool.FirstOrDefault(x => x.Parameter == "Email").Value,
            PlaceOfBirth = datapool.FirstOrDefault(x => x.Parameter == "PlaceOfBirth").Value,
            NationalityType = datapool.FirstOrDefault(x => x.Parameter == "NationalityType").Value,
            TaxRegime = datapool.FirstOrDefault(x => x.Parameter == "TaxRegime").Value,
            UseOfServiceCfdi = datapool.FirstOrDefault(x => x.Parameter == "UseOfServiceCFDI").Value,
            UseOfCfdiOfEquipment = datapool.FirstOrDefault(x => x.Parameter == "UseOfCFDIOfEquipment").Value,
            TypeOfAddress = datapool.FirstOrDefault(x => x.Parameter == "TypeOfAddress").Value,
            PreferredAddress = datapool.FirstOrDefault(x => x.Parameter == "PreferredAddress").Value,
            TypeOfStreet = datapool.FirstOrDefault(x => x.Parameter == "TypeOfStreet").Value,
            Street = datapool.FirstOrDefault(x => x.Parameter == "Street").Value,
            OutdoorNumber = datapool.FirstOrDefault(x => x.Parameter == "OutdoorNumber").Value,
            ZipCode = datapool.FirstOrDefault(x => x.Parameter == "ZIPCode").Value,
            MayorOrMunicipality = datapool.FirstOrDefault(x => x.Parameter == "MayorOrMunicipality").Value,
            City = datapool.FirstOrDefault(x => x.Parameter == "City").Value,
            State = datapool.FirstOrDefault(x => x.Parameter == "State").Value,
            Suburb = datapool.FirstOrDefault(x => x.Parameter == "Suburb").Value
        };

        return customer;
    }
}