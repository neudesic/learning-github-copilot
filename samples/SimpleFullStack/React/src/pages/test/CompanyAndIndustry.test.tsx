import { render, screen } from "@testing-library/react";
import "@testing-library/jest-dom";
import CompanyAndIndustry, { companyAndIndustries } from "./answer/AnswerCompanyAndIndustry";
import { describe, it, expect } from "vitest";

describe("CompanyAndIndustry Component", () => {
    it("renders the heading 'Company and Industry'", () => {
        render(<CompanyAndIndustry companyName="apple" />);
        expect(screen.getByText("Company and Industry")).toBeInTheDocument();
    });

    it("displays the correct company name", () => {
        render(<CompanyAndIndustry companyName="apple" />);
        expect(screen.getByText("Company Name: apple")).toBeInTheDocument();
    });

    it("displays the correct industry for a known company", () => {
        render(<CompanyAndIndustry companyName="apple" />);
        expect(screen.getByText("Industry: AI")).toBeInTheDocument();
    });

    it("displays 'Unknown Industry' for an unknown company", () => {
        render(<CompanyAndIndustry companyName="unknown" />);
        expect(screen.getByText("Industry: Unknown Industry")).toBeInTheDocument();
    });

    it("handles case-insensitive company names", () => {
        render(<CompanyAndIndustry companyName="Apple" />);
        expect(screen.getByText("Industry: AI")).toBeInTheDocument();
    });

    it("displays 'Unknown Industry' when no company name is provided", () => {
        render(<CompanyAndIndustry companyName="" />);
        expect(screen.getByText("Industry: Unknown Industry")).toBeInTheDocument();
    });

    it("matches the industry data structure", () => {
        expect(companyAndIndustries).toEqual([
            { industry: "AI", companies: ["apple", "google", "microsoft"] },
            { industry: "Finance", companies: ["goldman sachs", "jp morgan", "bank of america"] },
            { industry: "Retail", companies: ["walmart", "target", "costco"] },
        ]);
    });
});
