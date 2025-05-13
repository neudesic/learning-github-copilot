
export const companyAndIndustries = [
    {
        industry: "AI",
        companies: ["apple", "google", "microsoft"],
    },
    {
        industry: "Finance",
        companies: ["goldman sachs", "jp morgan", "bank of america"],
    },
    {
        industry: "Retail",
        companies: ["walmart", "target", "costco"],
    },
]

const CompanyAndIndustry = ({
    companyName
}:
    {
        companyName: string;
    }) => {


    const findIndustry = (companyName: string) => {
        const industry = companyAndIndustries.find(industry => industry.companies.includes(companyName.toLowerCase()));
        return industry ? industry.industry : "Unknown Industry";
    }


    return (

        <div>
            <h1>Company and Industry</h1>
            <p>Company Name: {companyName}</p>
            <p>Industry: {findIndustry(companyName)}</p>
        </div>
    )

}

export default CompanyAndIndustry;