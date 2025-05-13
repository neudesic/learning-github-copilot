import { useNavigate } from 'react-router-dom';
import DeleteIcon from '@mui/icons-material/Delete';
import EditIcon from '@mui/icons-material/Edit';
import Box from '@mui/material/Box';
import IconButton from '@mui/material/IconButton';
import { GridColDef, GridRowParams } from '@mui/x-data-grid';
import useGlobalModal from 'store/global/useGlobalModal';
import CompanyForm from './CompanyForm';
import GenericDataGrid from 'components/generics/GenericDataGrid';
import { Company } from 'types/company';
import { useCompanies } from 'store/useCompanies';

const CompaniesTable = ({ search }: { search?: string }) => {
  const nav = useNavigate();
  const { openGlobalModal, closeGlobalModal } = useGlobalModal();
  const { companies: data, deleteCompany } = useCompanies();

  const filteredCompanies = data?.filter(company =>
    company?.accountName?.toLowerCase().includes(search?.toLowerCase() || '')
  );

  const onRowClick = (params: GridRowParams) => {
    nav(`/company/${params.row.id}`);
  };

  const handleDelete = (id: string) => {
    openGlobalModal({
      content: 'Confirm you want to remove this company.',
      title: `Remove Company`,
      primaryAction: () => {
        deleteCompany(id);
        closeGlobalModal();
      },
      primaryActionText: 'Remove',
      dialogProps: {
        maxWidth: 'sm',
        fullWidth: true,
      }
    });
  };

  const handleEdit = (data: Company) => {
    if (data) {
      openGlobalModal({
        content: (
          <CompanyForm
            initialData={{
              id: data?.id || '',
              tpid: data?.tpid || undefined,
              accountName: data?.accountName || '',
              segment: data?.segment || '',
              industry: data?.industry || '',
              subSegment: data?.subSegment || '',
              vertical: data?.vertical || '',
            }}
          />
        ),
        title: `Edit Company`,
        formName: "company-form",
        isDirtyCheck: true,
      });
    }
  };

  const columns: GridColDef[] = [
    { field: 'tpid', headerName: 'TPID' },
    {
      field: 'accountName',
      headerName: 'Account Name',
      flex: 1,
    },
    {
      field: 'industry',
      headerName: 'Industry',
      flex: 1,
    },
    {
      field: 'vertical',
      headerName: 'Vertical',
      flex: 1,
    },
    {
      field: 'segment',
      headerName: 'Segment',
      flex: 1,
    },
    {
      field: 'subSegment',
      headerName: 'Sub Segment',
      flex: 1,
    },
    {
      field: 'action',
      headerName: '',
      width: 150,
      renderCell: params => {
        return (
          <Box
            sx={{
              display: 'flex',
              flexDirection: 'row',
              justifyContent: 'center',
              alignContent: 'center',
              gap: 1,
              width: '100%',
            }}
            data-testid="action-cell"
          >
            <IconButton
              data-testid="edit-button"
              onClick={e => {
                e.stopPropagation();
                handleEdit(params.row);
              }}
            >
              <EditIcon
                sx={{
                  fill: "main.primary",
                }}
              />
            </IconButton>
            <IconButton
              data-testid="delete-button"
              onClick={e => {
                e.stopPropagation();
                handleDelete(params.row.id);
              }}
            >
              <DeleteIcon
                sx={{
                  fill: "main.primary",
                }}
              />
            </IconButton>
          </Box>
        );
      },
    },
  ];

  return (
    <div data-testid="companies-table">
      <GenericDataGrid
        rows={filteredCompanies || []}
        columns={columns}
        onRowClick={onRowClick}
        loading={false}
      />
    </div>
  );
};

export default CompaniesTable;
