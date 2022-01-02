export class User
{
    constructor(username, password, fullname=null, priority=null, reports=null, servers=null, id=null)
    {
        this.ID       = id;
        this.Username = username;
        this.FullName = fullname;
        this.Password = password;
        this.Priority = priority;
        this.Reports  = reports;
        this.Servers  = servers;
    }

    async login()
    {
        try {
            let res = await fetch("/User/login", 
            {
                method: "POST",
                body: JSON.stringify({
                    Username: this.Username,
                    Password: this.Password
                }),
                headers:
                {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json'
                }
            });
            let data = await res.json();
            if(res.status !== 200) throw new Error(data.error);
            if(!data && !data.token) throw new Error("Something goes wrong. Please try again.");
            return { token: data.token };
        } catch (error) {
            return {error: error.message};
        }
    }

    async register()
    {
        try {
            let res = await fetch("/User/", 
            {
                method: "POST",
                body: JSON.stringify({
                    Username: this.Username,
                    Password: this.Password,
                    FullName: this.FullName,
                    Priority: this.Priority
                }),
                headers:
                {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json'
                }
            });
            let data = await res.json();
            if(res.status !== 200) throw new Error(data.error);
            if(!data && !data.token) throw new Error("Something goes wrong. Please try again.");
            return { token: data.token };
        } catch (error) {
            return {error: error.message};
        }
    }
}