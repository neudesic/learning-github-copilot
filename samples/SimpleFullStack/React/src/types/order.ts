export type Order = {
  orderId: string;
  companyId: string;
  customerName: string;
  orderDate: string; // ISO 8601 format
  totalAmount: number;
};

export type OrderRequest = {
  orders: Order[];
  totalAmount: number;
};
