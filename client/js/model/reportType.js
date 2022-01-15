export class ReportType
{
    constructor(
        name, 
        id=null
        )
    {
        this.Name = name;
        this.ID   = id; 
    }

    static async getReportTypes()
    {
        try {
            let response = await fetch("/ReportType", {
                method: "GET",
                headers:
                {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json'
                }
            })

            let data = await response.json();
            let reportTypes = [];
            data.reportTypes.forEach(reportType=>{
                reportTypes.push(new ReportType(reportType.name, reportType.id));
            })
            return reportTypes;
        } catch (error) {
            return {error: error.message};
        }
    }

    static async getStats()
    {
        try {
            let response = await fetch("/ReportType/Stats", {
                method: "GET",
                headers:
                {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json'
                }
            })

            let data = await response.json();
            if(response.status !== 200) throw new Error(data.error);

            return data.reportTypes;
        } catch (error) {
            return {error: error.message};
        }
    }
}