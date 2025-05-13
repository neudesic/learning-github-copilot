import { render, screen } from "@testing-library/react";
import OrderCard from "./OrderCard";
import "@testing-library/jest-dom";

describe("OrderCard Component", () => {
    it("renders without crashing", () => {
        render(
            <OrderCard
                orderId="12345"
                customerName="John Doe"
                orderDate="2023-01-01"
                totalAmount="$100.00"
            />
        );
        const boxElement = screen.getByRole("presentation");
        expect(boxElement).toBeInTheDocument();
    });

});