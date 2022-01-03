export class Datacenter
{
    constructor(
        name, 
        address, 
        city, 
        country, 
        id=null, 
        servers
        )
    {
        this.ID = id;
        this.Name = name;
        this.Address = address;
        this.City = city;
        this.Country = country;
        this.Servers = servers;
    }
}