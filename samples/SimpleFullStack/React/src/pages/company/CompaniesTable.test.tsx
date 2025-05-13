import { describe, it, expect, beforeEach, vi } from 'vitest';
import { render, screen, fireEvent } from '@testing-library/react';
import CompaniesTable from './CompaniesTable';
import '@testing-library/jest-dom';
import { BrowserRouter } from 'react-router-dom';

// Mock the hooks and modules
vi.mock('store/useCompanies', () => ({
  useCompanies: () => ({
    companies: [
      {
        id: '1',
        tpid: 'TPID1',
        accountName: 'Test Company',
        segment: 'Segment1',
        industry: 'Industry1',
        subSegment: 'SubSegment1',
        vertical: 'Vertical1'
      }
    ],
    deleteCompany: vi.fn(),
  }),
}));

const mockOpenGlobalModal = vi.fn();
const mockCloseGlobalModal = vi.fn();

// For default export mocking, return an object with a "default" key.
vi.mock('store/global/useGlobalModal', () => ({
  default: () => ({
    openGlobalModal: mockOpenGlobalModal,
    closeGlobalModal: mockCloseGlobalModal,
  }),
}));

const mockNavigate = vi.fn();
vi.mock('react-router-dom', async () => {
  const actual = await vi.importActual('react-router-dom');
  return {
    ...actual,
    useNavigate: () => mockNavigate,
  };
});

// Wrap component with BrowserRouter for routing context
const renderComponent = (search?: string) => {
  return render(
    <BrowserRouter>
      <CompaniesTable search={search} />
    </BrowserRouter>
  );
};

describe('CompaniesTable Component', () => {
  beforeEach(() => {
    vi.clearAllMocks();
  });

  it('renders without crashing and shows the companies table', () => {
    renderComponent();
    const tableElement = screen.getByTestId('companies-table');
    expect(tableElement).toBeInTheDocument();
  });

  it('navigates to company details on row click', () => {
    renderComponent();
    // Use the company name as a proxy for the row click
    const rowElement = screen.getByText('Test Company');
    fireEvent.click(rowElement);
    expect(mockNavigate).toHaveBeenCalledWith('/company/1');
  });

  it('opens global modal when delete button is clicked', () => {
    renderComponent();
    const deleteButton = screen.getByTestId('delete-button');
    fireEvent.click(deleteButton);
    expect(mockOpenGlobalModal).toHaveBeenCalled();
    const modalCallArg = mockOpenGlobalModal.mock.calls[0][0];
    expect(modalCallArg.title).toBe('Remove Company');
    expect(modalCallArg.content).toBe('Confirm you want to remove this company.');
  });
});
