import Box from '@mui/material/Box';
import {
  DataGrid,
  GridColDef,
  GridRowParams,
  GridValidRowModel,
  MuiEvent,
} from '@mui/x-data-grid';

export type TypeSafeColDef<T> = GridColDef & { field: keyof T };

type GenericDataGridProps<T extends GridValidRowModel> = {
  rows: T[] | undefined;
  columns: GridColDef<T>[];
  onRowClick: (
    params: GridRowParams,
    _event: MuiEvent<React.MouseEvent<HTMLElement, MouseEvent>>,
  ) => void;
  loading?: boolean;
  rowCount?: number;
  paginationModel?: {
    page: number;
    pageSize: number;
  };
  onPaginationModelChange?: React.Dispatch<
    React.SetStateAction<{
      page: number;
      pageSize: number;
    }>
  >;
  getRowIdFxn?: (row: T) => string;
};
const GenericDataGrid = <T extends GridValidRowModel>({
  rows,
  columns,
  onRowClick,
  loading = false,
  rowCount,
  paginationModel,
  onPaginationModelChange,
  getRowIdFxn = undefined,
}: GenericDataGridProps<T>) => {
  return (
    <Box
      sx={{
        display: 'flex',
        flexDirection: 'column',
        gap: 3,
        width: '100%',
      }}
    >
      <DataGrid
        autoHeight
        pagination
        loading={loading || !rows}
        slotProps={{
          loadingOverlay: {
            sx: {
              backgroundColor: '#FFF',
              '& .MuiCircularProgress-svg': {
                color: "grey.400",
              },
            },
          },
        }}
        rows={rows || []}
        getRowId={getRowIdFxn}
        rowCount={rowCount}
        getRowHeight={() => 'auto'}
        columns={columns}
        pageSizeOptions={[25]}
        onRowClick={onRowClick}
        paginationMode="server"
        paginationModel={paginationModel}
        onPaginationModelChange={onPaginationModelChange}
        slots={{
          noRowsOverlay: () => (
            <Box
              height="100%"
              width="100%"
              display="flex"
              alignItems="center"
              justifyContent="center"
            >
              No Data
            </Box>
          ),
          noResultsOverlay: () => (
            <Box
              height="100%"
              width="100%"
              display="flex"
              alignItems="center"
              justifyContent="center"
            >
              No Data
            </Box>
          ),
        }}
        sx={{
          p: 0,
          borderRadius: '4px',
          backgroundColor: "common.white",
          color: 'custom.darkBlue',
          '& .MuiDataGrid-columnHeaderTitle': {
            fontWeight: '600',
          },
          '& .MuiTablePagination-actions > .MuiIconButton-root:not(.Mui-disabled) > svg':
          {
            fill: 'black',
          },
        }}
      />
    </Box>
  );
};

export default GenericDataGrid;
