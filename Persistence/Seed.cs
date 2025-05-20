namespace Persistence
{
    public class Seed
    {
        public List<Address> GenerateFakeAddresses(int count = 100)
        {
            var addressFaker = new Faker<Address>()
                .RuleFor(a => a.Id, f => Guid.NewGuid())
                .RuleFor(a => a.Country, f => f.Address.Country())
                .RuleFor(a => a.State, f => f.Address.State())
                .RuleFor(a => a.City, f => f.Address.City())
                .RuleFor(a => a.PinCode, f => f.Address.ZipCode())
                .RuleFor(a => a.LandMark, f => f.Address.SecondaryAddress())
                .RuleFor(a => a.CreatedAt, f => f.Date.Past())
                .RuleFor(a => a.UpdatedAt, f => f.Date.Recent());

            return addressFaker.Generate(count);
        }
    }
}