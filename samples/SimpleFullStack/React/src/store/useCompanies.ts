import { mockCompanies } from 'constants/mockCompanies';
import { Company } from 'types/company';
import create from 'zustand';

type CompaniesState = {
  companies: Company[];
  addCompany: (company: Company) => void;
  editCompany: (id: string, updatedCompany: Partial<Company>) => void;
  deleteCompany: (id: string) => void;
};

export const useCompanies = create<CompaniesState>(set => ({
  companies: mockCompanies,
  addCompany: company =>
    set(state => ({
      companies: [...state.companies, company],
    })),
  editCompany: (id, updatedCompany) =>
    set(state => ({
      companies: state.companies.map(company =>
        company.id === id ? { ...company, ...updatedCompany } : company,
      ),
    })),
  deleteCompany: id =>
    set(state => ({
      companies: state.companies.filter(company => company.id !== id),
    })),
}));
