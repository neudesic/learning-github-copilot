import { Box, Card, CardContent, Typography, Button } from "@mui/material";

interface OrderCardProps {
    orderId: string;
    customerName: string;
    orderDate: string;
    totalAmount: string;
}

const OrderCard = ({ orderId, customerName, orderDate, totalAmount }: OrderCardProps) => {
    return (
        <Card
            id="order-card"
            role="presentation"
            sx={{ maxWidth: 400, margin: 2, boxShadow: 3 }}>
            <CardContent>
                <Typography variant="h6" component="div" gutterBottom>
                    Order ID: {orderId}
                </Typography>
                <Typography variant="body1" color="text.secondary">
                    Customer: {customerName}
                </Typography>
                <Typography variant="body2" color="text.secondary">
                    Date: {orderDate}
                </Typography>
                <Typography variant="body1" color="text.primary" sx={{ marginTop: 1 }}>
                    Total: {totalAmount}
                </Typography>
                <Box sx={{ marginTop: 2, textAlign: "right" }}>
                    <Button variant="contained" color="primary" size="small">
                        View Details
                    </Button>
                </Box>
            </CardContent>
        </Card>
    );
};

export default OrderCard;