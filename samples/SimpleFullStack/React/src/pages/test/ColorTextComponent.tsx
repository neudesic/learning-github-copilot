import { Box, Typography } from "@mui/material";

const ColorTextComponent = ({
    amount
}: {
    amount: number
}) => {

    const determineColor = (amount: number) => {
        if (amount > 0) {
            return "rgb(0, 128, 0)";
        } else if (amount < 0) {
            return "rgb(255, 0, 0)";
        } else {
            return "rgb(128, 128, 128)";
        }
    }

    return (
        <Box sx={{
            width: '100%',
        }}>
            <Typography sx={{
                color: determineColor(amount),
                fontSize: 24,
                fontWeight: 'bold',
                marginTop: 2,
            }}>
                {amount}
            </Typography>
        </Box>
    )
}

export default ColorTextComponent