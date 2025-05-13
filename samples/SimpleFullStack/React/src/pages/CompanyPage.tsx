import { Box, CircularProgress, Typography, Grid } from "@mui/material";
import { useParams } from "react-router-dom";
import { useQuery } from '@tanstack/react-query';
import { apiClient } from "service/axiosConfig";
import { OrderRequest } from "types/order";
import OrderCard from "./order/OrderCard"; // Import OrderCard component

const fetchCompany = async (companyId: string) => {
  const response = await apiClient.get<OrderRequest>(
    `/api/orders/company?companyId=${companyId}`
  );
  return response.data;
};

const CompanyPage = () => {
  const { id } = useParams<{ id: string }>();
  const { data: ordersRequest, isLoading, error } = useQuery({
    queryKey: ["orders", id],
    queryFn: () => fetchCompany(id || ""),
    enabled: !!id,
  });

  if (isLoading) return <CircularProgress />;
  if (error) return <Typography color="error">Failed to load order data</Typography>;

  return (
    <Box>
      <Typography variant="h4" gutterBottom>
        Orders for Company {id}
      </Typography>
      <Grid container spacing={2}>
        {ordersRequest?.orders.map((order) => (
          <Grid item xs={12} sm={6} md={4} key={order.orderId}>
            <OrderCard
              orderId={order.orderId}
              customerName={order.customerName}
              orderDate={order.orderDate}
              totalAmount={`$${ordersRequest?.totalAmount.toFixed(2) || '0'}`}
            />
          </Grid>
        ))}
      </Grid>
    </Box>
  );
};

export default CompanyPage;
