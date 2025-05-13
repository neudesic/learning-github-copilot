import React from "react";

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
];

interface CompanyAndIndustryProps {
    companyName: string;
}

const CompanyAndIndustry: React.FC<CompanyAndIndustryProps> = ({ companyName }) => {
    const normalizedCompanyName = companyName.toLowerCase();
    const industry = companyAndIndustries.find((entry) =>
        entry.companies.includes(normalizedCompanyName)
    )?.industry || "Unknown Industry";

    return (
        <div>
            <h1>Company and Industry</h1>
            <p>Company Name: {companyName}</p>
            <p>Industry: {industry}</p>
        </div>
    );
};

export default CompanyAndIndustry;
