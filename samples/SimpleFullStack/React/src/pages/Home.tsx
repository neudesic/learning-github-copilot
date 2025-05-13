import { Divider, Grid } from "@mui/material"
// import CompaniesTable from "./company/CompaniesTable"
import ColorTextComponent from "./test/ColorTextComponent"

const Home = () => {
    return (
        <Grid container>
            {/* <CompaniesTable /> */}
            <ColorTextComponent amount={100} />
            <Divider />
            <ColorTextComponent amount={-100} />

        </Grid>
    )
}

export default Home